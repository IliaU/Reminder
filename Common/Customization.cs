using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Reflection;
using System.Drawing;

namespace Common
{
    /// <summary>
    /// Класс для реализации кастомизации нашей программы
    /// </summary>
    public class Customization : CustomizationPlg.CustomizationBase, CustomizationPlg.CustomizationI
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="PlugInType">Тип палгина - this.GetType().FullName || Assembly.GetExecutingAssembly().FullName</param>
        /// <param name="VersionPlg">Версия плагина - Assembly.GetExecutingAssembly().GetName().Version.ToString()</param>
        public Customization(string PlugInType, string VersionPlg) : base(PlugInType, VersionPlg)
        {
            try
            {

            }
            catch(Exception ex)
            {
                ApplicationException ae = new ApplicationException(string.Format("Упали при инициализации конструктора с ошибкой: ({0})", ex.Message));
                Log.EventSave(ae.Message, string.Format("{0}.Customization", this.GetType().FullName), EventEn.Error);
                throw ae;
            }
        }

        /// <summary>
        /// Получение иконок для статусов в трее по событию к которому нужно получить иконку
        /// </summary>
        /// <param name="evn">Событие к которому нужно получить иконку</param>
        /// <returns>Возвращаем иконку</returns>
        public virtual Icon GetIconStatus(EventEn evn) 
        {
            try
            {
                return Resource.Icon_Message;
            }
            catch (Exception ex)
            {
                ApplicationException ae = new ApplicationException(string.Format("Упали при получении иконки с ошибкой: ({0})", ex.Message));
                Log.EventSave(ae.Message, string.Format("{0}.GetIconStatus", this.GetType().FullName), EventEn.Error);
                throw ae;
            }
        }

        /// <summary>
        /// Получение иконок для статуса базы данных
        /// </summary>
        /// <param name="Connected">Если статус базы данных доступна</param>
        /// <returns>Возвращаем иконку</returns>
        public virtual Bitmap GetIconDbStatus(bool Connected)
        {
            try
            {
                if (Connected) return Resource.REP_Online;
                else return Resource.REP_Ofline;
            }
            catch (Exception ex)
            {
                ApplicationException ae = new ApplicationException(string.Format("Упали при получении рисунка для статуса базы данных: ({0})", ex.Message));
                Log.EventSave(ae.Message, string.Format("{0}.GetIconDbStatus", this.GetType().FullName), EventEn.Error);
                throw ae;
            }
        }

    }
}
