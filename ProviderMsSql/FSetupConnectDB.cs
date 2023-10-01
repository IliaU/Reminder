using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Common;
using System.Data.SqlClient;

namespace ProviderMsSql
{
    public partial class FSetupConnectDB : Form
    {
        private ProviderMsSql Prv;
        private Boolean isEdit = false;

        private string OldConnectionString;
        public string NewConnectionString;

        /// <summary>
        /// Конструктор по настройке нативного подключения к базе данных
        /// </summary>
        /// <param name="Prv">Провайдер который мы настраиваем</param>
        public FSetupConnectDB(ProviderMsSql Prv)
        {
            try
            {
                this.Prv = Prv;
                this.NewConnectionString = Prv.ConnectionString;
                InitializeComponent();

                this.Icon = CustomizationFarm.CurCustomization.GetIconStatus(EventEn.Message);

                // Если мы редактируем элемент, то подгружаем имя провайдера
                if (Prv.ConnectionString != null && Prv.ConnectionString != string.Empty)
                {
                    this.Prv.Scsb = new SqlConnectionStringBuilder(Prv.ConnectionString);
                    this.txtBox_Server_MSSQL.Text = this.Prv.Scsb.DataSource;
                    this.cBox_Audent.Checked = this.Prv.Scsb.IntegratedSecurity;
                    if (this.cBox_Audent.Checked)
                    {
                        this.txtBox_Login_MSSQL.Text = this.Prv.Scsb.UserID;
                        this.txtBox_Passvord_MSSQL.Text = this.Prv.Scsb.Password;
                    }
                    this.OldConnectionString = this.Prv.ConnectionString;
                    this.isEdit = true;
                    BD_MSSQL_Clear_MSSQL();
                }
                this.isEdit = false;
            }
            catch (Exception ex)
            {
                ApplicationException ae = new ApplicationException(string.Format(@"Ошибка в конструкторе:""{0}""", ex.Message));
                Log.EventSave(ae.Message, string.Format("{0}", this.GetType().FullName), EventEn.Error);
                throw ae;
            }
        }

        // Чтение формы
        private void FSetupConnectDB_Load(object sender, EventArgs e)
        {
            try
            {
                if (this.cBox_Audent.Checked) this.panel_MSSQL_Login.Visible = false;
                else this.panel_MSSQL_Login.Visible = true;
            }
            catch (Exception ex)
            {
                ApplicationException ae = new ApplicationException(string.Format(@"Ошибка при чтении формы:""{0}""", ex.Message));
                Log.EventSave(ae.Message, string.Format("{0}.FSetupConnectDB_Load", this.GetType().FullName), EventEn.Error);
                throw ae;
            }
        }

        // Пользователь решил протестировать соединение
        private void btnTestODBC_Click(object sender, EventArgs e)
        {
            try
            {
                this.NewConnectionString = this.Prv.InstalProvider(this.txtBox_Server_MSSQL.Text, this.cBox_Audent.Checked, (this.cBox_Audent.Checked && !string.IsNullOrWhiteSpace(this.txtBox_Login_MSSQL.Text) ? null : this.txtBox_Login_MSSQL.Text), (this.cBox_Audent.Checked && !string.IsNullOrWhiteSpace(this.txtBox_Passvord_MSSQL.Text) ? null : this.txtBox_Passvord_MSSQL.Text), this.cBox_BD_MSSQL.SelectedText, true, false, false);
                if (!string.IsNullOrWhiteSpace(this.NewConnectionString)) MessageBox.Show("Тестирование подключения завершилось успешно");
            }
            catch (Exception ex)
            {
                ApplicationException ae = new ApplicationException(string.Format(@"Ошибка при тестировании подключения:""{0}""", ex.Message));
                Log.EventSave(ae.Message, string.Format("{0}.btnTestODBC_Click", this.GetType().FullName), EventEn.Error);
                throw ae;
            }
        }

        // Пользователь сохраняет подключение
        private void btnSaveODBC_Click(object sender, EventArgs e)
        {
            try
            {
                this.NewConnectionString = this.Prv.InstalProvider(this.txtBox_Server_MSSQL.Text, this.cBox_Audent.Checked, (this.cBox_Audent.Checked && !string.IsNullOrWhiteSpace(this.txtBox_Login_MSSQL.Text) ? null : this.txtBox_Login_MSSQL.Text), (this.cBox_Audent.Checked && !string.IsNullOrWhiteSpace(this.txtBox_Passvord_MSSQL.Text) ? null : this.txtBox_Passvord_MSSQL.Text), this.cBox_BD_MSSQL.Text, true, false, false);
                if (!string.IsNullOrWhiteSpace(this.NewConnectionString)) this.DialogResult = DialogResult.Yes;
                else this.DialogResult = DialogResult.No;

                this.Close();
            }
            catch (Exception ex)
            {
                ApplicationException ae = new ApplicationException(string.Format(@"Ошибка при сохранении строки подключения:""{0}""", ex.Message));
                Log.EventSave(ae.Message, string.Format("{0}.btnSaveODBC_Click", this.GetType().FullName), EventEn.Error);
                throw ae;
            }
        }

