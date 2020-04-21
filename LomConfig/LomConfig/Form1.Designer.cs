namespace LomConfig
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.UrlRestTbx = new System.Windows.Forms.TextBox();
            this.UrlRestLbl = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.CkydConnTxb = new System.Windows.Forms.TextBox();
            this.PinGenTimeTableLbx = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.DBUpdateTimeTableLbx = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.AddPinCodeGenTimeBtn = new System.Windows.Forms.Button();
            this.DeletePinCodeGenTimeBtn = new System.Windows.Forms.Button();
            this.AddDBUpdateTimeBtn = new System.Windows.Forms.Button();
            this.DeleteDBUpdateTimeBtn = new System.Windows.Forms.Button();
            this.PinGenMTbx = new System.Windows.Forms.MaskedTextBox();
            this.DBUpdaterMTbx = new System.Windows.Forms.MaskedTextBox();
            this.ParentOrgCbx = new System.Windows.Forms.ComboBox();
            this.SaveConfBtn = new System.Windows.Forms.Button();
            this.CheckConnectionSKYDTbx = new System.Windows.Forms.Button();
            this.OpenFolderBtn = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.SaveSettingStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.CheckConnectionRestBtn = new System.Windows.Forms.Button();
            this.FileRotationUpD = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FileRotationUpD)).BeginInit();
            this.SuspendLayout();
            // 
            // UrlRestTbx
            // 
            this.UrlRestTbx.Location = new System.Drawing.Point(12, 32);
            this.UrlRestTbx.Name = "UrlRestTbx";
            this.UrlRestTbx.Size = new System.Drawing.Size(794, 26);
            this.UrlRestTbx.TabIndex = 0;
            // 
            // UrlRestLbl
            // 
            this.UrlRestLbl.AutoSize = true;
            this.UrlRestLbl.Location = new System.Drawing.Point(12, 9);
            this.UrlRestLbl.Name = "UrlRestLbl";
            this.UrlRestLbl.Size = new System.Drawing.Size(67, 20);
            this.UrlRestLbl.TabIndex = 1;
            this.UrlRestLbl.Text = "Url Rest";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 75);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(231, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "Строка подключения к СКУД";
            // 
            // CkydConnTxb
            // 
            this.CkydConnTxb.Location = new System.Drawing.Point(12, 98);
            this.CkydConnTxb.Multiline = true;
            this.CkydConnTxb.Name = "CkydConnTxb";
            this.CkydConnTxb.Size = new System.Drawing.Size(794, 74);
            this.CkydConnTxb.TabIndex = 3;
            // 
            // PinGenTimeTableLbx
            // 
            this.PinGenTimeTableLbx.FormattingEnabled = true;
            this.PinGenTimeTableLbx.ItemHeight = 20;
            this.PinGenTimeTableLbx.Location = new System.Drawing.Point(16, 291);
            this.PinGenTimeTableLbx.Name = "PinGenTimeTableLbx";
            this.PinGenTimeTableLbx.Size = new System.Drawing.Size(378, 204);
            this.PinGenTimeTableLbx.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 268);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(258, 20);
            this.label2.TabIndex = 5;
            this.label2.Text = "Расписание генерации пинкодов";
            // 
            // DBUpdateTimeTableLbx
            // 
            this.DBUpdateTimeTableLbx.FormattingEnabled = true;
            this.DBUpdateTimeTableLbx.ItemHeight = 20;
            this.DBUpdateTimeTableLbx.Location = new System.Drawing.Point(413, 291);
            this.DBUpdateTimeTableLbx.Name = "DBUpdateTimeTableLbx";
            this.DBUpdateTimeTableLbx.Size = new System.Drawing.Size(393, 204);
            this.DBUpdateTimeTableLbx.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(409, 268);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(294, 20);
            this.label3.TabIndex = 7;
            this.label3.Text = "Расписание обновления базы данных";
            // 
            // AddPinCodeGenTimeBtn
            // 
            this.AddPinCodeGenTimeBtn.Location = new System.Drawing.Point(185, 501);
            this.AddPinCodeGenTimeBtn.Name = "AddPinCodeGenTimeBtn";
            this.AddPinCodeGenTimeBtn.Size = new System.Drawing.Size(108, 41);
            this.AddPinCodeGenTimeBtn.TabIndex = 8;
            this.AddPinCodeGenTimeBtn.Text = "Добавить";
            this.AddPinCodeGenTimeBtn.UseVisualStyleBackColor = true;
            this.AddPinCodeGenTimeBtn.Click += new System.EventHandler(this.AddPinCodeGenTimeBtn_Click);
            // 
            // DeletePinCodeGenTimeBtn
            // 
            this.DeletePinCodeGenTimeBtn.Location = new System.Drawing.Point(299, 501);
            this.DeletePinCodeGenTimeBtn.Name = "DeletePinCodeGenTimeBtn";
            this.DeletePinCodeGenTimeBtn.Size = new System.Drawing.Size(95, 41);
            this.DeletePinCodeGenTimeBtn.TabIndex = 9;
            this.DeletePinCodeGenTimeBtn.Text = "Удалить";
            this.DeletePinCodeGenTimeBtn.UseVisualStyleBackColor = true;
            this.DeletePinCodeGenTimeBtn.Click += new System.EventHandler(this.DeletePinCodeGenTimeBtn_Click);
            // 
            // AddDBUpdateTimeBtn
            // 
            this.AddDBUpdateTimeBtn.Location = new System.Drawing.Point(577, 501);
            this.AddDBUpdateTimeBtn.Name = "AddDBUpdateTimeBtn";
            this.AddDBUpdateTimeBtn.Size = new System.Drawing.Size(111, 41);
            this.AddDBUpdateTimeBtn.TabIndex = 10;
            this.AddDBUpdateTimeBtn.Text = "Добавить";
            this.AddDBUpdateTimeBtn.UseVisualStyleBackColor = true;
            this.AddDBUpdateTimeBtn.Click += new System.EventHandler(this.AddDBUpdateTimeBtn_Click);
            // 
            // DeleteDBUpdateTimeBtn
            // 
            this.DeleteDBUpdateTimeBtn.Location = new System.Drawing.Point(694, 501);
            this.DeleteDBUpdateTimeBtn.Name = "DeleteDBUpdateTimeBtn";
            this.DeleteDBUpdateTimeBtn.Size = new System.Drawing.Size(112, 41);
            this.DeleteDBUpdateTimeBtn.TabIndex = 11;
            this.DeleteDBUpdateTimeBtn.Text = "Удалить";
            this.DeleteDBUpdateTimeBtn.UseVisualStyleBackColor = true;
            this.DeleteDBUpdateTimeBtn.Click += new System.EventHandler(this.DeleteDBUpdateTimeBtn_Click);
            // 
            // PinGenMTbx
            // 
            this.PinGenMTbx.Location = new System.Drawing.Point(12, 508);
            this.PinGenMTbx.Mask = "00:00";
            this.PinGenMTbx.Name = "PinGenMTbx";
            this.PinGenMTbx.Size = new System.Drawing.Size(167, 26);
            this.PinGenMTbx.TabIndex = 12;
            this.PinGenMTbx.ValidatingType = typeof(System.DateTime);
            // 
            // DBUpdaterMTbx
            // 
            this.DBUpdaterMTbx.Location = new System.Drawing.Point(407, 508);
            this.DBUpdaterMTbx.Mask = "00:00";
            this.DBUpdaterMTbx.Name = "DBUpdaterMTbx";
            this.DBUpdaterMTbx.Size = new System.Drawing.Size(164, 26);
            this.DBUpdaterMTbx.TabIndex = 13;
            this.DBUpdaterMTbx.ValidatingType = typeof(System.DateTime);
            // 
            // ParentOrgCbx
            // 
            this.ParentOrgCbx.FormattingEnabled = true;
            this.ParentOrgCbx.Location = new System.Drawing.Point(12, 569);
            this.ParentOrgCbx.Name = "ParentOrgCbx";
            this.ParentOrgCbx.Size = new System.Drawing.Size(794, 28);
            this.ParentOrgCbx.TabIndex = 14;
            this.ParentOrgCbx.Text = "Организация";
            // 
            // SaveConfBtn
            // 
            this.SaveConfBtn.Location = new System.Drawing.Point(12, 614);
            this.SaveConfBtn.Name = "SaveConfBtn";
            this.SaveConfBtn.Size = new System.Drawing.Size(231, 45);
            this.SaveConfBtn.TabIndex = 15;
            this.SaveConfBtn.Text = "Сохранить конфигурацию";
            this.SaveConfBtn.UseVisualStyleBackColor = true;
            this.SaveConfBtn.Click += new System.EventHandler(this.SaveConfBtn_Click);
            // 
            // CheckConnectionSKYDTbx
            // 
            this.CheckConnectionSKYDTbx.Location = new System.Drawing.Point(822, 114);
            this.CheckConnectionSKYDTbx.Name = "CheckConnectionSKYDTbx";
            this.CheckConnectionSKYDTbx.Size = new System.Drawing.Size(244, 58);
            this.CheckConnectionSKYDTbx.TabIndex = 16;
            this.CheckConnectionSKYDTbx.Text = "Проверить соединение со Скудом";
            this.CheckConnectionSKYDTbx.UseVisualStyleBackColor = true;
            this.CheckConnectionSKYDTbx.Click += new System.EventHandler(this.CheckConnectionTbx_Click);
            // 
            // OpenFolderBtn
            // 
            this.OpenFolderBtn.Location = new System.Drawing.Point(575, 615);
            this.OpenFolderBtn.Name = "OpenFolderBtn";
            this.OpenFolderBtn.Size = new System.Drawing.Size(231, 45);
            this.OpenFolderBtn.TabIndex = 17;
            this.OpenFolderBtn.Text = "Открыть настройки";
            this.OpenFolderBtn.UseVisualStyleBackColor = true;
            this.OpenFolderBtn.Click += new System.EventHandler(this.OpenFolderBtn_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 546);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(340, 20);
            this.label4.TabIndex = 18;
            this.label4.Text = "Организация для добавления сотрудников";
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2,
            this.SaveSettingStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 673);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1078, 32);
            this.statusStrip1.TabIndex = 19;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(127, 25);
            this.toolStripStatusLabel1.Text = "www.artonit.ru";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(191, 25);
            this.toolStripStatusLabel2.Text = "(Версия сборки и год)";
            // 
            // SaveSettingStatus
            // 
            this.SaveSettingStatus.Name = "SaveSettingStatus";
            this.SaveSettingStatus.Size = new System.Drawing.Size(205, 25);
            this.SaveSettingStatus.Text = "(состояние сохранения)";
            // 
            // CheckConnectionRestBtn
            // 
            this.CheckConnectionRestBtn.Location = new System.Drawing.Point(822, 23);
            this.CheckConnectionRestBtn.Name = "CheckConnectionRestBtn";
            this.CheckConnectionRestBtn.Size = new System.Drawing.Size(244, 53);
            this.CheckConnectionRestBtn.TabIndex = 20;
            this.CheckConnectionRestBtn.Text = "Проверить соединение c Rest";
            this.CheckConnectionRestBtn.UseVisualStyleBackColor = true;
            this.CheckConnectionRestBtn.Click += new System.EventHandler(this.CheckConnectionRestBtn_Click);
            // 
            // FileRotationUpD
            // 
            this.FileRotationUpD.Location = new System.Drawing.Point(12, 219);
            this.FileRotationUpD.Maximum = new decimal(new int[] {
            -1530494976,
            232830,
            0,
            0});
            this.FileRotationUpD.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.FileRotationUpD.Name = "FileRotationUpD";
            this.FileRotationUpD.Size = new System.Drawing.Size(378, 26);
            this.FileRotationUpD.TabIndex = 21;
            this.FileRotationUpD.Value = new decimal(new int[] {
            14,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 196);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(200, 20);
            this.label5.TabIndex = 22;
            this.label5.Text = "Период ротации файлов";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1078, 705);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.FileRotationUpD);
            this.Controls.Add(this.CheckConnectionRestBtn);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.OpenFolderBtn);
            this.Controls.Add(this.CheckConnectionSKYDTbx);
            this.Controls.Add(this.SaveConfBtn);
            this.Controls.Add(this.ParentOrgCbx);
            this.Controls.Add(this.DBUpdaterMTbx);
            this.Controls.Add(this.PinGenMTbx);
            this.Controls.Add(this.DeleteDBUpdateTimeBtn);
            this.Controls.Add(this.AddDBUpdateTimeBtn);
            this.Controls.Add(this.DeletePinCodeGenTimeBtn);
            this.Controls.Add(this.AddPinCodeGenTimeBtn);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.DBUpdateTimeTableLbx);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.PinGenTimeTableLbx);
            this.Controls.Add(this.CkydConnTxb);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.UrlRestLbl);
            this.Controls.Add(this.UrlRestTbx);
            this.MaximumSize = new System.Drawing.Size(2000, 2000);
            this.MinimumSize = new System.Drawing.Size(1100, 670);
            this.Name = "Form1";
            this.Text = "LomConfig";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FileRotationUpD)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox UrlRestTbx;
        private System.Windows.Forms.Label UrlRestLbl;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox CkydConnTxb;
        private System.Windows.Forms.ListBox PinGenTimeTableLbx;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox DBUpdateTimeTableLbx;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button AddPinCodeGenTimeBtn;
        private System.Windows.Forms.Button DeletePinCodeGenTimeBtn;
        private System.Windows.Forms.Button AddDBUpdateTimeBtn;
        private System.Windows.Forms.Button DeleteDBUpdateTimeBtn;
        private System.Windows.Forms.MaskedTextBox PinGenMTbx;
        private System.Windows.Forms.MaskedTextBox DBUpdaterMTbx;
        private System.Windows.Forms.ComboBox ParentOrgCbx;
        private System.Windows.Forms.Button SaveConfBtn;
        private System.Windows.Forms.Button CheckConnectionSKYDTbx;
        private System.Windows.Forms.Button OpenFolderBtn;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel SaveSettingStatus;
        private System.Windows.Forms.Button CheckConnectionRestBtn;
        private System.Windows.Forms.NumericUpDown FileRotationUpD;
        private System.Windows.Forms.Label label5;
    }
}

