
using System;

namespace LOM
{
    public static class Configuration
    {
        public static string RESTUrl = "http://lomapi.isp.regruhosting.ru/api/";

        public static string RESTToken = "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiJ9.eyJhdWQiOiIyIiwianRpIjoiNGMwZGRkYjI3OGNhODIyMzdlYjFkMjc2M2EyMmNmYzZjZmJkOWVmYzBmNzQwMGUwYzdiMmExMzcwODI5MGEwN2Y1MjY0ZGExMzI5M2YxMWIiLCJpYXQiOjE1ODg2MDI3NDEsIm5iZiI6MTU4ODYwMjc0MSwiZXhwIjoxNjIwMTM4NzQxLCJzdWIiOiIxIiwic2NvcGVzIjpbXX0.V87VBpYbpE4uT5fdWhIiaOSCieFy5vdnFy4oMPkYI3jEL5SA1kR2Z46ZQVc93gYr0c0rl37y4LnOLuARp64zRSF7Z_w099gNZNNVJQYPyLt1o7FawgCH74nopY2-EFUXqOkUfX7yZOVgA34awrNEXNPqWc-pLgn_7XxCy0ySO-iQrXld5Nz1yOb3pagpVSebsML25l9hhahzc7fflo66J2bCwH-FQXSbDMJ0VqOK51OlCpagYK0X3ZsT_8BzaI6UdIgDoFtAQIdSsjqdcEddjwe_AI-ofK3oTwzKmLhyu63vxS8uvEjjcduprAMxp0diUP3V5QSNYHbfYnF66e-_GzSsXJx7TH7hzE0QSimQpDp9CCmAFktbRx36EnHBv3wTNhjV3athJ4DlRU6JfwfKPjagpfDK9IIkX98AGN4ScKO6fe5QR-tydns9kjtcreb4lti5l8jF6yMvbzpVoZQ23w7v-R1V9XD92BoSE2i9hynSRnlAW2wWNI3cZIZbucRcLhbE5p9Aqa0Vs3CDBt-7jDOebPlbvZvUjkHvmEwL1P9WFEhgJLiafZS4036nP6GEus6nNrBjQMSHNK_K_f8nlnm4CqexJynLaORhcZsWvBl1VaH5V0SMPLA2dp2M-vZkrvAWgpwt9-H-V8OeJ2g-4io7EG6O36eaKMApKfJwV-g";

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

