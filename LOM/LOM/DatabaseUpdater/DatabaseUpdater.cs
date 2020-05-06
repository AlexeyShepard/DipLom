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
                Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Configuration.RESTToken);
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
                Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Configuration.RESTToken);
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
