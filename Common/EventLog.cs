using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;

namespace Common
{
    /// <summary>
    /// Событие которое фиксируется в логе
    /// </summary>
    public class EventLog : EventArgs
    {
        /// <summary>
        /// Сообщение
        /// </summary>
        public string Message { get; private set; }

        /// <summary>
        /// Источник
        /// </summary>
        public string Source { get; private set; }

        /// <summary>
        /// Тип события
        /// </summary>
        public EventEn Evn { get; private set; }

        /// <summary>
        /// Писать в лог
        /// </summary>
        public bool isLog;

        /// <summary>
        /// Показывать пользователю
        /// </summary>
        public bool Show;

        /// <summary>
        /// Передаём таблицу которая может иметь результат для основного окна и потенциально писать сообщения
        /// </summary>
        public DataTable Tab;

        /// <summary>
        /// Конструктор собятия
        /// </summary>
        /// <param name="Message">Сообщение</param>
        /// <param name="Source">Источник</param>
        /// <param name="Evn">Тип собятия</param>
        /// <param name="Tab">Передаём таблицу которая может иметь результат для основного окна и потенциально писать сообщения</param>
        /// <param name="isLog">Писать в лог</param>
        /// <param name="Show">Показывать пользователю</param>
        public EventLog(string Message, string Source, EventEn Evn, DataTable Tab, bool isLog, bool Show) : this(Message, Source, Evn, isLog, Show)
        {
            try
            {
                this.Tab = Tab;
            }
            catch (Exception ex)
            {
                ApplicationException ae = new ApplicationException(string.Format("Упали при инициализации конструктора с ошибкой: ({0})", ex.Message));
                Log.EventSave(ae.Message, string.Format("{0}.EventLog", this.GetType().FullName), EventEn.Error);
                throw ae;
            }
        }
        //
        /// <summary>
        /// Конструктор собятия
        /// </summary>
        /// <param name="Message">Сообщение</param>
        /// <param name="Source">Источник</param>
        /// <param name="Evn">Тип собятия</param>
        /// <param name="isLog">Писать в лог</param>
        /// <param name="Show">Показывать пользователю</param>
        public EventLog(string Message, string Source, EventEn Evn, bool isLog, bool Show)
        {
            try
            {
                this.Message = Message;
                this.Source = Source;
                this.Evn = Evn;
                this.isLog = isLog;
                this.Show = Show;
            }
            catch (Exception ex)
            {
                ApplicationException ae = new ApplicationException(string.Format("Упали при инициализации конструктора с ошибкой: ({0})", ex.Message));
                Log.EventSave(ae.Message, string.Format("{0}.EventLog", this.GetType().FullName), EventEn.Error);
                throw ae;
            }
        }

    }
}
