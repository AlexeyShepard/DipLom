using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using IziLog;
using IziLog.Records;
using System.Net;
using System.Net.Http.Headers;

namespace LOM
{
    class PinCodeGenerator
    {
        private static string RestMethodToGetPeopleId = "people";
        private static string RestMethodToPostEventsHandling = "events-handling";

        public static async Task IterationExecute()
        {
            try
            {
                Logger.Log(new InfoRecord("Старт генерации пин-кодов в базе данных СКУД"));
                string JsonContent = await GetPeopleId();
                List<People> Peoples = JSONConvertToIntList(JsonContent);

                SendRequestToGeneratePincode(Peoples);
            }
            catch (Exception ex)
            {
                Logger.Log(new ErrorRecord(ex.Message));
            }
        }

        private static async Task<string> GetPeopleId()
        {
            string JsonContent;

            using (HttpClient Client = new HttpClient())
            {
                Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiJ9.eyJhdWQiOiIyIiwianRpIjoiNGMwZGRkYjI3OGNhODIyMzdlYjFkMjc2M2EyMmNmYzZjZmJkOWVmYzBmNzQwMGUwYzdiMmExMzcwODI5MGEwN2Y1MjY0ZGExMzI5M2YxMWIiLCJpYXQiOjE1ODg2MDI3NDEsIm5iZiI6MTU4ODYwMjc0MSwiZXhwIjoxNjIwMTM4NzQxLCJzdWIiOiIxIiwic2NvcGVzIjpbXX0.V87VBpYbpE4uT5fdWhIiaOSCieFy5vdnFy4oMPkYI3jEL5SA1kR2Z46ZQVc93gYr0c0rl37y4LnOLuARp64zRSF7Z_w099gNZNNVJQYPyLt1o7FawgCH74nopY2-EFUXqOkUfX7yZOVgA34awrNEXNPqWc-pLgn_7XxCy0ySO-iQrXld5Nz1yOb3pagpVSebsML25l9hhahzc7fflo66J2bCwH-FQXSbDMJ0VqOK51OlCpagYK0X3ZsT_8BzaI6UdIgDoFtAQIdSsjqdcEddjwe_AI-ofK3oTwzKmLhyu63vxS8uvEjjcduprAMxp0diUP3V5QSNYHbfYnF66e-_GzSsXJx7TH7hzE0QSimQpDp9CCmAFktbRx36EnHBv3wTNhjV3athJ4DlRU6JfwfKPjagpfDK9IIkX98AGN4ScKO6fe5QR-tydns9kjtcreb4lti5l8jF6yMvbzpVoZQ23w7v-R1V9XD92BoSE2i9hynSRnlAW2wWNI3cZIZbucRcLhbE5p9Aqa0Vs3CDBt-7jDOebPlbvZvUjkHvmEwL1P9WFEhgJLiafZS4036nP6GEus6nNrBjQMSHNK_K_f8nlnm4CqexJynLaORhcZsWvBl1VaH5V0SMPLA2dp2M-vZkrvAWgpwt9-H-V8OeJ2g-4io7EG6O36eaKMApKfJwV-g");
                HttpResponseMessage response = await Client.GetAsync(Configuration.RESTUrl + RestMethodToGetPeopleId);
                JsonContent = await response.Content.ReadAsStringAsync();
            }

            Logger.Log(new InfoRecord("Был получен JSON файл с PEOPLE: " + JsonContent));

            return JsonContent;
        }

        private static List<People> JSONConvertToIntList(string JsonContent)
        {
            JArray JArray = JArray.Parse(JsonContent);
            return JArray.ToObject<List<People>>();
        }

        private static async void SendRequestToGeneratePincode(List<People> Peoples)
        {
            using (HttpClient Client = new HttpClient())
            {
                foreach (People People in Peoples)
                {
                    Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiJ9.eyJhdWQiOiIyIiwianRpIjoiNGMwZGRkYjI3OGNhODIyMzdlYjFkMjc2M2EyMmNmYzZjZmJkOWVmYzBmNzQwMGUwYzdiMmExMzcwODI5MGEwN2Y1MjY0ZGExMzI5M2YxMWIiLCJpYXQiOjE1ODg2MDI3NDEsIm5iZiI6MTU4ODYwMjc0MSwiZXhwIjoxNjIwMTM4NzQxLCJzdWIiOiIxIiwic2NvcGVzIjpbXX0.V87VBpYbpE4uT5fdWhIiaOSCieFy5vdnFy4oMPkYI3jEL5SA1kR2Z46ZQVc93gYr0c0rl37y4LnOLuARp64zRSF7Z_w099gNZNNVJQYPyLt1o7FawgCH74nopY2-EFUXqOkUfX7yZOVgA34awrNEXNPqWc-pLgn_7XxCy0ySO-iQrXld5Nz1yOb3pagpVSebsML25l9hhahzc7fflo66J2bCwH-FQXSbDMJ0VqOK51OlCpagYK0X3ZsT_8BzaI6UdIgDoFtAQIdSsjqdcEddjwe_AI-ofK3oTwzKmLhyu63vxS8uvEjjcduprAMxp0diUP3V5QSNYHbfYnF66e-_GzSsXJx7TH7hzE0QSimQpDp9CCmAFktbRx36EnHBv3wTNhjV3athJ4DlRU6JfwfKPjagpfDK9IIkX98AGN4ScKO6fe5QR-tydns9kjtcreb4lti5l8jF6yMvbzpVoZQ23w7v-R1V9XD92BoSE2i9hynSRnlAW2wWNI3cZIZbucRcLhbE5p9Aqa0Vs3CDBt-7jDOebPlbvZvUjkHvmEwL1P9WFEhgJLiafZS4036nP6GEus6nNrBjQMSHNK_K_f8nlnm4CqexJynLaORhcZsWvBl1VaH5V0SMPLA2dp2M-vZkrvAWgpwt9-H-V8OeJ2g-4io7EG6O36eaKMApKfJwV-g");
                    HttpRequestMessage Request = new HttpRequestMessage(HttpMethod.Post, Configuration.RESTUrl + RestMethodToPostEventsHandling);
                    Request.Content = GenerateJsonOfEventHandling(People);

                    HttpResponseMessage Response = await Client.SendAsync(Request);

                    Logger.Log(new InfoRecord("Генерация пин-кода для " + People.FirstName + " " + People.SurName));
                }
            }
        }

        private static FormUrlEncodedContent GenerateJsonOfEventHandling(People People)
        {
            int status = 110;

            var keyValues = new List<KeyValuePair<string, string>>();
            keyValues.Add(new KeyValuePair<string, string>("id_People", People.id.ToString()));
            keyValues.Add(new KeyValuePair<string, string>("id_EventType", status.ToString()));
            keyValues.Add(new KeyValuePair<string, string>("HumanOrder", "Нет"));

            return new FormUrlEncodedContent(keyValues);
        }
    }
}