        // Появление мышки над выбором базы данных
        private bool MouseEnterFlag = false; // Флаг, по которому срабатывает перестроение списка баз данных
        private void cBox_BD_MSSQL_MouseEnter(object sender, EventArgs e)
        {
            try
            {
                // проверяем флаг, возможно перечитывать список баз не нужно
                if (this.MouseEnterFlag)
                {
                    string ConnectionStringTmp = null;
                    try
                    {
                        ConnectionStringTmp = this.Prv.InstalProvider(this.txtBox_Server_MSSQL.Text, this.cBox_Audent.Checked, (this.cBox_Audent.Checked && !string.IsNullOrWhiteSpace(this.txtBox_Login_MSSQL.Text) ? null : this.txtBox_Login_MSSQL.Text), (this.cBox_Audent.Checked && !string.IsNullOrWhiteSpace(this.txtBox_Passvord_MSSQL.Text) ? null : this.txtBox_Passvord_MSSQL.Text), this.cBox_BD_MSSQL.SelectedText, false, false, false);
                    }
                    catch (Exception) { }

                    // проверяем подключение и получаем список доступных баз
                    if (!string.IsNullOrWhiteSpace(ConnectionStringTmp))
                    {
                        foreach (string item in this.Prv.GetBdList(ConnectionStringTmp))
                        {
                            this.cBox_BD_MSSQL.Items.Add(item);
                        }
                    }

                }
                this.MouseEnterFlag = false;
            }
            catch (Exception ex)
            {
                ApplicationException ae = new ApplicationException(string.Format(@"Ошибка в методе при получении списка баз данных:""{0}""", ex.Message));
                Log.EventSave(ae.Message, string.Format("{0}.cBox_BD_MSSQL_MouseEnter", this.GetType().FullName), EventEn.Error);
                throw ae;
            }
        }
        // Чистка списка доступных баз данных
        private void BD_MSSQL_Clear_MSSQL()
        {
            try
            {
                this.cBox_BD_MSSQL.Items.Clear();

                MouseEnterFlag = true;

                // Делаем если только мы не на редактировании
                if (!string.IsNullOrWhiteSpace(this.OldConnectionString))
                {
                    if (this.isEdit)
                    {
                        this.txtBox_Login_MSSQL.Text = this.Prv.Scsb.UserID;
                        this.txtBox_Passvord_MSSQL.Text = this.Prv.Scsb.Password;
                    }

                    // Подгружаем список доступных баз
                    try
                    {
                        cBox_BD_MSSQL_MouseEnter(null, null);
                        if (this.cBox_BD_MSSQL.Items.Count == 0) this.cBox_BD_MSSQL.Text = "";
                        else this.cBox_BD_MSSQL.Text = this.Prv.Scsb.InitialCatalog;
                    }
                    catch (Exception) { }
                }
            }
            catch (Exception ex)
            {
                ApplicationException ae = new ApplicationException(string.Format(@"Ошибка при чтении списка баз данных:""{0}""", ex.Message));
                Log.EventSave(ae.Message, string.Format("{0}.BD_MSSQL_Clear_MSSQL", this.GetType().FullName), EventEn.Error);
                throw ae;
            }
        }

        // Пользователь изменил тип аудентификации при подключении к MS SQL
        private void cBox_Audent_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                FSetupConnectDB_Load(null, null);
                BD_MSSQL_Clear_MSSQL();
            }
            catch (Exception ex)
            {
                ApplicationException ae = new ApplicationException(string.Format(@"Ошибка при изменении типа аутентификации:""{0}""", ex.Message));
                Log.EventSave(ae.Message, string.Format("{0}.cBox_Audent_CheckedChanged", this.GetType().FullName), EventEn.Error);
                throw ae;
            }
        }

        // Пользователь изменил имя сервера
        private void txtBox_Server_MSSQL_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(this.NewConnectionString))
                {
                    this.NewConnectionString = null;
                    BD_MSSQL_Clear_MSSQL();
                }
            }
            catch (Exception ex)
            {
                ApplicationException ae = new ApplicationException(string.Format(@"Ошибка при изменении имени сервера:""{0}""", ex.Message));
                Log.EventSave(ae.Message, string.Format("{0}.txtBox_Server_MSSQL_TextChanged", this.GetType().FullName), EventEn.Error);
                throw ae;
            }
        }

        // Пользователь изменил Login
        private void txtBox_Login_MSSQL_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(this.NewConnectionString))
                {
                    this.NewConnectionString = null;
                    BD_MSSQL_Clear_MSSQL();
                }
                this.MouseEnterFlag = true;
            }
            catch (Exception ex)
            {
                ApplicationException ae = new ApplicationException(string.Format(@"Ошибка при изменении логина:""{0}""", ex.Message));
                Log.EventSave(ae.Message, string.Format("{0}.txtBox_Login_MSSQL_TextChanged", this.GetType().FullName), EventEn.Error);
                throw ae;
            }
        }

        // Пользователь изменил Password
        private void txtBox_Passvord_MSSQL_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(this.NewConnectionString))
                {
                    this.NewConnectionString = null;
                    BD_MSSQL_Clear_MSSQL();
                }
                this.MouseEnterFlag = true;
            }
            catch (Exception ex)
            {
                ApplicationException ae = new ApplicationException(string.Format(@"Ошибка при изменении пароля:""{0}""", ex.Message));
                Log.EventSave(ae.Message, string.Format("{0}.txtBox_Passvord_MSSQL_TextChanged", this.GetType().FullName), EventEn.Error);
                throw ae;
            }
        }
    }
}
