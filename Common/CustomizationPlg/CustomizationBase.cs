using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.CustomizationPlg
{
    /// <summary>
    /// Абстрактный класс для кастомизации
    /// </summary>
    public abstract class CustomizationBase
    {
        /// <summary>
        /// Тип палгина
        /// </summary>
        public string PlugInType { get; private set; }

        /// <summary>
        /// Возвращает версию плагина
        /// </summary>
        public string VersionPlg { get; private set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="PlugInType">Тип палгина - this.GetType().FullName || Assembly.GetExecutingAssembly().FullName</param>
        /// <param name="VersionPlg">Версия плагина - Assembly.GetExecutingAssembly().GetName().Version.ToString()</param>
        public CustomizationBase(string PlugInType, string VersionPlg)
        {
            try
            {
                if (PlugInType.IndexOf(",") > 0)
                {
                    this.PlugInType = PlugInType.Substring(0, PlugInType.IndexOf(","));
                }
                else this.PlugInType = PlugInType;
                this.VersionPlg = VersionPlg;

                Log.EventSave(string.Format("Загружен плагин кастомизации {0} ({1})", this.PlugInType, this.VersionPlg), this.GetType().FullName, EventEn.Message);
            }
            catch (Exception ex)
            {
                ApplicationException ae = new ApplicationException(string.Format("Упали при инициализации конструктора с ошибкой: ({0})", ex.Message));
                Log.EventSave(ae.Message, string.Format("{0}.CustomizationBase", this.GetType().FullName), EventEn.Error);
                throw ae;
            }

        }

        /// <summary>
        /// Метод для записи информации в лог
        /// </summary>
        /// <param name="Message">Сообщение</param>
        /// <param name="Source">Источник</param>
        /// <param name="evn">Тип события</param>
        protected void EventSave(string Message, string Source, EventEn evn)
        {
            try
            {
                Log.EventSave(Message, string.Format("({0}).{1}", this.PlugInType, Source), evn, true, false);
            }
            catch (Exception ex)
            {
                ApplicationException ae = new ApplicationException(string.Format("Упали при обработке сообщения для лога с ошибкой: ({0})", ex.Message));
                Log.EventSave(ae.Message, string.Format("{0}.EventSave", this.GetType().FullName), EventEn.Error);
                throw ae;
            }

        }
    }
}
