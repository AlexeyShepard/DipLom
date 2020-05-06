
using System;

namespace LomConfig
{
    public static class Configuration
    {
        public static string RESTUrl = "http://lomapi.isp.regruhosting.ru//api/";

        public static string RESTToken = "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiJ9.eyJhdWQiOiIxIiwianRpIjoiM2RlOWIxZTM5ZTZkOTI3N2I4NjM1YmVlZTE2N2JmYWMxNzU2NGE3Nzg3NzQ4N2JlZjY4ZTllOGFlZTU1Yzg5Njk4ZDJkYzM4OGEyZWUwMmEiLCJpYXQiOjE1ODg2MDE0OTUsIm5iZiI6MTU4ODYwMTQ5NSwiZXhwIjoxNjIwMTM3NDk1LCJzdWIiOiIxIiwic2NvcGVzIjpbXX0.OMtVubD9kN--ak4uXlc5h82erCZD3Pd8lJ440PWgiW5e5oW2sCibN33xr5KBGoR_VeZc55yboxqvEcu5J8X8ny4gvi3ML8O3Gkzwh3S9pDOyGbDU2tTsOSSOL3d9g96QoDJHY7rkqnQWSVtwFUw0Svhiy-xUkvpJDuMy3acy-gRMbjMlwzIzEOEBx1spn9T6uxQrM1BWuk3cW_qZC_cNObLj4Ylyd_riKwn4loPPVyQsTKguTfqif9ub45TMewcoo2qzGRR1AhRJWXCBGeP_s4BwH6OGnGqJkHOHCK0aeHgFeHteqGy_FVkyNa5luuSJFGpU0wk1mLVLD9Yh3ty632NDM83N-nqTFqlxUdlt_LR8oND4lCPaoVFnDFwZH-QvEz-r_NgsFEX6KjGubL3euiP0aXhkRpQFmBLIw6AQAdntqfoeub9BiEbH-t_aZy7UWjUOQxp5XEdZLS_cR2h2uic5goGAbsgNGGmZjrHQfrcHp9BKDnpMG7v_YynQAzLwXKVSFoVGC_mnMwiBb0SR5UJGp8Bvfy2H6r0vh-j_p094ISyzhWTf5Ah0SBDNfIcAuVqsMJtYhWWGcd5x-BkLIaBDLzL_I71VizuK-7BAslKK2VMbBSY8dRVDjbyLCarFDQS_ZLqxWEfx0cheLtwoDKnQfuPKMswMgM4ljjH2d8M";

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

