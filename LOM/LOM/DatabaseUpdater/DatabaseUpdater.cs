using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using IziLog;
using IziLog.Records;
using System.Net.Http.Headers;

namespace LOM
{
    class DatabaseUpdater
    {

        private static string RestMethodToGetPinCodes = "pin-code";
        private static string RestMethodToGetPeopleId = "people";

        public static async Task iterationExecute()
        {
            try
            {
                Logger.Log(new InfoRecord("Старт обновления базы данных СКУД"));

                string JsonContent = await GetLastPinCodesInJSON();

                List<Pincode> PincodeList = JSONConvertToObjectList(JsonContent);

                DeleteOldPeople();
                InsertNewData(PincodeList);

            }
            catch (Exception ex)
            {
                Logger.Log(new ErrorRecord(ex.Message));
            }
        }

        private static async Task<string> GetLastPinCodesInJSON()
        {
            string JsonContent;

            using (HttpClient Client = new HttpClient())
            {
                Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiJ9.eyJhdWQiOiIyIiwianRpIjoiNGMwZGRkYjI3OGNhODIyMzdlYjFkMjc2M2EyMmNmYzZjZmJkOWVmYzBmNzQwMGUwYzdiMmExMzcwODI5MGEwN2Y1MjY0ZGExMzI5M2YxMWIiLCJpYXQiOjE1ODg2MDI3NDEsIm5iZiI6MTU4ODYwMjc0MSwiZXhwIjoxNjIwMTM4NzQxLCJzdWIiOiIxIiwic2NvcGVzIjpbXX0.V87VBpYbpE4uT5fdWhIiaOSCieFy5vdnFy4oMPkYI3jEL5SA1kR2Z46ZQVc93gYr0c0rl37y4LnOLuARp64zRSF7Z_w099gNZNNVJQYPyLt1o7FawgCH74nopY2-EFUXqOkUfX7yZOVgA34awrNEXNPqWc-pLgn_7XxCy0ySO-iQrXld5Nz1yOb3pagpVSebsML25l9hhahzc7fflo66J2bCwH-FQXSbDMJ0VqOK51OlCpagYK0X3ZsT_8BzaI6UdIgDoFtAQIdSsjqdcEddjwe_AI-ofK3oTwzKmLhyu63vxS8uvEjjcduprAMxp0diUP3V5QSNYHbfYnF66e-_GzSsXJx7TH7hzE0QSimQpDp9CCmAFktbRx36EnHBv3wTNhjV3athJ4DlRU6JfwfKPjagpfDK9IIkX98AGN4ScKO6fe5QR-tydns9kjtcreb4lti5l8jF6yMvbzpVoZQ23w7v-R1V9XD92BoSE2i9hynSRnlAW2wWNI3cZIZbucRcLhbE5p9Aqa0Vs3CDBt-7jDOebPlbvZvUjkHvmEwL1P9WFEhgJLiafZS4036nP6GEus6nNrBjQMSHNK_K_f8nlnm4CqexJynLaORhcZsWvBl1VaH5V0SMPLA2dp2M-vZkrvAWgpwt9-H-V8OeJ2g-4io7EG6O36eaKMApKfJwV-g");
                HttpResponseMessage response = await Client.GetAsync(Configuration.RESTUrl + RestMethodToGetPinCodes);
                JsonContent = await response.Content.ReadAsStringAsync();
            }

            Logger.Log(new InfoRecord("Был получен JSON файл с последними пин-кодами: " + JsonContent));

            return JsonContent;
        }

        private static List<Pincode> JSONConvertToObjectList(string JsonContent)
        {
            JArray JArray = JArray.Parse(JsonContent);
            return JArray.ToObject<List<Pincode>>();
        }


        private static void DeleteOldPeople()
        {
            OdbcCommand Command = new OdbcCommand("DELETE FROM people p WHERE p.note LIKE 'LOM_%'");

            ExecuteQuery(Command);

            Logger.Log(new InfoRecord("Удаление старых PEOPLE из базы данных СКУД"));
        }

        private static async void InsertNewData(List<Pincode> PincodeList)
        {
            foreach (Pincode Pin in PincodeList)
            {
                int PeopleId = await InsertNewPeople(Pin, Pin.id_People);
                InsertNewCard(Pin, PeopleId);
            }
        }

        private static async Task<int> InsertNewPeople(Pincode Pincode, int PeopleId)
        {
            People CurrentPeople = await SelectPeopleFromRest(PeopleId);

            OdbcCommand Command = new OdbcCommand("INSERT INTO people (ID_DB,ID_ORG,SURNAME,NAME,PATRONYMIC,DATEBIRTH,PLACELIFE,PLACEREG,PHONEHOME,PHONECELLULAR,PHONEWORK,NUMDOC,DATEDOC,PLACEDOC,PHOTO,WORKSTART,WORKEND,\"ACTIVE\",FLAG,LOGIN,PSWD,ID_DEVGROUP,ID_ORGCTRL,PEPTYPE,POST,PLACEBIRTH,SOUND,ID_PLAN,PRESENT,NOTE,ID_AREA,SYSNOTE)VALUES(1, " + Configuration.ParentOrg + ", '" + CurrentPeople.FirstName + "', ' " + CurrentPeople.SurName +"', '" + CurrentPeople.PatronymicName + "', '19-AUG-2008', NULL, NULL, NULL, NULL, NULL, 'X', '19-AUG-2008', 'X', NULL, '8:00:00', '17:00:00', 1, 57343, 'ACRH" + Pincode.PinCode + "', '3332', 1, 1, 1, NULL, 'X', NULL, NULL, 0, 'LOM_" + Pincode.id_People + "', 0, NULL);");

            ExecuteQuery(Command);

            Logger.Log(new InfoRecord("Вставлен People " + CurrentPeople.FirstName + " " + CurrentPeople.SurName));

            return GetPeopleId("LOM_" + Pincode.id_People);
        }

