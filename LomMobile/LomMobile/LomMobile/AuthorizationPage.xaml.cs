using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using LomMobile.System;
using Newtonsoft.Json.Linq;
using Xamarin.Essentials;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LomMobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AuthorizationPage : ContentPage
    {
        public AuthorizationPage()
        {
            InitializeComponent();

            StartCheckInternet();

            StartCheckInternetStatus();
        }

        private async void StartCheckInternet()
        {
            await InternetChecker.Start();
        }

        private async void StartCheckInternetStatus()
        {
            await Task.Run(() =>
            {
                while (true)
                {
                    Thread.Sleep(100);
                    

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        if (!InternetChecker.IsInternetConnected && !InternetChecker.PageShowed)
                        {
                            Navigation.PushModalAsync(new NoInternetPage());
                            InternetChecker.PageShowed = true;
                            LoginBtn.IsEnabled = false;
                            ToRegistrationBtn.IsEnabled = false;
                        }
                        else if (InternetChecker.IsInternetConnected && InternetChecker.PageShowed)
                        {
                            Navigation.PopModalAsync();
                            InternetChecker.PageShowed = false;
                            LoginBtn.IsEnabled = true;
                            ToRegistrationBtn.IsEnabled = true;
                        }
                    });
                }
            });
        }
        private bool IsValidate()
        {
            bool Password = true, Login = true;
            
            if (LoginEntr.Text == null || LoginEntr.Text == "")
            {
                LoginErrorLbl.Text = "Введите логин";
                Password = false;
            }
            if (PasswordEntr.Text == null || PasswordEntr.Text == "")
            {
                PasswordErrorLbl.Text = "Введите пароль";
                Login = false;
            }

            if (Password && Login) return true;
            else return false;
        }

        private async Task<bool> IsAuthorized()
        {
            try
            {
                User User = await GetUserAsync();

                if (User.id != 0)
                {
                    UserInfo.Id = User.id;
                    UserInfo.FirstName = User.FirstName;
                    UserInfo.SurName = User.SurName;
                    UserInfo.PatronymicName = User.PatronymicName;
                    
                    return true;
                } 
                else return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        private async Task<User> GetUserAsync()
        {

            string JsonContent;

            using (HttpClient Client = new HttpClient())
            {
                HttpRequestMessage Request = new HttpRequestMessage(HttpMethod.Post, Configuration.RESTUrl + "login");
                Request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Configuration.RESTToken);
                Request.Content = GenerateJsonForLogin();

                HttpResponseMessage Response = await Client.SendAsync(Request);

                JsonContent = await Response.Content.ReadAsStringAsync();
            }

            JObject JObject = JObject.Parse(JsonContent);
            return JObject.ToObject<User>();
        }

        private FormUrlEncodedContent GenerateJsonForLogin()
        {
            var keyValues = new List<KeyValuePair<string, string>>();
            keyValues.Add(new KeyValuePair<string, string>("Login", LoginEntr.Text));
            keyValues.Add(new KeyValuePair<string, string>("Password", PasswordEntr.Text));

            return new FormUrlEncodedContent(keyValues);
        }

        private async Task lmao()
        {
            await Task.Run(() =>
            {
                Thread.Sleep(2000);

                Device.BeginInvokeOnMainThread(() =>
                {
                    LoginErrorLbl.Text = "Вроде работает!";
                });            
            });             
        }

        private async void LoginBtn_Clicked(Object sender, EventArgs e)
        {
            UserInfo.FirstName = "";
            UserInfo.Id = 0;
            UserInfo.PatronymicName = "";
            UserInfo.PinCode = "";
            UserInfo.SurName = "";

            LoginErrorLbl.Text = "";
            PasswordErrorLbl.Text = "";
            LoginResultLbl.Text = "";

            bool IsValid = IsValidate();
            bool IsAuth = await IsAuthorized();

            if (IsValid && IsAuth) await Navigation.PushAsync(new MainPage());
            else if (IsValid) LoginResultLbl.Text = "Неверный логин или пароль";

        }

        private async void ToRegistrationBtn_Clicked(Object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new RegistrationPage());
        }
    }
}