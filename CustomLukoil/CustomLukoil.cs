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
    public class CustomLukoil : Customization, Common.CustomizationPlg.CustomizationI
    {
        /// <summary>
        /// Храним последнее состояние значка и его значение по умолчанию
        /// </summary>
        private static Icon CurIcon = Resource.Icon_S;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="PlugInType">Тип палгина - this.GetType().FullName</param>
        /// <param name="VersionPlg">Версия - плагина</param>
        public CustomLukoil() : base(Assembly.GetExecutingAssembly().FullName, Assembly.GetExecutingAssembly().GetName().Version.ToString())
        {
            try
            {
                base.FStartText = @"Напоминалка для ЕЦМ ООО ""ЛУКОЙЛ-Технологии""";

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
                switch (evn)
                {
                    case EventEn.Success:
                        CurIcon = Resource.Icon_S;
                        break;
                    case EventEn.Warning:
                        CurIcon = Resource.Icon_W;
                        break;
                    case EventEn.Error:
                        CurIcon = Resource.Icon_E;
                        break;
                    case EventEn.FatalError:
                        CurIcon = Resource.Icon_FE;
                        break;
                    case EventEn.Message:
                    case EventEn.Empty:
                    case EventEn.Dump:
                    case EventEn.Trace:
                    default:
                        break;
                }

                return CurIcon;
            }
            catch (Exception ex)
            {
                ApplicationException ae = new ApplicationException(string.Format("Упали при получении иконки с ошибкой: ({0})", ex.Message));
                this.EventSave(ae.Message, string.Format("{0}.GetIconStatus", this.GetType().FullName), EventEn.Error);
                throw ae;
            }
        }

        /// <summary>
        /// Получение иконок для статуса базы данных
        /// </summary>
        /// <param name="Connected">Если статус базы данных доступна</param>
        /// <returns>Возвращаем иконку</returns>
        public override Bitmap GetIconDbStatus(bool Connected)
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
