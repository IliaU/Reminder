﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.RepositoryPlg
{
    /// <summary>
    /// Абстрактный класс для репозитория
    /// </summary>
    public abstract class RepositoryBase
    {
        #region Param (private)
        /// <summary>
        /// Строка подключения
        /// </summary>
        private string _ConnectionString;

        #endregion

        #region Param (public get; protected set;)

        /// <summary>
        /// Тип палгина
        /// </summary>
        public string PlugInType { get; private set; } = null;


        /// <summary>
        /// Возвращает версию плагина
        /// </summary>
        public string VersionPlg { get; private set; }

        /// <summary>
        /// Информация о версии базы данных
        /// </summary>
        public string VersionDB { get; protected set; }

        /// <summary>
        /// Наличие подключения к источнику данных
        /// </summary>
        public bool HashConnect { get; protected set; } = false;

        /// <summary>
        /// Строка подключения
        /// </summary>
        public string ConnectionString
        {
            get
            {
                return this._ConnectionString;
            }
            protected set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    this._ConnectionString = null;
                    this.HashConnect = false;
                }
                else
                {
                    this._ConnectionString = value.ToString();
                    this.HashConnect = this.TestConnect();
                }
            }
        }

        #endregion

        #region Param (protected)

        #endregion

        #region Method (public)

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="PlugInType">Тип палгина - this.GetType().FullName || Assembly.GetExecutingAssembly().FullName</param>
        /// <param name="VersionPlg">Версия плагина - Assembly.GetExecutingAssembly().GetName().Version.ToString()</param>
        /// <param name="ConnectionString">Строка подключения к репозиторию</param>
        public RepositoryBase(string PlugInType, string VersionPlg, string ConnectionString)
        {
            try
            {
                if (PlugInType.IndexOf(",") > 0)
                {
                    this.PlugInType = PlugInType.Substring(0, PlugInType.IndexOf(","));
                }
                else this.PlugInType = PlugInType;
                this.VersionPlg = VersionPlg;
                this.ConnectionString = ConnectionString;

                Log.EventSave(string.Format("Загружен плагин репозитория {0} ({1})", this.PlugInType, this.VersionPlg), this.GetType().FullName, EventEn.Message);
            }
            catch (Exception ex)
            {
                ApplicationException ae = new ApplicationException(string.Format("Упали при инициализации конструктора с ошибкой: ({0})", ex.Message));
                Log.EventSave(ae.Message, string.Format("{0}.RepositoryBase", this.GetType().FullName), EventEn.Error);
                throw ae;
            }
        }

        /// <summary>
        /// Проверка строки подключения
        /// </summary>
        /// <returns>True - Если база доступна | False - Если база не доступна</returns>
        public bool TestConnect()
        {
            try
            {
                HashConnect = TestConnect(this.ConnectionString, false);

                return HashConnect;
            }
            catch (Exception ex)
            {
                ApplicationException ae = new ApplicationException(string.Format(@"Ошибка в методе проверки подключения:""{0}""", ex.Message));
                EventSave(ae.Message, string.Format("{0}.TestConnect()", this.GetType().FullName), EventEn.Error);
                throw ae;
            }
        }

        #endregion

        #region Method (protected)

        /// <summary>
        /// Метод для записи информации в лог
        /// </summary>
        /// <param name="Message">Сообщение</param>
        /// <param name="Source">Источник</param>
        /// <param name="evn">Тип события</param>
        /// <param name="IsLog">Писать в лог или нет</param>
        /// <param name="Show">Отобразить сообщение пользователю или нет</param>
        protected void EventSave(string Message, string Source, EventEn evn, bool IsLog, bool Show)
        {
            try
            {
                Log.EventSave(Message, string.Format("({0}).{1}", this.PlugInType, Source), evn, IsLog, Show);
            }
            catch (Exception ex)
            {
                ApplicationException ae = new ApplicationException(string.Format("Упали при обработке сообщения для лога с ошибкой: ({0})", ex.Message));
                Log.EventSave(ae.Message, string.Format("{0}.EventSave", this.GetType().FullName), EventEn.Error);
                throw ae;
            }
        }

        /// <summary>
        /// Событие программы
        /// </summary>
        /// <param name="Message">Сообщение</param>
        /// <param name="Source">Источник</param>
        /// <param name="evn">Тип события</param>
        /// <param name="IsLog">Писать в лог или нет</param>
        /// <param name="Show">Отобразить сообщение пользователю или нет</param>
        protected void EventSave(string Message, string Source, EventEn evn)
        {
            try
            {
                EventSave(Message, Source, evn, true, false);
            }
            catch (Exception ex)
            {
                ApplicationException ae = new ApplicationException(string.Format("Упали при обработке сообщения для лога с ошибкой: ({0})", ex.Message));
                Log.EventSave(ae.Message, string.Format("{0}.EventSave", this.GetType().FullName), EventEn.Error);
                throw ae;
            }
        }

        #endregion

        #region Method (virtual)

        /// <summary>
        /// Проверка строки подключения
        /// </summary>
        /// <param name="ConnectionString">Строка подключения для проверки</param>
        /// <param name="VisibleError">True если при проверке подключения надо выводить сообщения пользователю</param>
        /// <returns>True - Если база доступна | False - Если база не доступна</returns>
        public virtual bool TestConnect(string ConnectionString, bool VisibleError)
        {
            try
            {
                throw new ApplicationException("Необходимо перезаписать метод TestConnect в наследуемом классе чтобы обработать данный метод.");
            }
            catch (Exception ex)
            {
                ApplicationException ae = new ApplicationException(string.Format(@"Ошибка в методе проверки подключения:""{0}""", ex.Message));
                EventSave(ae.Message, string.Format("{0}.TestConnect(ConnectionString, VisibleError)", this.GetType().FullName), EventEn.Error);
                throw ae;
            }
        }

        /// <summary>
        /// Вывод строки подключения в лог или интерфейс пользователя с затиранием пароля
        /// </summary>
        /// <returns>Строка подключения которую мы можем безопасно передавать пользователю так как пароль должен быть затёрт</returns>
        public virtual string PrintConnectionString()
        {
            try
            {
                throw new ApplicationException("Необходимо перезаписать метод PrintConnectionString в наследуемом классе чтобы обработать данный метод.");
            }
            catch (Exception ex)
            {
                ApplicationException ae = new ApplicationException(string.Format(@"Ошибка в методе печати строки подключения:""{0}""", ex.Message));
                EventSave(ae.Message, string.Format("{0}.PrintConnectionString", this.GetType().FullName), EventEn.Error);
                throw ae;
            }
        }

        /// <summary>
        /// Вывод строки подключения в лог или интерфейс пользователя с затиранием пароля
        /// </summary>
        /// <param name="Rep">Репозиторий который мы хотим править</param>
        /// <returns>True если пользователь решил сохранить репозиторй | False если пользователь не хочет сохранять</returns>
        public virtual bool SetupConnectDB(ref Repository Rep)
        {
            try
            {
                throw new ApplicationException("Необходимо перезаписать метод SetupConnectDB в наследуемом классе чтобы обработать данный метод.");
            }
            catch (Exception ex)
            {
                ApplicationException ae = new ApplicationException(string.Format(@"Ошибка в методе печати строки подключения:""{0}""", ex.Message));
                EventSave(ae.Message, string.Format("{0}.SetupConnectDB", this.GetType().FullName), EventEn.Error);
                throw ae;
            }
        }

        // <summary>
        /// Запись лога в базу данных
        /// </summary>
        /// <param name="Message">Сообщение которое пишем в базу данных</param>
        /// <param name="Source">Источник где оно возникло</param>
        /// <param name="evn">Событие системное которое фиксируем</param>
        public virtual void EventSaveDb(string Message, string Source, EventEn evn)
        {
            try
            {
                throw new ApplicationException("Необходимо перезаписать метод EventSave(string Message, string Source, EventEn evn) в наследуемом классе чтобы была возмоность писать в лог.");
            }
            catch (Exception ex)
            {
                ApplicationException ae = new ApplicationException(string.Format(@"Ошибка в методе печати строки подключения:""{0}""", ex.Message));
                EventSave(ae.Message, string.Format("{0}.EventSaveDb", this.GetType().FullName), EventEn.Error);
                throw ae;
            }
        }

        /*
        /// <summary>
        /// Получение списка инстансов из разы репозитория
        /// </summary>
        /// <returns>Возвращаем списокинстансов</returns>
        public virtual List<TInstance> GetTInstanceList()
        {
            try
            {
                throw new ApplicationException("Необходимо перезаписать метод List<TInstance> GetTInstanceList() в наследуемом классе чтобы метод работал корректно.");
            }
            catch (Exception ex)
            {
                Com.Log.EventSave(string.Format(@"Ошибка в методе GetTInstanceList:""{0}""", ex.Message), this.GetType().FullName, EventEn.Error, true, true);
                throw ex;
            }
        }
        */

        #endregion
    }
}
