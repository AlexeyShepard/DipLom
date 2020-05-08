using System;
using System.Threading;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LomMobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegistrationPage : ContentPage
    {
        public RegistrationPage()
        {
            InitializeComponent();

            StartCheckInternetStatus();
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
                        if (!InternetChecker.IsInternetConnected && !InternetChecker.PageShowed) RegBtn.IsEnabled = false;
                        else if (InternetChecker.IsInternetConnected && InternetChecker.PageShowed) RegBtn.IsEnabled = true;
                    });
                }
            });
        }

        private async void RegBtn_Clicked(Object sender, EventArgs e)
        {
            SurNameErrorLbl.Text = "";
            FirstNameErrorLbl.Text = "";
            PatronymicNameErrorLbl.Text = "";
            LoginErrorLbl.Text = "";
            PasswordErrorLbl.Text = "";
            RepeatPasswordErrorLbl.Text = "";

            bool IsValid = IsValidate();
            bool IsRegistred = await IsRegistered();

            if (IsValid && IsRegistred) await Navigation.PopModalAsync();
        }

        private bool IsValidate()
        {
            bool SurName = true, FirstName = true, PatronymicName = true, Login = true, Password = true, RepeatPassword = true; 

            if(SurNameEntr.Text == null || SurNameEntr.Text == "")
            {
                SurNameErrorLbl.Text = "Введите фамилию";
                SurName = false;
            }
            else if (SurNameEntr.Text == null || SurNameEntr.Text.Length < 3)
            {
                SurNameErrorLbl.Text = "Фамилия должна быть длинее 3-ёх символов";
                SurName = false;
            }

                if (FirstNameEntr.Text == null || FirstNameEntr.Text == "") 
            {
                FirstNameErrorLbl.Text = "Введите имя";
                FirstName = false;
            }
            else if (FirstNameEntr.Text == null || FirstNameEntr.Text.Length < 3) 
            {
                FirstNameErrorLbl.Text = "Имя должно быть длинее 2-ух символов";
                FirstName = false;
            }

            if(LoginEntr.Text == null || LoginEntr.Text == "")
            {         
                LoginErrorLbl.Text = "Введите логин";
                Login = false;
            }
            else if (LoginEntr.Text == null || LoginEntr.Text.Length < 6)
            {
                LoginErrorLbl.Text = "Логин должен быть длинее 6-ти символов";
                Login = false;
            }

            if (PasswordEntr.Text == null || PasswordEntr.Text == "")
            {        
                PasswordErrorLbl.Text = "Введите пароль";
                Password = false;
            }
            else if (PasswordEntr.Text == null || PasswordEntr.Text.Length < 6)
            {
                PasswordErrorLbl.Text = "Пароль должен быть длинее 6-ти символов";
                Password = false;
            }

            if (RepeatPasswordEntr.Text == null || PasswordEntr.Text != RepeatPasswordEntr.Text)
            {
                RepeatPasswordErrorLbl.Text = "Пароли не совпадают";
                RepeatPassword = false;
            }

            if (SurName && FirstName && PatronymicName && Login && Password && RepeatPassword) return true;
            else return false;
        }

        private async Task<bool> IsRegistered()
        {
            try
            {
                User User = await GetUserAsync();

                if (User.id != 0) return true;
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
                HttpRequestMessage Request = new HttpRequestMessage(HttpMethod.Post, Configuration.RESTUrl + "people");
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
            keyValues.Add(new KeyValuePair<string, string>("SurName", SurNameEntr.Text));
            keyValues.Add(new KeyValuePair<string, string>("FirstName", FirstNameEntr.Text));
            keyValues.Add(new KeyValuePair<string, string>("PatronymicName", PatronymicNameEntr.Text));
            keyValues.Add(new KeyValuePair<string, string>("Login", LoginEntr.Text));
            keyValues.Add(new KeyValuePair<string, string>("Password", PasswordEntr.Text));

            return new FormUrlEncodedContent(keyValues);
        }

        private async void BackBtn_Clicked(Object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}