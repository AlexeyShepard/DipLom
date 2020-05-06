using System.IO;
using System.Text;
using System.Reflection;
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
    public partial class MainForm : Form
    {
        private static string RestMethodToGetPeopleId = "peoples";

        public static bool ProgramStarted = false;

        public MainForm()
        {
            InitializeComponent();

            VersionYearStatus.Text = Assembly.GetExecutingAssembly().GetName().Version.ToString() + " 2020";

            IziLog.Configuration.PathToLogFile = Configuration.DefaultDirectoryPathToLog;
            IziLog.Configuration.FileRotation = Convert.ToInt32(Configuration.FileRotation);

            Configuration.SetLogFileNewName();

            LoggerServer.Start();

            if (IsFirstLaunch()) FirstStartConfiguration();
            else GetConfigurationFromFile();
        }

        #region Methods

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
                    "RESTToken = " + Configuration.RESTToken + "\n" +
                    "FileRotation = " + Configuration.FileRotation + "\n" +
                    "ScudConnectionString = " + Configuration.ScudConnectionString + "\n" +
                    "AdminPanelUrl = " + Configuration.AdminPanelUrl + "\n" +
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
            try
            {
                Logger.Log(new InfoRecord("Считывание конфигурации, приложением LomConfig..."));

                FileIniDataParser IniParser = new FileIniDataParser();
                IniData IniData = IniParser.ReadFile(Configuration.DefaultDirectoryPathToIni);

                Configuration.RESTUrl = IniData["Main"]["UrlREST"];
                Configuration.RESTToken = IniData["Main"]["RESTToken"];
                Configuration.FileRotation = IniData["Main"]["FileRotation"];
                Configuration.ScudConnectionString = IniData["Main"]["ScudConnectionString"];
                Configuration.AdminPanelUrl = IniData["Main"]["AdminPanelUrl"];
                Configuration.TimeGenerationPinCode = FromStringToArray(IniData["Main"]["TimeGenerationPincode"]);
                Configuration.TimeUpdateDatabase = FromStringToArray(IniData["Main"]["TimeUpdateDatabase"]);
                Configuration.ParentOrg = IniData["Main"]["ParentOrg"];


                Logger.Log(new InfoRecord("Считывание конфигурации, приложением LomConfig завершенно, прошло успешно"));

                InsertConfigurationToTheField();
            }
            catch(Exception ex)
            {
                MessageBox.Show("В результате чтения конфигурационного файла, произошла ошибка. Удалите файл конфигурации и повторите попытку!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }           
        }

        private void InsertConfigurationToTheField()
        {
            UrlRestTbx.Text = Configuration.RESTUrl;
            CkydConnTxb.Text = Configuration.ScudConnectionString;
            RESTAPITokenTbx.Text = Configuration.RESTToken;

            FileRotationUpD.Value = Convert.ToInt32(Configuration.FileRotation);

            UrlAdminPanelTbx.Text = Configuration.AdminPanelUrl;

            foreach (string time in Configuration.TimeGenerationPinCode) PinGenTimeTableLbx.Items.Add(time);

            foreach (string time in Configuration.TimeUpdateDatabase) DBUpdateTimeTableLbx.Items.Add(time);

            FillOrganizationTree();
            SelectCurrentOrganizationNode(OrganizationTree.Nodes);

            ProgramStarted = true;
        }

        ///Вдруг попросят вернуть выпадающий список
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

        private void FillOrganizationTree()
        {
            OdbcCommand Command = new OdbcCommand("SELECT * FROM ORGANIZATION WHERE ID_PARENT = 1");
            OdbcDataReader reader;

            using (OdbcConnection Connection = new OdbcConnection(Configuration.ScudConnectionString))
            {

                Command.Connection = Connection;
                Connection.Open();
                reader = Command.ExecuteReader();

                TreeNode OrganizationNode;


                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        OrganizationNode = new TreeNode(reader.GetValue(0) + " - " + reader.GetValue(2));
                        OrganizationTree.Nodes.Add(OrganizationNode);
                        if(Convert.ToInt32(reader.GetValue(0)) != 1) AddChildrenOrganizationNode(OrganizationNode, Convert.ToInt32(reader.GetValue(0)));
                    }
                }  
            }
        }

        private void AddChildrenOrganizationNode(TreeNode ParentNode, int ParentId)
        {
            OdbcCommand Command = new OdbcCommand("SELECT * FROM ORGANIZATION WHERE ID_PARENT = " + ParentId);
            OdbcDataReader reader;

            using (OdbcConnection Connection = new OdbcConnection(Configuration.ScudConnectionString))
            {

                Command.Connection = Connection;
                Connection.Open();
                reader = Command.ExecuteReader();

                TreeNode OrganizationNode;

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        OrganizationNode = new TreeNode(reader.GetValue(0) + " - " + reader.GetValue(2));
                        ParentNode.Nodes.Add(OrganizationNode);
                        AddChildrenOrganizationNode(OrganizationNode, Convert.ToInt32(reader.GetValue(0)));
                    }
                }
            }
        }

        private void SelectCurrentOrganizationNode(TreeNodeCollection Nodes)
        {
            foreach(TreeNode OrgNode in Nodes)
            {
                int Index = OrgNode.Text.IndexOf(" ");
                if (OrgNode.Text.Substring(0, Index) == Configuration.ParentOrg)
                {
                    OrganizationTree.SelectedNode = OrgNode;
                    OrganizationTree.SelectedNode.BackColor = System.Drawing.Color.LightBlue;
                    break;
                }
                else SelectCurrentOrganizationNode(OrgNode.Nodes);
            }
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

        private bool IsChangesExist()
        {
            bool ChangeExisting = false;
            
            if (UrlRestTbx.Text != Configuration.RESTUrl) ChangeExisting = true;
            if (CkydConnTxb.Text != Configuration.ScudConnectionString) ChangeExisting = true;
            if (RESTAPITokenTbx.Text != Configuration.RESTToken) ChangeExisting = true;
            if (UrlAdminPanelTbx.Text != Configuration.AdminPanelUrl) ChangeExisting = true;
            if (FileRotationUpD.Value.ToString() != Configuration.FileRotation) ChangeExisting = true;

            
            if(OrganizationTree.SelectedNode != null)
            {
                string NodeOrgStr = OrganizationTree.SelectedNode.Text;
                int Index = NodeOrgStr.IndexOf(" ");
                if (NodeOrgStr.Substring(0, Index) != Configuration.ParentOrg) ChangeExisting = true;
            }           

            string[] cash = new string[PinGenTimeTableLbx.Items.Count];
            PinGenTimeTableLbx.Items.CopyTo(cash, 0);
            string total = FromArrayToString(cash);
            if(total != FromArrayToString(Configuration.TimeGenerationPinCode)) ChangeExisting = true;

            cash = new string[DBUpdateTimeTableLbx.Items.Count];
            DBUpdateTimeTableLbx.Items.CopyTo(cash, 0);
            total = FromArrayToString(cash);
            if (total != FromArrayToString(Configuration.TimeUpdateDatabase)) ChangeExisting = true;

            return ChangeExisting;          
        }

        public void CheckChanges()
        {
            if (ProgramStarted && IsChangesExist()) SaveSettingStatus.Text = "Есть несохраненные изменения";
            else SaveSettingStatus.Text = "Изменения сохранены";

            SaveConfBtn.Enabled = IsChangesExist();
        }

        public void SaveProcess()
        {
            try
            {
                FileIniDataParser IniParser = new FileIniDataParser();
                IniData IniData = new IniData();

                Configuration.RESTUrl = UrlRestTbx.Text;
                Configuration.ScudConnectionString = CkydConnTxb.Text;
                Configuration.RESTToken = RESTAPITokenTbx.Text;

                Configuration.FileRotation = FileRotationUpD.Value.ToString();
                Configuration.AdminPanelUrl = UrlAdminPanelTbx.Text;

                string[] cash = new string[PinGenTimeTableLbx.Items.Count];
                PinGenTimeTableLbx.Items.CopyTo(cash, 0);
                Configuration.TimeGenerationPinCode = cash;


                cash = new string[DBUpdateTimeTableLbx.Items.Count];
                DBUpdateTimeTableLbx.Items.CopyTo(cash, 0);
                Configuration.TimeUpdateDatabase = cash;

                string NodeOrgStr = OrganizationTree.SelectedNode.Text;
                int Index = NodeOrgStr.IndexOf(" ");
                Configuration.ParentOrg = NodeOrgStr.Substring(0, Index);

                IniData["Main"]["UrlREST"] = Configuration.RESTUrl;
                IniData["Main"]["RESTToken"] = Configuration.RESTToken;
                IniData["Main"]["FileRotation"] = Configuration.FileRotation;
                IniData["Main"]["ScudConnectionString"] = Configuration.ScudConnectionString;
                IniData["Main"]["AdminPanelUrl"] = Configuration.AdminPanelUrl;
                IniData["Main"]["TimeGenerationPincode"] = FromArrayToString(Configuration.TimeGenerationPinCode);
                IniData["Main"]["TimeUpdateDatabase"] = FromArrayToString(Configuration.TimeUpdateDatabase);
                IniData["Main"]["ParentOrg"] = Configuration.ParentOrg;

                IniParser.WriteFile(Configuration.DefaultDirectoryPathToIni, IniData);

                Logger.Log(new InfoRecord("Ручное изменение конфигурации, прошло успешно"));
                CheckChanges();
                MessageBox.Show("Сохранено успешно!");
            }
            catch (Exception ex)
            {
                Logger.Log(new ErrorRecord(ex.Message));
                MessageBox.Show(ex.Message);
            }
            finally
            {
                Logger.Log(new WarningRecord("Были использованы следующие настройки:\nUrlRest = " + Configuration.RESTUrl + "\n" +
                "RESTTokem = " + Configuration.RESTToken + "\n" + 
                "FileRotation = " + Configuration.FileRotation + "\n" +
                "ScudConnectionString = " + Configuration.ScudConnectionString + "\n" +
                "AdminPanelUrl = " + Configuration.AdminPanelUrl + "\n" +
                "TimeGenerationPincode = " + FromArrayToString(Configuration.TimeGenerationPinCode) + "\n" +
                "TimeUpdateDatabase = " + FromArrayToString(Configuration.TimeUpdateDatabase) + "\n" +
                "ParentOrg = " + Configuration.ParentOrg));
            }
        }

        #endregion

        #region Events

        private void AddPinCodeGenTimeBtn_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime Time = Convert.ToDateTime(PinGenMTbx.Text);
                PinGenTimeTableLbx.Items.Add(Time.ToString("H:mm"));

                CheckChanges();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Неверный формат данных!");
            }
        }

        private void DeletePinCodeGenTimeBtn_Click(object sender, EventArgs e)
        {
            PinGenTimeTableLbx.Items.Remove(PinGenTimeTableLbx.SelectedItem);

            CheckChanges();
        }

        private void AddDBUpdateTimeBtn_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime Time = Convert.ToDateTime(DBUpdaterMTbx.Text);
                DBUpdateTimeTableLbx.Items.Add(Time.ToString("H:mm"));

                CheckChanges();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Неверный формат данных!");
            }           
        }

        private void DeleteDBUpdateTimeBtn_Click(object sender, EventArgs e)
        {
            DBUpdateTimeTableLbx.Items.Remove(DBUpdateTimeTableLbx.SelectedItem);
            CheckChanges();
        }     

        private void SaveConfBtn_Click(object sender, EventArgs e)
        {
            SaveProcess();
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

        private void OpenFolderBtn_Click(object sender, EventArgs e)
        {
            Process.Start("explorer.exe", Configuration.DefaultDirectoryPath);
        }

        private void OpenAdminPanelBtn_Click(object sender, EventArgs e)
        {
            Process.Start(Configuration.AdminPanelUrl);
        }

        private void HelpMenuBtn_Click(object sender, EventArgs e)
        {
            MessageBox.Show("/help - вызов справки\n/silent - запуск программы в штатном режиме\n/pin - генерация пин-кодв\n/load - Выгрузка базы данных\n/stop - остановка LOM", "Помощь", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void AboutMenuBtn_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Разработчик: Бушин Алексей Юрьевич\nООО \"Арстек\" 2020\n", "О программе", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void UrlRestTbx_TextChanged(object sender, EventArgs e)
        {
            CheckChanges();   
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (IsChangesExist())
            {
                DialogResult Result = MessageBox.Show("Имеются несохраненные данные, сохранить перед выходом?", "Есть несохраненные данные", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (Result.Equals(DialogResult.Yes)) SaveProcess();
            }
        }

        private void OrganizationTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            CheckChanges();
        }

        #endregion
    }
}
