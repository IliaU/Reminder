
namespace ProviderMsSql
{
    partial class FSetupConnectDB
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lbl_DB_MSSQL = new System.Windows.Forms.Label();
            this.cBox_BD_MSSQL = new System.Windows.Forms.ComboBox();
            this.panel_MSSQL_Login = new System.Windows.Forms.Panel();
            this.txtBox_Passvord_MSSQL = new System.Windows.Forms.TextBox();
            this.lbl_Passvord_MSSQL = new System.Windows.Forms.Label();
            this.txtBox_Login_MSSQL = new System.Windows.Forms.TextBox();
            this.lbl_Login_MSSQL = new System.Windows.Forms.Label();
            this.btnSaveODBC = new System.Windows.Forms.Button();
            this.btnTestODBC = new System.Windows.Forms.Button();
            this.txtBox_Server_MSSQL = new System.Windows.Forms.TextBox();
            this.lbl_Server_MSSQL = new System.Windows.Forms.Label();
            this.cBox_Audent = new System.Windows.Forms.CheckBox();
            this.panel_MSSQL_Login.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbl_DB_MSSQL
            // 
            this.lbl_DB_MSSQL.AutoSize = true;
            this.lbl_DB_MSSQL.Location = new System.Drawing.Point(16, 146);
            this.lbl_DB_MSSQL.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl_DB_MSSQL.Name = "lbl_DB_MSSQL";
            this.lbl_DB_MSSQL.Size = new System.Drawing.Size(92, 17);
            this.lbl_DB_MSSQL.TabIndex = 60;
            this.lbl_DB_MSSQL.Text = "База данных";
            // 
            // cBox_BD_MSSQL
            // 
            this.cBox_BD_MSSQL.FormattingEnabled = true;
            this.cBox_BD_MSSQL.Location = new System.Drawing.Point(156, 143);
            this.cBox_BD_MSSQL.Margin = new System.Windows.Forms.Padding(4);
            this.cBox_BD_MSSQL.Name = "cBox_BD_MSSQL";
            this.cBox_BD_MSSQL.Size = new System.Drawing.Size(260, 24);
            this.cBox_BD_MSSQL.TabIndex = 59;
            this.cBox_BD_MSSQL.MouseEnter += new System.EventHandler(this.cBox_BD_MSSQL_MouseEnter);
            // 
            // panel_MSSQL_Login
            // 
            this.panel_MSSQL_Login.Controls.Add(this.txtBox_Passvord_MSSQL);
            this.panel_MSSQL_Login.Controls.Add(this.lbl_Passvord_MSSQL);
            this.panel_MSSQL_Login.Controls.Add(this.txtBox_Login_MSSQL);
            this.panel_MSSQL_Login.Controls.Add(this.lbl_Login_MSSQL);
            this.panel_MSSQL_Login.Location = new System.Drawing.Point(16, 74);
            this.panel_MSSQL_Login.Margin = new System.Windows.Forms.Padding(4);
            this.panel_MSSQL_Login.Name = "panel_MSSQL_Login";
            this.panel_MSSQL_Login.Size = new System.Drawing.Size(453, 68);
            this.panel_MSSQL_Login.TabIndex = 58;
            this.panel_MSSQL_Login.Visible = false;
            // 
            // txtBox_Passvord_MSSQL
            // 
            this.txtBox_Passvord_MSSQL.Location = new System.Drawing.Point(140, 37);
            this.txtBox_Passvord_MSSQL.Margin = new System.Windows.Forms.Padding(4);
            this.txtBox_Passvord_MSSQL.Name = "txtBox_Passvord_MSSQL";
            this.txtBox_Passvord_MSSQL.Size = new System.Drawing.Size(260, 22);
            this.txtBox_Passvord_MSSQL.TabIndex = 10;
            this.txtBox_Passvord_MSSQL.UseSystemPasswordChar = true;
            this.txtBox_Passvord_MSSQL.TextChanged += new System.EventHandler(this.txtBox_Passvord_MSSQL_TextChanged);
            // 
            // lbl_Passvord_MSSQL
            // 
            this.lbl_Passvord_MSSQL.AutoSize = true;
            this.lbl_Passvord_MSSQL.Location = new System.Drawing.Point(0, 41);
            this.lbl_Passvord_MSSQL.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl_Passvord_MSSQL.Name = "lbl_Passvord_MSSQL";
            this.lbl_Passvord_MSSQL.Size = new System.Drawing.Size(57, 17);
            this.lbl_Passvord_MSSQL.TabIndex = 9;
            this.lbl_Passvord_MSSQL.Text = "Пароль";
            // 
            // txtBox_Login_MSSQL
            // 
            this.txtBox_Login_MSSQL.Location = new System.Drawing.Point(140, 5);
            this.txtBox_Login_MSSQL.Margin = new System.Windows.Forms.Padding(4);
            this.txtBox_Login_MSSQL.Name = "txtBox_Login_MSSQL";
            this.txtBox_Login_MSSQL.Size = new System.Drawing.Size(260, 22);
            this.txtBox_Login_MSSQL.TabIndex = 8;
            this.txtBox_Login_MSSQL.TextChanged += new System.EventHandler(this.txtBox_Login_MSSQL_TextChanged);
            // 
            // lbl_Login_MSSQL
            // 
            this.lbl_Login_MSSQL.AutoSize = true;
            this.lbl_Login_MSSQL.Location = new System.Drawing.Point(0, 5);
            this.lbl_Login_MSSQL.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl_Login_MSSQL.Name = "lbl_Login_MSSQL";
            this.lbl_Login_MSSQL.Size = new System.Drawing.Size(47, 17);
            this.lbl_Login_MSSQL.TabIndex = 7;
            this.lbl_Login_MSSQL.Text = "Логин";
            // 
            // btnSaveODBC
            // 
            this.btnSaveODBC.Location = new System.Drawing.Point(293, 198);
            this.btnSaveODBC.Margin = new System.Windows.Forms.Padding(4);
            this.btnSaveODBC.Name = "btnSaveODBC";
            this.btnSaveODBC.Size = new System.Drawing.Size(100, 28);
            this.btnSaveODBC.TabIndex = 54;
            this.btnSaveODBC.Text = "Сохранить";
            this.btnSaveODBC.UseVisualStyleBackColor = true;
            this.btnSaveODBC.Click += new System.EventHandler(this.btnSaveODBC_Click);
            // 
            // btnTestODBC
            // 
            this.btnTestODBC.Location = new System.Drawing.Point(16, 198);
            this.btnTestODBC.Margin = new System.Windows.Forms.Padding(4);
            this.btnTestODBC.Name = "btnTestODBC";
            this.btnTestODBC.Size = new System.Drawing.Size(239, 28);
            this.btnTestODBC.TabIndex = 53;
            this.btnTestODBC.Text = "Протестировать подключение";
            this.btnTestODBC.UseVisualStyleBackColor = true;
            this.btnTestODBC.Click += new System.EventHandler(this.btnTestODBC_Click);
            // 
            // txtBox_Server_MSSQL
            // 
            this.txtBox_Server_MSSQL.Location = new System.Drawing.Point(156, 13);
            this.txtBox_Server_MSSQL.Margin = new System.Windows.Forms.Padding(4);
            this.txtBox_Server_MSSQL.Name = "txtBox_Server_MSSQL";
            this.txtBox_Server_MSSQL.Size = new System.Drawing.Size(260, 22);
            this.txtBox_Server_MSSQL.TabIndex = 57;
            this.txtBox_Server_MSSQL.TextChanged += new System.EventHandler(this.txtBox_Server_MSSQL_TextChanged);
            // 
            // lbl_Server_MSSQL
            // 
            this.lbl_Server_MSSQL.AutoSize = true;
            this.lbl_Server_MSSQL.Location = new System.Drawing.Point(16, 17);
            this.lbl_Server_MSSQL.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl_Server_MSSQL.Name = "lbl_Server_MSSQL";
            this.lbl_Server_MSSQL.Size = new System.Drawing.Size(56, 17);
            this.lbl_Server_MSSQL.TabIndex = 56;
            this.lbl_Server_MSSQL.Text = "Сервер";
            // 
            // cBox_Audent
            // 
            this.cBox_Audent.AutoSize = true;
            this.cBox_Audent.Location = new System.Drawing.Point(20, 45);
            this.cBox_Audent.Margin = new System.Windows.Forms.Padding(4);
            this.cBox_Audent.Name = "cBox_Audent";
            this.cBox_Audent.Size = new System.Drawing.Size(214, 21);
            this.cBox_Audent.TabIndex = 55;
            this.cBox_Audent.Text = "Доменная аудентификация";
            this.cBox_Audent.UseVisualStyleBackColor = true;
            this.cBox_Audent.CheckedChanged += new System.EventHandler(this.cBox_Audent_CheckedChanged);
            // 
            // FSetupConnectDB
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 238);
            this.Controls.Add(this.lbl_DB_MSSQL);
            this.Controls.Add(this.cBox_BD_MSSQL);
            this.Controls.Add(this.panel_MSSQL_Login);
            this.Controls.Add(this.btnSaveODBC);
            this.Controls.Add(this.btnTestODBC);
            this.Controls.Add(this.txtBox_Server_MSSQL);
            this.Controls.Add(this.lbl_Server_MSSQL);
            this.Controls.Add(this.cBox_Audent);
            this.Name = "FSetupConnectDB";
            this.Text = "Настройка нативного клиента Ms Sql Server";
            this.Load += new System.EventHandler(this.FSetupConnectDB_Load);
            this.panel_MSSQL_Login.ResumeLayout(false);
            this.panel_MSSQL_Login.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbl_DB_MSSQL;
        private System.Windows.Forms.ComboBox cBox_BD_MSSQL;
        private System.Windows.Forms.Panel panel_MSSQL_Login;
        private System.Windows.Forms.TextBox txtBox_Passvord_MSSQL;
        private System.Windows.Forms.Label lbl_Passvord_MSSQL;
        private System.Windows.Forms.TextBox txtBox_Login_MSSQL;
        private System.Windows.Forms.Label lbl_Login_MSSQL;
        private System.Windows.Forms.Button btnSaveODBC;
        private System.Windows.Forms.Button btnTestODBC;
        private System.Windows.Forms.TextBox txtBox_Server_MSSQL;
        private System.Windows.Forms.Label lbl_Server_MSSQL;
        private System.Windows.Forms.CheckBox cBox_Audent;
    }
}