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

namespace RepositoryMsSql
{
    /// <summary>
    /// Класс для реализации репозитория нашей программы для рпаботы с Ms Sql Server
    /// </summary>
    public class RepositoryMsSql : Repository, Common.RepositoryPlg.RepositoryI
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
        /// <param name="ConnectionString">Строка подключения к репозиторию</param>
        public RepositoryMsSql(string ConnectionString) : base(Assembly.GetExecutingAssembly().FullName, Assembly.GetExecutingAssembly().GetName().Version.ToString(), (string.IsNullOrWhiteSpace(ConnectionString) ? null : ConnectionString))
        {
            try
            {
                //Log.EventSave("Плагин лугойла", "dd", EventEn.Message);
                //this.EventSave("Плагин лугойла yfcktljdfyysq", "dd", EventEn.Message);
            }
            catch (Exception ex)
            {
                ApplicationException ae = new ApplicationException(string.Format("Упали при инициализации конструктора с ошибкой: ({0})", ex.Message));
                this.EventSave(ae.Message, string.Format("{0}.RepositoryBase", this.GetType().FullName), EventEn.Error);
                throw ae;
            }
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public RepositoryMsSql() : this (null)
        {
            try
            {
                //Log.EventSave("Плагин лугойла", "dd", EventEn.Message);
                //this.EventSave("Плагин лугойла yfcktljdfyysq", "dd", EventEn.Message);
            }
            catch (Exception ex)
            {
                ApplicationException ae = new ApplicationException(string.Format("Упали при инициализации конструктора с ошибкой: ({0})", ex.Message));
                this.EventSave(ae.Message, string.Format("{0}.RepositoryBase", this.GetType().FullName), EventEn.Error);
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
        public string InstalRepository(string Server, bool ADAud, string Login, string Password, string BD, bool VisibleError, bool WriteLog, bool InstalConnect)
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
        /// <param name="Rep">Репозиторий который мы хотим править</param>
        /// <returns>True если пользователь решил сохранить репозиторй | False если пользователь не хочет сохранять</returns>
        public override bool SetupConnectDB(ref Repository Rep)
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

        /// <summary>
        /// Запись лога в базу данных
        /// </summary>
        /// <param name="Message">Сообщение которое пишем в базу данных</param>
        /// <param name="Source">Источник где оно возникло</param>
        /// <param name="evn">Событие системное которое фиксируем</param>
        public override void EventSaveDb(string Message, string Source, EventEn evn)
        {
            //int rez = 0;
            string SQL = string.Format("insert into [io].[Log]([DateTime], [Message], [Source], [Status]) Values(GetDate(), '{0}', '{1}', '{2}')"
                , Message.Replace("'", "''"), Source.Replace("'", "''"), evn.ToString());

            try
            {
                if (!base.HashConnect) throw new ApplicationException("Нет подключение к репозиторию данных.");

                if (Config.Trace) base.EventSave(SQL, GetType().Name + ".EventSaveDb", EventEn.Dump, true, false);

                // Закрывать конект не нужно он будет закрыт деструктором
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    con.Open();

                    using (SqlCommand com = new SqlCommand(SQL, con))
                    {
                        // Запускаем процедуру
                        com.ExecuteNonQuery();
                    }
                }

                //return rez;
            }
            catch (SqlException ex)
            {
                throw new ApplicationException(string.Format("Произошла ошибка при получении списка доступных баз даных. {0}", ex.Message));
            }
            catch (Exception ex)
            {
                throw new ApplicationException(string.Format("Произошла ошибка при получении списка доступных баз даных. {0}", ex.Message));
            }
            finally
            {
            }
        }

        #endregion


        #region Методы Public RepositoryI

        /// <summary>
        /// Метод для регистрации состояния пула с потоками
        /// </summary>
        /// <param name="MachineName">Имя машины где работает пул потоков</param>
        /// <param name="CustomClassTyp">Имя класса с потоками этого вида</param>
        /// <param name="LastDateReflection">Дата и время последнего получения статуса</param>
        /// <param name="VersionPul">Версия пула с потоками</param>
        /// <param name="LastStatusCustom">Статус который получили от самих потоков в этом пуле</param>
        public void PulBasicSetStatus(string MachineName, string CustomClassTyp, DateTime LastDateReflection, string VersionPul, string LastStatusCustom)
        {
            string SQL = "[io].[PulBasicSetStatus]";
            try
            {
                // Проверка подключения
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    con.Open();

                    using (SqlCommand com = new SqlCommand(SQL, con))
                    {
                        com.CommandTimeout = 900;  // 15 минут
                        com.CommandType = CommandType.StoredProcedure;
                        //
                        //SqlParameter PIdOut = new SqlParameter("@IdOut", SqlDbType.Int);
                        //PIdOut.Direction = ParameterDirection.ReturnValue;
                        //com.Parameters.Add(PIdOut);
                        //
                        //SqlParameter PId = new SqlParameter("@Id", SqlDbType.Int);
                        //PId.Direction = ParameterDirection.Input;
                        //if (nTInstance.ID != null) PId.Value = (int)nTInstance.ID;
                        //com.Parameters.Add(PId);
                        //
                        SqlParameter PMachineName = new SqlParameter("@MachineName", SqlDbType.VarChar, 100);
                        PMachineName.Direction = ParameterDirection.Input;
                        PMachineName.Value = MachineName;
                        com.Parameters.Add(PMachineName);
                        //
                        SqlParameter PCustomClassTyp = new SqlParameter("@CustomClassTyp", SqlDbType.VarChar, 300);
                        PCustomClassTyp.Direction = ParameterDirection.Input;
                        PCustomClassTyp.Value = CustomClassTyp;
                        com.Parameters.Add(PCustomClassTyp);
                        //
                        SqlParameter PLastDateReflection = new SqlParameter("@LastDateReflection", SqlDbType.DateTime);
                        PLastDateReflection.Direction = ParameterDirection.Input;
                        PLastDateReflection.Value = LastDateReflection;
                        com.Parameters.Add(PLastDateReflection);
                        //
                        SqlParameter PVersionPul = new SqlParameter("@VersionPul", SqlDbType.VarChar, 50);
                        PVersionPul.Direction = ParameterDirection.Input;
                        PVersionPul.Value = VersionPul;
                        com.Parameters.Add(PVersionPul);
                        //
                        SqlParameter PLastStatusCustom = new SqlParameter("@LastStatusCustom", SqlDbType.VarChar, 50);
                        PLastStatusCustom.Direction = ParameterDirection.Input;
                        PLastStatusCustom.Value = LastStatusCustom;
                        com.Parameters.Add(PLastStatusCustom);

                        // Строим строку которую воткнём в дамп в случае падения
                        SQL = GetStringPrintPar(com);

                        // Запускаем процедуру
                        com.ExecuteNonQuery();

                        // Получаем идентификатор товара
                        //rez = int.Parse(PIdOut.Value.ToString());
                    }

                    con.Close();
                }
            }
            catch (SqlException ex)
            {
                // Логируем ошибку если её должен видеть пользователь или если взведён флаг трассировке в файле настройки программы
                ApplicationException xe = new ApplicationException(string.Format(@"Ошибка при сохранении статуса ноды в репозиторий:""{0}""", ex.Message));
                if (Config.Trace) base.EventSave(xe.Message, string.Format("{0}.PulBasicSetStatus", this.GetType().FullName), EventEn.Error, true, false);
                throw xe;
            }
            catch (Exception ex)
            {
                // Логируем ошибку если её должен видеть пользователь или если взведён флаг трассировке в файле настройки программы
                ApplicationException xe = new ApplicationException(string.Format(@"Ошибка при сохранении статуса ноды в репозиторий:""{0}""", ex.Message));
                if (Config.Trace) base.EventSave(xe.Message, string.Format("{0}.PulBasicSetStatus", this.GetType().FullName), EventEn.Error, true, false);
                throw xe;
            }
        }

        /// <summary>
        /// Метод для регистрации состояния всей ноды воркера
        /// </summary>
        /// <param name="MachineName">Имя машины где работает пул потоков</param>
        /// <param name="LastDateReflection">Дата и время последнего получения статуса</param>
        /// <param name="VersionNode">Версия воркера</param>
        /// <param name="LastStatusNode">Статус который получили от самой ноды</param>
        public void NodeSetStatus(string MachineName, DateTime LastDateReflection, string VersionNode, string LastStatusNode)
        {
            string SQL = "[io].[NodeSetStatus]";
            try
            {
                // Проверка подключения
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    con.Open();

                    using (SqlCommand com = new SqlCommand(SQL, con))
                    {
                        com.CommandTimeout = 900;  // 15 минут
                        com.CommandType = CommandType.StoredProcedure;
                        //
                        //SqlParameter PIdOut = new SqlParameter("@IdOut", SqlDbType.Int);
                        //PIdOut.Direction = ParameterDirection.ReturnValue;
                        //com.Parameters.Add(PIdOut);
                        //
                        //SqlParameter PId = new SqlParameter("@Id", SqlDbType.Int);
                        //PId.Direction = ParameterDirection.Input;
                        //if (nTInstance.ID != null) PId.Value = (int)nTInstance.ID;
                        //com.Parameters.Add(PId);
                        //
                        SqlParameter PMachineName = new SqlParameter("@MachineName", SqlDbType.VarChar, 100);
                        PMachineName.Direction = ParameterDirection.Input;
                        PMachineName.Value = MachineName;
                        com.Parameters.Add(PMachineName);
                        //
                        SqlParameter PLastDateReflection = new SqlParameter("@LastDateReflection", SqlDbType.DateTime);
                        PLastDateReflection.Direction = ParameterDirection.Input;
                        PLastDateReflection.Value = LastDateReflection;
                        com.Parameters.Add(PLastDateReflection);
                        //
                        SqlParameter PVersionPul = new SqlParameter("@VersionNode", SqlDbType.VarChar, 50);
                        PVersionPul.Direction = ParameterDirection.Input;
                        PVersionPul.Value = VersionNode;
                        com.Parameters.Add(PVersionPul);
                        //
                        SqlParameter PLastStatusCustom = new SqlParameter("@LastStatusNode", SqlDbType.VarChar, 50);
                        PLastStatusCustom.Direction = ParameterDirection.Input;
                        PLastStatusCustom.Value = LastStatusNode;
                        com.Parameters.Add(PLastStatusCustom);

                        // Строим строку которую воткнём в дамп в случае падения
                        SQL = GetStringPrintPar(com);

                        // Запускаем процедуру
                        com.ExecuteNonQuery();

                        // Получаем идентификатор товара
                        //rez = int.Parse(PIdOut.Value.ToString());
                    }

                    con.Close();
                }
            }
            catch (SqlException ex)
            {
                // Логируем ошибку если её должен видеть пользователь или если взведён флаг трассировке в файле настройки программы
                ApplicationException xe = new ApplicationException(string.Format(@"Ошибка при сохранении статуса ноды в репозиторий:""{0}""", ex.Message));
                if (Config.Trace) base.EventSave(xe.Message, string.Format("{0}.NodeSetStatus", this.GetType().FullName), EventEn.Error, true, false);
                throw xe;
            }
            catch (Exception ex)
            {
                // Логируем ошибку если её должен видеть пользователь или если взведён флаг трассировке в файле настройки программы
                ApplicationException xe = new ApplicationException(string.Format(@"Ошибка при сохранении статуса ноды в репозиторий:""{0}""", ex.Message));
                if (Config.Trace) base.EventSave(xe.Message, string.Format("{0}.NodeSetStatus", this.GetType().FullName), EventEn.Error, true, false);
                throw xe;
            }
        }

