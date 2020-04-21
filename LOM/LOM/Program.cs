using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using IniParser;
using IniParser.Model;
using IziLog;
using IziLog.Records;

namespace LOM
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                IziLog.Configuration.PathToLogFile = Configuration.DefaultDirectoryPathToLog;
                IziLog.Configuration.FileRotation = Convert.ToInt32(Configuration.FileRotation);

                LoggerServer.Start();
                
                if (IsFirstLaunch()) FirstStartConfiguration();
                else GetConfigurationFromFile();

                string Mode = GetApplicationMode(args);
                string KeyString = args[0].ToLower();

                ExecuteApplication(KeyString);

                string Name = Process.GetCurrentProcess().ProcessName;

                Command CurrentCommand = GetCommandObject(KeyString);

                CurrentCommand.Run();

                Console.WriteLine("Нажмите любую клавишу...");

                Console.ReadKey();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }   
        }

        private static string GetApplicationMode(string[] args)
        {
            switch (args.Length)
            {
                case 0:
                    {
                        Console.WriteLine("/help - вызов справки\n/silent - запуск программы в штатном режиме\n/pin - генерация пин-кодв\n/load - Выгрузка базы данных\n/stop - остановка LOM");
                        throw new Exception("Программа запущена без использования ключей, остановка программы");
                        break;
                    }
                case 1:
                    {
                        return args[0];
                    }
                default:
                    {
                        throw new Exception("Введено недопустимое кол-во ключей");
                        break;
                    }
            }
            
                       
            return "";
        }

        private static void ExecuteApplication(string Key)
        {
            if (IsKeyExist(Key)) RunApplicationInCurrentMode(Key); 
        }

        private static bool IsKeyExist(string Key)
        {
            foreach(Command CommandObject in KeyConfiguration.List)
            {
                if (CommandObject.CommandName.Equals(Key)) return true;
            }

            throw new Exception(Key + " - такого ключа не существует!");
        }

        private static void RunApplicationInCurrentMode(string Mode)
        {
            Console.WriteLine("Программа запущена в режиме " + Mode);
        }

        private static Command GetCommandObject(string Key)
        {
            foreach(Command CommandObject in KeyConfiguration.List)
            {
                if (CommandObject.CommandName == Key) return CommandObject;
            }

            throw new NullReferenceException();
        }


        private static bool IsFirstLaunch()
        {

            if (!Directory.Exists(Configuration.DefaultDirectoryPath)) return true;
            
            return false;
        }

        private static void FirstStartConfiguration()
        {
            Directory.CreateDirectory(Configuration.DefaultDirectoryPath);

            using (FileStream fstream = new FileStream(Configuration.DefaultDirectoryPathToIni, FileMode.OpenOrCreate))
            {
                string contains = "[Main]\n" +
                    "UrlREST = " + Configuration.RESTUrl + "\n" +
                    "FileRotation = " + Configuration.FileRotation + "\n" +
                    "ScudConnectionString = " + Configuration.ScudConnectionString + "\n" +
                    "TimeGenerationPincode = " + FromArrayToString(Configuration.TimeGenerationPinCode) + "\n" +
                    "TimeUpdateDatabase = " + FromArrayToString(Configuration.TimeUpdateDatabase) + "\n" +
                    "ParentOrg = " + Configuration.ParentOrg;
                byte[] array = Encoding.Default.GetBytes(contains);
                fstream.Write(array, 0, array.Length);
            }

            Logger.Log(new InfoRecord("Процесс создания домашней директории и файла конфигурации, завершен успешно"));

            GetConfigurationFromFile();
        }

        public static void GetConfigurationFromFile()
        {
            Logger.Log(new InfoRecord("Считывание конфигурации..."));

            FileIniDataParser IniParser = new FileIniDataParser();
            IniData IniData = IniParser.ReadFile(Configuration.DefaultDirectoryPathToIni);

            Configuration.RESTUrl = IniData["Main"]["UrlREST"];
            Configuration.FileRotation = IniData["Main"]["FileRotation"];
            Configuration.ScudConnectionString = IniData["Main"]["ScudConnectionString"];
            Configuration.TimeGenerationPinCode = FromStringToArray(IniData["Main"]["TimeGenerationPincode"]);
            Configuration.TimeUpdateDatabase = FromStringToArray(IniData["Main"]["TimeUpdateDatabase"]);
            Configuration.ParentOrg = IniData["Main"]["ParentOrg"];


            Logger.Log(new InfoRecord("Считывание конфигурации, прошло успешно"));
        }

        private static string FromArrayToString(string[] TimeTable)
        {
            string FinalString = "";

            foreach (string Time in TimeTable) FinalString += Time + " ";

            return FinalString;
        }

        private static string[] FromStringToArray(string Times)
        {
            return Times.Split(' ');
        }
    }
}
