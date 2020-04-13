using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using IziLog;
using IziLog.Records;

namespace LOM
{
    class SilentCommand : Command
    {
        #region Для функции скрытия консольного окна

        const int SW_HIDE = 0;
        const int SW_SHOW = 5;
        const int SW_Min = 2;
        const int SW_Max = 3;
        const int SW_Norm = 4;

        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        #endregion

        private static bool PinCodeAlreadyGenerated = false;

        private static bool DatabaseAlreadyUpdated = false;

        private static int SecondsLeftToUpdateConfiguration = 60;

        public override string CommandName { get; set; } = "/silent";

        public async override Task Run()
        {
            try
            {
                if (IsAlreadyRunned())
                {
                    Logger.Log(new InfoRecord("Запуск LOM.exe в режиме /silent"));
                    HideConsole();
                    Main();
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Run() - " + ex.Message);
            }         
        }
        private void HideConsole()
        {

            try
            {
                var handle = GetConsoleWindow();

                //скрыть консоль
                ShowWindow(handle, SW_HIDE);

                Logger.Log(new InfoRecord("Скрытие консоли"));
            }
            catch (Exception ex)
            {
                Console.WriteLine("HideConsole() - " + ex.Message);
            }         
        }

        private bool IsAlreadyRunned()
        {
            try
            {
                int CountProcess = 0;

                Process[] ProcessList = Process.GetProcesses();

                foreach (Process ProcessObject in ProcessList)
                {
                    if (ProcessObject.ProcessName == "LOM") CountProcess++;
                }

                if (CountProcess == 1) return true;

                throw new Exception("Не возможно запустить ещё один экземпляр программы");
            }
            catch (Exception ex)
            {
                Console.WriteLine("IsAlreadyRunned() - " + ex.Message);
                return false;
            }        
        }

        private async void Main()
        {
            try
            {
                while (true)
                {
                    SecondsLeftToUpdateConfiguration--;

                    if (SecondsLeftToUpdateConfiguration == 0)
                    {
                        Configuration.SetLogFileNewName();
                        Program.GetConfigurationFromFile();
                        SecondsLeftToUpdateConfiguration = 60;
                    }

                    await GenerateAndUpdateAsync();

                    string CashTime = DateTime.Now.ToString("H:mm");

                    Thread.Sleep(1000);

                    if(DateTime.Now.ToString("H:mm") != CashTime)
                    {
                        DatabaseAlreadyUpdated = false;
                        PinCodeAlreadyGenerated = false;
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Main() - " + ex.Message);
            }          
        }

        private async Task GenerateAndUpdateAsync()
        {
            try
            {
                //Logger.Log(new InfoRecord("IsTimeToGeneratePincode() = " + IsTimeToGeneratePincode() + " IsPinCodeAlreadyGenerated() = " + IsPinCodeAlreadyGenerated()));
                //Logger.Log(new InfoRecord("IsTimeToUpdateDatabase() = " + IsTimeToUpdateDatabase() + "IsDatabaseAlreadyUpdated() = " + IsDatabaseAlreadyUpdated()));
                 
                if (IsTimeToGeneratePincode() && !IsPinCodeAlreadyGenerated())
                {
                    PinCodeAlreadyGenerated = true;
                    //await PinCodeGenerator.IterationExecute();
                    Task.WaitAll(PinCodeGenerator.IterationExecute());                   
                }

                if (IsTimeToUpdateDatabase() && !IsDatabaseAlreadyUpdated())
                {
                    DatabaseAlreadyUpdated = true;
                    await DatabaseUpdater.iterationExecute();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("GenerateAndUpdateAync() - " + ex.Message);
            }            
        } 

        private bool IsTimeToGeneratePincode()
        {
            try
            {
                foreach (string Time in Configuration.TimeGenerationPinCode)
                {
                    if (Time == DateTime.Now.ToString("H:mm")) return true;
                }

                PinCodeAlreadyGenerated = false;
                return false;
            }
            catch(Exception ex)
            {
                Console.WriteLine("IsTimeToGeneratePincode() - " + ex.Message);
                return false;
            }                 
        }

        private bool IsTimeToUpdateDatabase()
        {
            try
            {
                foreach (string Time in Configuration.TimeUpdateDatabase)
                {
                    if (Time == DateTime.Now.ToString("H:mm")) return true;
                }

                DatabaseAlreadyUpdated = false;
                return false;
            }
            catch(Exception ex)
            {
                Console.WriteLine("IsTimeToUpdataDatabase() - " + ex.Message);
                return false;
            }           
        }

        private bool IsPinCodeAlreadyGenerated()
        {
            return PinCodeAlreadyGenerated;
        }

        private bool IsDatabaseAlreadyUpdated()
        {
            return DatabaseAlreadyUpdated;
        }

    }
}