        /// <summary>
        /// Сохранение провайдеоа в репозиторий
        /// </summary>
        /// <param name="Prv">Провайдер который сохраняем</param>
        /// <param name="Mon">Какой тип провайдера для мониторинга или для объектов</param>
        public void SaveProvider(Provider Prv, bool Mon)
        {
            string SQL = "[io].[SaveConfig]";
            string QSQL1 = null;
            string QSQL2 = null;
            try
            {
                // Проверка подключения
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    con.Open();

                    using (SqlCommand com = new SqlCommand(SQL, con))
                    {
                        com.CommandTimeout = 900;  // 15 минут
                        com.CommandType = CommandType.StoredProcedure;
                        //
                        //SqlParameter PIdOut = new SqlParameter("@IdOut", SqlDbType.Int);
                        //PIdOut.Direction = ParameterDirection.ReturnValue;
                        //com.Parameters.Add(PIdOut);
                        //
                        //SqlParameter PId = new SqlParameter("@Id", SqlDbType.Int);
                        //PId.Direction = ParameterDirection.Input;
                        //if (nTInstance.ID != null) PId.Value = (int)nTInstance.ID;
                        //com.Parameters.Add(PId);
                        //
                        SqlParameter PParamSpace = new SqlParameter("@ParamSpace", SqlDbType.VarChar, 300);
                        PParamSpace.Direction = ParameterDirection.Input;
                        PParamSpace.Value = "io";
                        com.Parameters.Add(PParamSpace);
                        //
                        SqlParameter PParamGroup = new SqlParameter("@ParamGroup", SqlDbType.VarChar, 300);
                        PParamGroup.Direction = ParameterDirection.Input;
                        PParamGroup.Value = (Mon ? "ProviderMon" : "ProviderObj");
                        com.Parameters.Add(PParamGroup);
                        //
                        SqlParameter PParamName = new SqlParameter("@ParamName", SqlDbType.VarChar, 300);
                        PParamName.Direction = ParameterDirection.Input;
                        PParamName.Value = "PlugInType";
                        com.Parameters.Add(PParamName);
                        //
                        SqlParameter PValStr0 = new SqlParameter("@ValStr0", SqlDbType.VarChar, 1000);
                        PValStr0.Direction = ParameterDirection.Input;
                        PValStr0.Value = Prv.PlugInType;
                        com.Parameters.Add(PValStr0);


                        // Строим строку которую воткнём в дамп в случае падения
                        QSQL1 = GetStringPrintPar(com);

                        // Запускаем процедуру
                        com.ExecuteNonQuery();

                        // Получаем идентификатор товара
                        //rez = int.Parse(PIdOut.Value.ToString());
                    }


                    using (SqlCommand com = new SqlCommand(SQL, con))
                    {
                        com.CommandTimeout = 900;  // 15 минут
                        com.CommandType = CommandType.StoredProcedure;
                        //
                        //SqlParameter PIdOut = new SqlParameter("@IdOut", SqlDbType.Int);
                        //PIdOut.Direction = ParameterDirection.ReturnValue;
                        //com1.Parameters.Add(PIdOut);
                        //
                        //SqlParameter PId = new SqlParameter("@Id", SqlDbType.Int);
                        //PId.Direction = ParameterDirection.Input;
                        //if (nTInstance.ID != null) PId.Value = (int)nTInstance.ID;
                        //com1.Parameters.Add(PId);
                        //
                        SqlParameter PParamSpace = new SqlParameter("@ParamSpace", SqlDbType.VarChar, 300);
                        PParamSpace.Direction = ParameterDirection.Input;
                        PParamSpace.Value = "io";
                        com.Parameters.Add(PParamSpace);
                        //
                        SqlParameter PParamGroup = new SqlParameter("@ParamGroup", SqlDbType.VarChar, 300);
                        PParamGroup.Direction = ParameterDirection.Input;
                        PParamGroup.Value = (Mon ? "ProviderMon" : "ProviderObj");
                        com.Parameters.Add(PParamGroup);
                        //
                        SqlParameter PParamName = new SqlParameter("@ParamName", SqlDbType.VarChar, 300);
                        PParamName.Direction = ParameterDirection.Input;
                        PParamName.Value = "ConnectionString";
                        com.Parameters.Add(PParamName);
                        //
                        SqlParameter PValStr0 = new SqlParameter("@ValStr0", SqlDbType.VarChar, 1000);
                        PValStr0.Direction = ParameterDirection.Input;
                        PValStr0.Value = Prv.ConnectionString;
                        com.Parameters.Add(PValStr0);


                        // Строим строку которую воткнём в дамп в случае падения
                        QSQL2 = GetStringPrintPar(com);

                        // Запускаем процедуру
                        com.ExecuteNonQuery();

                        // Получаем идентификатор товара
                        //rez = int.Parse(PIdOut.Value.ToString());
                    }

                    con.Close();
                }
            }
            catch (SqlException ex)
            {
                // Логируем ошибку если её должен видеть пользователь или если взведён флаг трассировке в файле настройки программы
                ApplicationException xe = new ApplicationException(string.Format(@"Ошибка при сохранении провайдера в репозиторий:""{0}""", ex.Message));
                if (Config.Trace) base.EventSave(xe.Message, string.Format("{0}.SaveProvider", this.GetType().FullName), EventEn.Error, true, false);
                throw xe;
            }
            catch (Exception ex)
            {
                // Логируем ошибку если её должен видеть пользователь или если взведён флаг трассировке в файле настройки программы
                ApplicationException xe = new ApplicationException(string.Format(@"Ошибка при сохранении провайдера в репозиторий:""{0}""", ex.Message));
                if (Config.Trace) base.EventSave(xe.Message, string.Format("{0}.SaveProvider", this.GetType().FullName), EventEn.Error, true, false);
                throw xe;
            }
        }

        /// <summary>
        /// Получение провайдера из базы
        /// </summary>
        /// <param name="Mon">Какой тип провайдера для мониторинга или для объектов</param>
        /// <returns>Провайдер полученный из базы</returns>
        public Provider SelectProvider(bool Mon)
        {
            Provider rez = null;


            string SQL = "[io].[SelectConfig]";
            try
            {
                // Проверка подключения
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    con.Open();

                    using (SqlCommand com = new SqlCommand(SQL, con))
                    {
                        com.CommandTimeout = 900;  // 15 минут
                        com.CommandType = CommandType.StoredProcedure;
                        //
                        //SqlParameter PIdOut = new SqlParameter("@IdOut", SqlDbType.Int);
                        //PIdOut.Direction = ParameterDirection.ReturnValue;
                        //com.Parameters.Add(PIdOut);
                        //
                        //SqlParameter PId = new SqlParameter("@Id", SqlDbType.Int);
                        //PId.Direction = ParameterDirection.Input;
                        //if (nTInstance.ID != null) PId.Value = (int)nTInstance.ID;
                        //com.Parameters.Add(PId);
                        //
                        SqlParameter PParamSpace = new SqlParameter("@ParamSpace", SqlDbType.VarChar, 300);
                        PParamSpace.Direction = ParameterDirection.Input;
                        PParamSpace.Value = "io";
                        com.Parameters.Add(PParamSpace);
                        //
                        SqlParameter PParamGroup = new SqlParameter("@ParamGroup", SqlDbType.VarChar, 300);
                        PParamGroup.Direction = ParameterDirection.Input;
                        PParamGroup.Value = (Mon ? "ProviderMon" : "ProviderObj");
                        com.Parameters.Add(PParamGroup);
                        //
                        SqlParameter PParamName = new SqlParameter("@ParamName", SqlDbType.VarChar, 300);
                        PParamName.Direction = ParameterDirection.Input;
                        PParamName.Value = "%";
                        com.Parameters.Add(PParamName);

                        // Строим строку которую воткнём в дамп в случае падения
                        SQL = GetStringPrintPar(com);

                        // Запускаем процедуру                        
                        using (SqlDataReader dr = com.ExecuteReader())
                        {

                            if (dr.HasRows)
                            {
                                string RPlugInType = null;
                                string RConnectionString = null;

                                // пробегаем по строкам
                                while (dr.Read())
                                {
                                    string TmpParamName = null;
                                    string TmpValStr0 = null;

                                    // пробегаем по столбцам
                                    for (int i = 0; i < dr.FieldCount; i++)
                                    {
                                        if (dr.GetName(i) == "ValStr0") TmpValStr0 = dr.GetValue(i).ToString();
                                        if (dr.GetName(i) == "ParamName") TmpParamName = dr.GetValue(i).ToString();
                                    }

                                    switch (TmpParamName)
                                    {
                                        case "PlugInType":
                                            RPlugInType = TmpValStr0;
                                            break;
                                        case "ConnectionString":
                                            RConnectionString = TmpValStr0;
                                            break;
                                        default:
                                            break;
                                    }
                                }

                                // Проверяем все необходимы параметры
                                if (!string.IsNullOrWhiteSpace(RPlugInType) && !string.IsNullOrWhiteSpace(RConnectionString))
                                {
                                    rez = ProviderFarm.CreateNewProvider(RPlugInType, RConnectionString);
                                }
                            }
                        }

                        // Получаем идентификатор товара
                        //rez = int.Parse(PIdOut.Value.ToString());
                    }

                    con.Close();
                }
            }
            catch (SqlException ex)
            {
                // Логируем ошибку если её должен видеть пользователь или если взведён флаг трассировке в файле настройки программы
                ApplicationException xe = new ApplicationException(string.Format(@"Ошибка при получении провайдера из репозитория:""{0}""", ex.Message));
                if (Config.Trace) base.EventSave(xe.Message, string.Format("{0}.SelectProvider", this.GetType().FullName), EventEn.Error, true, false);
                throw xe;
            }
            catch (Exception ex)
            {
                // Логируем ошибку если её должен видеть пользователь или если взведён флаг трассировке в файле настройки программы
                ApplicationException xe = new ApplicationException(string.Format(@"Ошибка при получении провайдера из репозитория:""{0}""", ex.Message));
                if (Config.Trace) base.EventSave(xe.Message, string.Format("{0}.SelectProvider", this.GetType().FullName), EventEn.Error, true, false);
                throw xe;
            }

            return rez;
        }

        /// <summary>
        /// Получение актуального задания на основе фильтра который относится к текущей ноде
        /// </summary>
        /// <param name="DraftTask"></param>
        /// <returns></returns>
        public IoTask GetListinerTask(IoTaskFilter DraftTask)
        {
            IoTask rez=null;

            string SQL = "[io].[NodeSetStatus]";
            try
            {/*
                // Проверка подключения
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    con.Open();

                    using (SqlCommand com = new SqlCommand(SQL, con))
                    {
                        com.CommandTimeout = 900;  // 15 минут
                        com.CommandType = CommandType.StoredProcedure;
                        //
                        //SqlParameter PIdOut = new SqlParameter("@IdOut", SqlDbType.Int);
                        //PIdOut.Direction = ParameterDirection.ReturnValue;
                        //com.Parameters.Add(PIdOut);
                        //
                        //SqlParameter PId = new SqlParameter("@Id", SqlDbType.Int);
                        //PId.Direction = ParameterDirection.Input;
                        //if (nTInstance.ID != null) PId.Value = (int)nTInstance.ID;
                        //com.Parameters.Add(PId);
                        //
                        SqlParameter PMachineName = new SqlParameter("@MachineName", SqlDbType.VarChar, 100);
                        PMachineName.Direction = ParameterDirection.Input;
                        PMachineName.Value = MachineName;
                        com.Parameters.Add(PMachineName);
                        //
                        SqlParameter PLastDateReflection = new SqlParameter("@LastDateReflection", SqlDbType.DateTime);
                        PLastDateReflection.Direction = ParameterDirection.Input;
                        PLastDateReflection.Value = LastDateReflection;
                        com.Parameters.Add(PLastDateReflection);
                        //
                        SqlParameter PVersionPul = new SqlParameter("@VersionNode", SqlDbType.VarChar, 50);
                        PVersionPul.Direction = ParameterDirection.Input;
                        PVersionPul.Value = VersionNode;
                        com.Parameters.Add(PVersionPul);
                        //
                        SqlParameter PLastStatusCustom = new SqlParameter("@LastStatusNode", SqlDbType.VarChar, 50);
                        PLastStatusCustom.Direction = ParameterDirection.Input;
                        PLastStatusCustom.Value = LastStatusNode;
                        com.Parameters.Add(PLastStatusCustom);

                        // Строим строку которую воткнём в дамп в случае падения
                        SQL = GetStringPrintPar(com);

                        // Запускаем процедуру
                        com.ExecuteNonQuery();

                        // Получаем идентификатор товара
                        //rez = int.Parse(PIdOut.Value.ToString());
                    }

                    con.Close();
                }
                */
            }
            catch (SqlException ex)
            {
                // Логируем ошибку если её должен видеть пользователь или если взведён флаг трассировке в файле настройки программы
                ApplicationException xe = new ApplicationException(string.Format(@"Ошибка при чтении задания из репозитория:""{0}""", ex.Message));
                if (Config.Trace) base.EventSave(xe.Message, string.Format("{0}.GetListinerTask", this.GetType().FullName), EventEn.Error, true, false);
                throw xe;
            }
            catch (Exception ex)
            {
                // Логируем ошибку если её должен видеть пользователь или если взведён флаг трассировке в файле настройки программы
                ApplicationException xe = new ApplicationException(string.Format(@"Ошибка при чтении задания из репозитория:""{0}""", ex.Message));
                if (Config.Trace) base.EventSave(xe.Message, string.Format("{0}.GetListinerTask", this.GetType().FullName), EventEn.Error, true, false);
                throw xe;
            }

            return rez;
        }

        #endregion

        #region Методы Private Method

        /// <summary>
        /// Строим строку которую будем потом печатать во время выполнения команды к данному репозиторию
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
