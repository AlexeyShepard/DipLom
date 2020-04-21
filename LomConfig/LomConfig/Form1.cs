using System.IO;
using System.Text;
using System.Windows.Forms;
using IniParser;
using IniParser.Model;
using IziLog;
using IziLog.Records;
using System.Data.Odbc;
using System;
using System.Diagnostics;
using System.Net.Http;

namespace LomConfig
{
    public partial class Form1 : Form
    {
        private static string RestMethodToGetPeopleId = "peoples";

        public Form1()
        {
            InitializeComponent();
           
            IziLog.Configuration.PathToLogFile = Configuration.DefaultDirectoryPathToLog;
            IziLog.Configuration.FileRotation = Convert.ToInt32(Configuration.FileRotation);

            Configuration.SetLogFileNewName();

            LoggerServer.Start();

            if (IsFirstLaunch()) FirstStartConfiguration();
            else GetConfigurationFromFile();
        }

        private bool IsFirstLaunch()
        {

            if (!Directory.Exists(Configuration.DefaultDirectoryPath)) return true;

            return false;
        }

        private void FirstStartConfiguration()
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

        public void GetConfigurationFromFile()
        {
            Logger.Log(new InfoRecord("Считывание конфигурации, приложением LomConfig..."));

            FileIniDataParser IniParser = new FileIniDataParser();
            IniData IniData = IniParser.ReadFile(Configuration.DefaultDirectoryPathToIni);

            Configuration.RESTUrl = IniData["Main"]["UrlREST"];
            Configuration.FileRotation = IniData["Main"]["FileRotation"];
            Configuration.ScudConnectionString = IniData["Main"]["ScudConnectionString"];
            Configuration.TimeGenerationPinCode = FromStringToArray(IniData["Main"]["TimeGenerationPincode"]);
            Configuration.TimeUpdateDatabase = FromStringToArray(IniData["Main"]["TimeUpdateDatabase"]);
            Configuration.ParentOrg = IniData["Main"]["ParentOrg"];


            Logger.Log(new InfoRecord("Считывание конфигурации, приложением LomConfig завершенно, прошло успешно"));

            InsertConfigurationToTheField();
        }

        private void InsertConfigurationToTheField()
        {
            UrlRestTbx.Text = Configuration.RESTUrl;
            CkydConnTxb.Text = Configuration.ScudConnectionString;

            FileRotationUpD.Value = Convert.ToInt32(Configuration.FileRotation);

            foreach (string time in Configuration.TimeGenerationPinCode) PinGenTimeTableLbx.Items.Add(time);

            foreach (string time in Configuration.TimeUpdateDatabase) DBUpdateTimeTableLbx.Items.Add(time);

            ParentOrgCbx.Items.AddRange(GetOrganizationList());

            foreach(var Item in ParentOrgCbx.Items)
            {
                string ItemOrgStr = Item.ToString();
                char[] ItemOrgChars = ItemOrgStr.ToCharArray();
                if (ItemOrgChars[0].ToString() == Configuration.ParentOrg) ParentOrgCbx.SelectedItem = Item;
            }
        }

        private static string[] GetOrganizationList()
        {
            object StringsCount;


            OdbcCommand Command = new OdbcCommand("SELECT COUNT(*) FROM ORGANIZATION");

            using (OdbcConnection Connection = new OdbcConnection(Configuration.ScudConnectionString))
            {
                Command.Connection = Connection;
                Connection.Open();
                StringsCount = Command.ExecuteScalar();
            }

            string[] items = new string[Convert.ToInt32(StringsCount)];

            Command = new OdbcCommand("SELECT * FROM ORGANIZATION");
            OdbcDataReader reader;

            using (OdbcConnection Connection = new OdbcConnection(Configuration.ScudConnectionString))
            {

                Command.Connection = Connection;
                Connection.Open();
                reader = Command.ExecuteReader();

                if(reader.HasRows)
                {
                        int i = 0;
                        items = new string[Convert.ToInt32(StringsCount)];

                        while (reader.Read())
                        {
                            items[i] += reader.GetValue(0) + " - " + reader.GetValue(2);
                            i++;
                        }
                    }

                    reader.Close();
                }

            return items;
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

        #region Events

        private void AddPinCodeGenTimeBtn_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime Time = Convert.ToDateTime(PinGenMTbx.Text);
                PinGenTimeTableLbx.Items.Add(Time.ToString("H:mm"));
            }
            catch(Exception ex)
            {
                MessageBox.Show("Неверный формат данных!");
            }
        }

        private void DeletePinCodeGenTimeBtn_Click(object sender, EventArgs e)
        {
            PinGenTimeTableLbx.Items.Remove(PinGenTimeTableLbx.SelectedItem);
        }

