using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Common;
using System.Reflection;
using System.Drawing;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;

namespace ProviderMsSql
{
    /// <summary>
    /// Класс для реализации провайдера нашей программы для рпаботы с Ms Sql Server
    /// </summary>
    public class ProviderMsSql : Provider, Common.ProviderPlg.ProviderI
    {
        #region Параметры Private

        #endregion

        #region Параметры Public

        public SqlConnectionStringBuilder Scsb;

        #endregion

        #region Методы Public

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="PlugInType">Тип палгина - this.GetType().FullName</param>
        /// <param name="VersionPlg">Версия - плагина</param>
        /// <param name="ConnectionString">Строка подключения к провайдеру</param>
        public ProviderMsSql(string ConnectionString) : base(Assembly.GetExecutingAssembly().FullName, Assembly.GetExecutingAssembly().GetName().Version.ToString(), (string.IsNullOrWhiteSpace(ConnectionString) ? null : ConnectionString))
        {
            try
            {
                //Log.EventSave("Плагин лугойла", "dd", EventEn.Message);
                //this.EventSave("Плагин лугойла yfcktljdfyysq", "dd", EventEn.Message);
            }
            catch (Exception ex)
            {
                ApplicationException ae = new ApplicationException(string.Format("Упали при инициализации конструктора с ошибкой: ({0})", ex.Message));
                this.EventSave(ae.Message, string.Format("{0}.ProviderBase", this.GetType().FullName), EventEn.Error);
                throw ae;
            }
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public ProviderMsSql() : this(null)
        {
            try
            {
                //Log.EventSave("Плагин лугойла", "dd", EventEn.Message);
                //this.EventSave("Плагин лугойла yfcktljdfyysq", "dd", EventEn.Message);
            }
            catch (Exception ex)
            {
                ApplicationException ae = new ApplicationException(string.Format("Упали при инициализации конструктора с ошибкой: ({0})", ex.Message));
                this.EventSave(ae.Message, string.Format("{0}.ProviderBase", this.GetType().FullName), EventEn.Error);
                throw ae;
            }
        }

        /// <summary>
        /// Строим строку подключения по параметрам
        /// </summary>
        /// <param name="Server">Сервер</param>
        /// <param name="ADAud">тип авторизации</param>
        /// <param name="Login">логин</param>
        /// <param name="Password">пароль</param>
        /// <param name="BD">база данных</param>
        /// <param name="VisibleError">Требуется выводить пользователю ошибку или нет</param>
        /// <param name="WriteLog">Логировать в системном логе или нет</param>
        /// <param name="InstalConnect">Сохранить в билдере строки подключения или нет</param>
        /// <returns></returns>
        public string InstalProvider(string Server, bool ADAud, string Login, string Password, string BD, bool VisibleError, bool WriteLog, bool InstalConnect)
        {
            SqlConnectionStringBuilder ScsbTmp = new SqlConnectionStringBuilder();
            ScsbTmp.DataSource = Server;
            ScsbTmp.IntegratedSecurity = ADAud;
            if (!ADAud)
            {
                ScsbTmp.UserID = Login;
                ScsbTmp.Password = Password;
            }
            if (BD != null && BD != string.Empty) ScsbTmp.InitialCatalog = BD;

            try
            {
                if (this.TestConnect(ScsbTmp.ConnectionString, VisibleError))
                {
                    if (InstalConnect) this.Scsb = ScsbTmp;
                    return ScsbTmp.ConnectionString;
                }
                else return null;
            }
            catch (Exception)
            {
                if (WriteLog) Log.EventSave("Не удалось создать подключение: " + Server, this.ToString(), EventEn.Error);
                throw;
            }
        }

        /// <summary>
        /// Получение списка доступных баз данных
        /// </summary>
        /// <param name="ConnectionString">Строка подключеиня которыю используем для получения списка</param>
        /// <returns>Список доступных баз данных</returns>
        public List<string> GetBdList(string ConnectionString)
        {
            List<string> rez = new List<string>();
            string SQL = "Select name, compatibility_level, is_read_only from sys.databases Where state_desc='ONLINE' and name not in ('master','tempdb','model','msdb') Order by name";

            try
            {
                if (Config.Trace) this.EventSave(SQL, GetType().Name + ".GetBdList", EventEn.Dump, true, false);

                // Закрывать конект не нужно он будет закрыт деструктором
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    con.Open();

                    using (SqlCommand com = new SqlCommand(SQL, con))
                    {
                        com.CommandTimeout = 900;  // 15 минут
                        using (SqlDataReader dr = com.ExecuteReader())
                        {

                            if (dr.HasRows)
                            {
                                // пробегаем по строкам
                                while (dr.Read())
                                {
                                    rez.Add(dr.GetValue(0).ToString());
                                }
                            }
                        }
                    }
                }

                return rez;
            }
            catch (SqlException ex)
            {
                base.EventSave(string.Format("Произошла ошибка при получении списка доступных баз даных. {0}", ex.Message), GetType().Name + ".GetBdList", EventEn.Error, true, false);
                if (Config.Trace) base.EventSave(SQL, GetType().Name + ".GetBdList", EventEn.Dump, true, false);
                throw;
            }
            catch (Exception ex)
            {
                base.EventSave(string.Format("Произошла ошибка при получении списка доступных баз даных. {0}", ex.Message), GetType().Name + ".GetBdList", EventEn.Error, true, false);
                if (Config.Trace) base.EventSave(SQL, GetType().Name + ".GetBdList", EventEn.Dump, true, false);
                throw;
            }
            finally
            {
            }
        }

