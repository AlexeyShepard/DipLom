
using System;

namespace LOM
{
    public static class Configuration
    {
        public static string RESTUrl = "http://lomapi.isp.regruhosting.ru/api/";

        public static string ScudConnectionString = "Driver={Gemini InterBase ODBC Driver 2.0};" +
                                                    "Server=localhost;" +
                                                    "DataBase=localhost:C:\\Program Files (x86)\\Cardsoft\\DuoSE\\access\\ShieldPro.gdb;" +
                                                    "Uid=sysdba;" +
                                                    "Pwd=temp;";

        public static string AdminPanelUrl = "http://lomadmin.isp.regruhosting.ru/admin";

        public static string FileRotation = "14";
        
        public static string[] TimeGenerationPinCode = new string[] {"12:00", "13:00"};

        public static string[] TimeUpdateDatabase = new string[] { "12:00", "13:00" };

        public static string DefaultDirectoryPath = "C:\\ProgramData\\LOM";

        public static string DefaultDirectoryPathToIni = "C:\\ProgramData\\LOM\\LOM.ini";

        public static string DefaultDirectoryPathToLog = "C:\\ProgramData\\LOM\\Lom_" + DateTime.Now.ToString("yyyy-MM-dd") + ".log";

        public static string ParentOrg = "1";

        public static void SetLogFileNewName()
        {
            DefaultDirectoryPathToLog = "C:\\ProgramData\\LOM\\Lom_" + DateTime.Now.ToString("yyyy-MM-dd") + ".log";
        }
    }
}