        private static void InsertNewCard(Pincode Pincode, int PeopleId)
        {
            // Pincode.PinCode = ConvertPincode(Pincode.PinCode);


            OdbcCommand Command = new OdbcCommand("INSERT INTO CARD (ID_CARD,ID_DB,ID_PEP,ID_ACCESSNAME,TIMESTART,STATUS,\"ACTIVE\", NOTE) " +
            "VALUES('" + ConvertPincode(Pincode.PinCode) + "', 1, " + PeopleId + ", 1, '" + Pincode.DateTimeGen + "', 0, 1,'" + Pincode.PinCode + "'); ");

            ExecuteQuery(Command);

            Logger.Log(new InfoRecord("Вставлен Pin-code " + Pincode.PinCode));
        }

        private static async Task<People> SelectPeopleFromRest(int PeopleId)
        {
            string JsonContent;

            using (HttpClient Client = new HttpClient())
            {
                Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiJ9.eyJhdWQiOiIyIiwianRpIjoiNGMwZGRkYjI3OGNhODIyMzdlYjFkMjc2M2EyMmNmYzZjZmJkOWVmYzBmNzQwMGUwYzdiMmExMzcwODI5MGEwN2Y1MjY0ZGExMzI5M2YxMWIiLCJpYXQiOjE1ODg2MDI3NDEsIm5iZiI6MTU4ODYwMjc0MSwiZXhwIjoxNjIwMTM4NzQxLCJzdWIiOiIxIiwic2NvcGVzIjpbXX0.V87VBpYbpE4uT5fdWhIiaOSCieFy5vdnFy4oMPkYI3jEL5SA1kR2Z46ZQVc93gYr0c0rl37y4LnOLuARp64zRSF7Z_w099gNZNNVJQYPyLt1o7FawgCH74nopY2-EFUXqOkUfX7yZOVgA34awrNEXNPqWc-pLgn_7XxCy0ySO-iQrXld5Nz1yOb3pagpVSebsML25l9hhahzc7fflo66J2bCwH-FQXSbDMJ0VqOK51OlCpagYK0X3ZsT_8BzaI6UdIgDoFtAQIdSsjqdcEddjwe_AI-ofK3oTwzKmLhyu63vxS8uvEjjcduprAMxp0diUP3V5QSNYHbfYnF66e-_GzSsXJx7TH7hzE0QSimQpDp9CCmAFktbRx36EnHBv3wTNhjV3athJ4DlRU6JfwfKPjagpfDK9IIkX98AGN4ScKO6fe5QR-tydns9kjtcreb4lti5l8jF6yMvbzpVoZQ23w7v-R1V9XD92BoSE2i9hynSRnlAW2wWNI3cZIZbucRcLhbE5p9Aqa0Vs3CDBt-7jDOebPlbvZvUjkHvmEwL1P9WFEhgJLiafZS4036nP6GEus6nNrBjQMSHNK_K_f8nlnm4CqexJynLaORhcZsWvBl1VaH5V0SMPLA2dp2M-vZkrvAWgpwt9-H-V8OeJ2g-4io7EG6O36eaKMApKfJwV-g");
                HttpResponseMessage response = await Client.GetAsync(Configuration.RESTUrl + RestMethodToGetPeopleId + "/" + PeopleId);
                JsonContent = await response.Content.ReadAsStringAsync();
            }

            //JArray JArray = JArray.Parse(JsonContent);
            JObject JObject = JObject.Parse(JsonContent);
            return JObject.ToObject<People>();
        }

        private static int GetPeopleId(string LomId)
        {
            OdbcCommand Command = new OdbcCommand("SELECT id_pep FROM people p WHERE p.note = '" + LomId + "'");

            object ScudId;

            using (OdbcConnection Connection = new OdbcConnection(Configuration.ScudConnectionString))
            {
                Command.Connection = Connection;
                Connection.Open();
                ScudId = Command.ExecuteScalar();
            }

            return (int)ScudId;
        }

        private static void ExecuteQuery(OdbcCommand Command)
        {
            using (OdbcConnection Connection = new OdbcConnection(Configuration.ScudConnectionString))
            {
                Command.Connection = Connection;
                Connection.Open();
                Command.ExecuteNonQuery();
            }
        }

        private static string ConvertPincode(string Pincode)
        {                                
            string result = "";
            byte[] HexString = new byte[Pincode.Length];

            for(int i = 0; i < Pincode.Length; i++) HexString[i] = Convert.ToByte(int.Parse(Pincode[i].ToString()));

            Array.Reverse(HexString);

            foreach (byte symbol in HexString)
            {
                string cash = Convert.ToString(symbol, 2).PadLeft(4, '0');

                char[] inputarray = cash.ToCharArray();
                Array.Reverse(inputarray);
                string output = new string(inputarray);

                string hex = Convert.ToInt32(output, 2).ToString("X");

                result += hex;
            }

            return result + "00001A";
        }
    }
}
