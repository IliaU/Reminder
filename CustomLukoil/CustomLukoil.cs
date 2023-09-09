using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Common;
using System.Reflection;
using System.Drawing;

namespace CustomLukoil
{
    /// <summary>
    /// Класс для реализации кастомизации нашей программы для компании Lukoil
    /// </summary>
    public class CustomLukoil : Common.Customization, Common.CustomizationPlg.CustomizationI
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="PlugInType">Тип палгина - this.GetType().FullName</param>
        /// <param name="VersionPlg">Версия - плагина</param>
        public CustomLukoil() : base(Assembly.GetExecutingAssembly().FullName, Assembly.GetExecutingAssembly().GetName().Version.ToString())
        {
            try
            {

                //Log.EventSave("Плагин лугойла", "dd", EventEn.Message);
                //this.EventSave("Плагин лугойла yfcktljdfyysq", "dd", EventEn.Message);
            }
            catch (Exception ex)
            {
                ApplicationException ae = new ApplicationException(string.Format("Упали при инициализации конструктора с ошибкой: ({0})", ex.Message));
                this.EventSave(ae.Message, string.Format("{0}.CustomizationBase", this.GetType().FullName), EventEn.Error);
                throw ae;
            }
        }

        /// <summary>
        /// Получение иконок для статусов в трее по событию к которому нужно получить иконку
        /// </summary>
        /// <param name="evn">Событие к которому нужно получить иконку</param>
        /// <returns>Возвращаем иконку</returns>
        public override Icon GetIconStatus(EventEn evn)
        {
            try
            {
                return Resource.Icon_Message;
            }
            catch (Exception ex)
            {
                ApplicationException ae = new ApplicationException(string.Format("Упали при получении иконки с ошибкой: ({0})", ex.Message));
                this.EventSave(ae.Message, string.Format("{0}.GetIconStatus", this.GetType().FullName), EventEn.Error);
                throw ae;
            }


        }

    }
}
