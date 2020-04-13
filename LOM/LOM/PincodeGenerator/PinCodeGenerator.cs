using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using IziLog;
using IziLog.Records;

namespace LOM
{
    class PinCodeGenerator
    {
        private static string RestMethodToGetPeopleId = "peoples";
        private static string RestMethodToPostEventsHandling = "events-handlings";

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