        #endregion

        #region Методы Public Override

        /// <summary>
        /// Проверка строки подключения
        /// </summary>
        /// <param name="ConnectionString">Строка подключения для проверки</param>
        /// <param name="VisibleError">True если при проверке подключения надо выводить сообщения пользователю</param>
        /// <returns>True - Если база доступна | False - Если база не доступна</returns>
        public override bool TestConnect(string ConnectionString, bool VisibleError)
        {
            try
            {
                // Проверка подключения
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    con.Open();
                    base.VersionDB = con.ServerVersion; // Если не упали, значит подключение создано верно, тогда сохраняем переданные параметры
                    con.Close();
                }

                return true;
            }
            catch (Exception ex)
            {
                // Логируем ошибку если её должен видеть пользователь или если взведён флаг трассировке в файле настройки программы
                if (VisibleError || Config.Trace) base.EventSave(string.Format(@"Ошибка при проверке подключения:""{0}""", ex.Message), "TestConnect", EventEn.Error, true, true);

                // Отображаем ошибку если это нужно
                if (VisibleError) MessageBox.Show(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Вывод строки подключения в лог или интерфейс пользователя с затиранием пароля
        /// </summary>
        /// <returns>Строка подключения которую мы можем безопасно передавать пользователю так как пароль должен быть затёрт</returns>
        public override string PrintConnectionString()
        {
            try
            {
                if (base.ConnectionString != null && base.ConnectionString.Trim() != string.Empty)
                {
                    this.Scsb = new SqlConnectionStringBuilder(base.ConnectionString);
                    string Pssword = Scsb.Password;

                    if (string.IsNullOrWhiteSpace(Pssword)) return base.ConnectionString;
                    else
                    {
                        Scsb.Password = "*****";
                        return Scsb.ConnectionString;
                    }
                }
            }
            catch (Exception ex) { base.EventSave(string.Format(@"Ошибка при печати строки подключения:""{0}""", ex.Message), "PrintConnectionString", EventEn.Error, true, true); }

            return null;
        }

        /// <summary>
        /// Вывод строки подключения в лог или интерфейс пользователя с затиранием пароля
        /// </summary>
        /// <param name="Rep">Провайдер который мы хотим править</param>
        /// <returns>True если пользователь решил сохранить провайдер | False если пользователь не хочет сохранять</returns>
        public override bool SetupConnectDB(ref Provider Rep)
        {
            try
            {
                bool HashSaveRepository = false;

                // Вызываем форм для проверки и настройки подключения
                using (FSetupConnectDB Frm = new FSetupConnectDB(this))
                {
                    // Проверяем результат того что сделал пользователь
                    DialogResult drez = Frm.ShowDialog();
                    if (drez == DialogResult.Yes)
                    {
                        base.ConnectionString = Frm.NewConnectionString;

                        HashSaveRepository = true;
                    }
                }

                return HashSaveRepository;
            }
            catch (Exception ex)
            {
                base.EventSave(string.Format(@"Ошибка при создании новой строки подключения к репозиторию SetupConnectDB:""{0}""", ex.Message), this.GetType().FullName, EventEn.Error, true, true);
                throw ex;
            }
        }

        #endregion


        #region Методы Public ProviderI


        #endregion

        #region Методы Private Method

        /// <summary>
        /// Строим строку которую будем потом печатать во время выполнения команды к данному провайдеру
        /// </summary>
        /// <param name="com">Команда</param>
        /// <returns>SQL предложение на основе соманды</returns>
        private string GetStringPrintPar(SqlCommand com)
        {
            string rez = string.Format("exec {0}", com.CommandText);
            try
            {
                // Строим строку которую воткнём в дамп в случае падения
                bool isFirst = true;
                foreach (SqlParameter item in com.Parameters)
                {
                    if (item.Direction != ParameterDirection.ReturnValue)
                    {
                        if (isFirst)
                        {
                            rez += " ";
                            isFirst = false;
                        }
                        else rez += ", ";

                        switch (item.SqlDbType)
                        {
                            case SqlDbType.Int:
                                rez += string.Format("{0}={1}", item.ParameterName, (item.SqlValue == null ? "null" : item.SqlValue));
                                break;
                            case SqlDbType.DateTime:
                                string tmp = "null";
                                if (item.SqlValue != null)
                                {
                                    DateTime dt = (DateTime)item.Value;
                                    // DateTime dt = DateTime.Parse(item.SqlValue.ToString());
                                    tmp = string.Format("Declare @P{0} datetime = convert(datetime,convert(varchar, '{1}.{2:D3}', 21),21);", item.ParameterName.Replace(@"@", ""), dt.ToString("yyyy-MM-dd HH:mm:ss"), dt.Millisecond);

                                    rez = tmp + "\r\n" + rez + string.Format("{0}={1}", item.ParameterName, @"@P" + item.ParameterName.Replace(@"@", ""));
                                }
                                else rez += string.Format("{0}={1}", item.ParameterName, "null");
                                break;
                            case SqlDbType.Money:
                                rez += string.Format("{0}={1}", item.ParameterName, (item.SqlValue == null ? "null" : item.SqlValue.ToString().Replace(",", ".")));
                                break;
                            default:
                                rez += string.Format("{0}={1}", item.ParameterName, (item.SqlValue == null ? "null" : string.Format("'{0}'", item.Value)));
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                base.EventSave(string.Format("Произошла ошибка при парсинге параметров. {0}", ex.Message), GetType().Name + ".GetStringPrintPar", EventEn.Error, true, false);
                throw;
            }
            return rez;
        }

        #endregion



    }
}