        private void AddDBUpdateTimeBtn_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime Time = Convert.ToDateTime(DBUpdaterMTbx.Text);
                DBUpdateTimeTableLbx.Items.Add(Time.ToString("H:mm"));
            }
            catch(Exception ex)
            {
                MessageBox.Show("Неверный формат данных!");
            }           
        }

        private void DeleteDBUpdateTimeBtn_Click(object sender, EventArgs e)
        {
            DBUpdateTimeTableLbx.Items.Remove(DBUpdateTimeTableLbx.SelectedItem);
        }     

        private void SaveConfBtn_Click(object sender, EventArgs e)
        {
            try
            {
                FileIniDataParser IniParser = new FileIniDataParser();
                IniData IniData = new IniData();

                Configuration.RESTUrl = UrlRestTbx.Text;
                Configuration.ScudConnectionString = CkydConnTxb.Text;

                Configuration.FileRotation = FileRotationUpD.Value.ToString();

                string[] cash = new string[PinGenTimeTableLbx.Items.Count];
                PinGenTimeTableLbx.Items.CopyTo(cash, 0);
                Configuration.TimeGenerationPinCode = cash;


                cash = new string[DBUpdateTimeTableLbx.Items.Count];
                DBUpdateTimeTableLbx.Items.CopyTo(cash, 0);
                Configuration.TimeUpdateDatabase = cash;


                string SelectedOrg = ParentOrgCbx.SelectedItem.ToString();
                char[] SelectedOrdChars = SelectedOrg.ToCharArray();
                Configuration.ParentOrg = SelectedOrdChars[0].ToString();

                IniData["Main"]["UrlREST"] = Configuration.RESTUrl;
                IniData["Main"]["FileRotation"] = Configuration.FileRotation;
                IniData["Main"]["ScudConnectionString"] = Configuration.ScudConnectionString;
                IniData["Main"]["TimeGenerationPincode"] = FromArrayToString(Configuration.TimeGenerationPinCode);
                IniData["Main"]["TimeUpdateDatabase"] = FromArrayToString(Configuration.TimeUpdateDatabase);
                IniData["Main"]["ParentOrg"] = Configuration.ParentOrg;

                IniParser.WriteFile(Configuration.DefaultDirectoryPathToIni, IniData);

                Logger.Log(new InfoRecord("Ручное изменение конфигурации, прошло успешно"));
                MessageBox.Show("Сохранено успешно!");
            }
            catch(Exception ex)
            {
                Logger.Log(new ErrorRecord(ex.Message));
                MessageBox.Show(ex.Message);
            }           
        }

        private void CheckConnectionTbx_Click(object sender, EventArgs e)
        {
            try
            {
                OdbcCommand Command = new OdbcCommand("SELECT * FROM people");

                object ScudId;

                using (OdbcConnection Connection = new OdbcConnection(Configuration.ScudConnectionString))
                {
                    Command.Connection = Connection;
                    Connection.Open();
                    OdbcDataReader OdbcReader = Command.ExecuteReader();
                }

                Logger.Log(new InfoRecord("Проверка подключения к базе данных СКУД прошло успешно"));
                MessageBox.Show("Статус: ОК\nОписание: Подключние успешно установлено!", "Проверка соединения со СКУД");
            }
            catch (Exception ex)
            {
                Logger.Log(new ErrorRecord("Проверка подключения к базе данных СКУД прошло с ошибкой " + ex.Message));
                MessageBox.Show("Статус: Ошибка\nОписание: Произошёл сбой в подключение! " + ex.Message, "Проверка соединения со СКУД");
            }        
        }

        private async void CheckConnectionRestBtn_Click(object sender, EventArgs e)
        {
            try
            {
                string JsonContent;

                using (HttpClient Client = new HttpClient())
                {
                    HttpResponseMessage response = await Client.GetAsync(Configuration.RESTUrl + RestMethodToGetPeopleId);
                    JsonContent = await response.Content.ReadAsStringAsync();
                }

                Logger.Log(new InfoRecord("Проверка подключения c REST прошло успешно"));
                MessageBox.Show("Статус: ОК\nОписание: Подключние успешно установлено!", "Проверка соединения с REST");
            }
            catch(Exception ex)
            {
                Logger.Log(new ErrorRecord("Проверка подключения c REST прошло с ошибкой " + ex.Message));
                MessageBox.Show("Статус: Ошибка\nОписание: Произошёл сбой в подключение! " + ex.Message, "Проверка соединения с Rest");
            }
        }

        #endregion

        private void OpenFolderBtn_Click(object sender, EventArgs e)
        {
            Process.Start("explorer.exe", Configuration.DefaultDirectoryPath);
        }       
    }
}
