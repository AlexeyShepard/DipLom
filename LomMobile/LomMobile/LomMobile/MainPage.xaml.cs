using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Newtonsoft.Json.Linq;
using LomMobile.System;

namespace LomMobile
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            FIOLbl.Text = UserInfo.FirstName + " " + UserInfo.SurName;

            SetPinCode();
        }

        private async void SetPinCode()
        {
            try
            {
                Pincode Pincode = await GetPinCode(UserInfo.Id);

                Device.BeginInvokeOnMainThread(() =>
                {
                    ActivePinCodeLbl.Text = Pincode.PinCode;
                });
            }
            catch(Exception ex)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    ActivePinCodeLbl.Text = "Пин-код отсутствует";
                });
            }          
        }
        
        private async Task<Pincode> GetPinCode(int UserId)
        {
            string JsonContent;

            using (HttpClient Client = new HttpClient())
            {
                Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Configuration.RESTToken);
                HttpResponseMessage response = await Client.GetAsync(Configuration.RESTUrl + "pin-code/" + UserId.ToString());
                JsonContent = await response.Content.ReadAsStringAsync();
            }

            JObject JObject = JObject.Parse(JsonContent);
            return JObject.ToObject<Pincode>();
        }

        private void ReFreshBtn_Clicked(Object sender, EventArgs e)
        {
            SetPinCode();
        }

        private async void GenerateBtn_Clicked(Object sender, EventArgs e)
        {
            try
            {
                using (HttpClient Client = new HttpClient())
                {
                    Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Configuration.RESTToken);
                    HttpRequestMessage Request = new HttpRequestMessage(HttpMethod.Post, Configuration.RESTUrl + "events-handling");
                    Request.Content = GenerateJsonOfEventHandling(UserInfo.Id);

                    HttpResponseMessage Response = await Client.SendAsync(Request);
                }
            }
            catch
            {

            }
            finally
            {
                SetPinCode();
            }
        }

        private static FormUrlEncodedContent GenerateJsonOfEventHandling(int Id)
        {
            int status = 110;

            var keyValues = new List<KeyValuePair<string, string>>();
            keyValues.Add(new KeyValuePair<string, string>("id_People", Id.ToString()));
            keyValues.Add(new KeyValuePair<string, string>("id_EventType", status.ToString()));
            keyValues.Add(new KeyValuePair<string, string>("HumanOrder", "Нет"));

            return new FormUrlEncodedContent(keyValues);
        }
    }
}
