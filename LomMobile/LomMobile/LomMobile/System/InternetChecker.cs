using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace LomMobile
{
    public static class InternetChecker
    {
        public static bool IsInternetConnected;

        public static bool PageShowed;
        
        public async static Task Start()
        {
            await Task.Run(() =>
            {
                while (true)
                {
                    Check();
                    Thread.Sleep(100);
                }
            });       
        }

        public static void Check()
        {
            var current = Connectivity.NetworkAccess;

            if (current == NetworkAccess.Internet) IsInternetConnected = true;
            else IsInternetConnected = false;
        }
    }
}
