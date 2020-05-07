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

            SetPinCode();
        }

        private async void SetPinCode()
        {
            Pincode Pincode = await GetPinCode(UserInfo.Id);

            Device.BeginInvokeOnMainThread(() =>
            {
                ActivePinCodeLbl.Text = Pincode.PinCode;
            });
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
    }
}
