using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.IoPlg
{
    /// <summary>
    /// Базовый класс наших плагинов
    /// </summary>
    public abstract partial class IoBase
    {
        #region Param (private)

        /// <summary>
        /// Интерфейс для базового класса чтобы он мог дёргать скрытыем методы
        /// </summary>
        private IoI InterfIo = null;

        /// <summary>
        /// Тип палгина
        /// </summary>
        public string PlugInType { get; private set; } = null;

        /// <summary>
        /// Возвращает версию плагина
        /// </summary>
        public string VersionPlg { get; private set; }

        #endregion

        #region Param (public get; protected set;)

        /// <summary>
        /// Индекс в списке IoList
        /// </summary>
        public int index { get; protected set; } = -1;

        #endregion


        #region Method (public)

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="PlugInType">Тип палгина - this.GetType().FullName || Assembly.GetExecutingAssembly().FullName</param>
        /// <param name="VersionPlg">Версия плагина - Assembly.GetExecutingAssembly().GetName().Version.ToString()</param>
        public IoBase(string PlugInType, string VersionPlg)
        {
            try
            {
                if (PlugInType.IndexOf(",") > 0)
                {
                    this.PlugInType = PlugInType.Substring(0, PlugInType.IndexOf(","));
                }
                else this.PlugInType = PlugInType;
                this.VersionPlg = VersionPlg;
            }
            catch (Exception ex)
            {
                ApplicationException ae = new ApplicationException(string.Format("Упали при инициализации конструктора с ошибкой: ({0})", ex.Message));
                Log.EventSave(ae.Message, string.Format("{0}.IoBase", this.GetType().FullName), EventEn.Error);
                throw ae;
            }
        }

        #endregion

        #region Method (public virtual)


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

        /*
        #region CrossClass

        /// <summary>
        /// Внутренний класс для линковки интерфейсов састомного класса скрытых для пользователя
        /// </summary>
        public class CrossLink
        {
            /// <summary>
            /// Линкуеминтерфейс IoI скрытый для пользователя
            /// </summary>
            /// <param name="CustIo">Кастомный обьект для линковки</param>
            public CrossLink(IoBase CustIo)
            {
                CustIo.InterfIo = (IoI)CustIo;
            }
        }

        #endregion
        */
    }
}
