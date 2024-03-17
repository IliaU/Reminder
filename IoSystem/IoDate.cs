using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Common;
using System.Reflection;

namespace IoSystem
{
    /// <summary>
    /// Переменная типа IoDate
    /// </summary>
    public class IoDate : Param, Common.ParamPlg.ParamTransferSqlI
    {
        /// <summary>
        /// Значение параметра
        /// </summary>
        public DateTime ParamValue;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="PlugInType">Тип палгина - this.GetType().FullName</param>
        /// <param name="VersionPlg">Версия - плагина</param>
        public IoDate() : base(Assembly.GetExecutingAssembly().FullName, Assembly.GetExecutingAssembly().GetName().Version.ToString())
        {
            try
            {
                //Log.EventSave("Плагин лугойла", "dd", EventEn.Message);
                //this.EventSave("Плагин лугойла yfcktljdfyysq", "dd", EventEn.Message);
            }
            catch (Exception ex)
            {
                ApplicationException ae = new ApplicationException(string.Format("Упали при инициализации конструктора с ошибкой: ({0})", ex.Message));
                this.EventSave(ae.Message, string.Format("{0}.IoDate", this.GetType().FullName), EventEn.Error);
                throw ae;
            }
        }

        /// <summary>
        /// Присвоение параметру значения
        /// </summary>
        /// <param name="par">Значение которое надо присвоить</param>
        public override void SetParam(object par)
        {
            try
            {
                this.ParamValue = ((DateTime)par).Date;
            }
            catch (Exception ex)
            {
                // Логируем ошибку если её должен видеть пользователь или если взведён флаг трассировке в файле настройки программы
                ApplicationException xe = new ApplicationException(string.Format(@"Ошибка при сохранении параметра:""{0}""", ex.Message));
                if (Config.Trace) base.EventSave(xe.Message, string.Format("{0}.SetParam", this.GetType().FullName), EventEn.Error, true, false);
                throw xe;
            }
        }

        /// <summary>
        /// Получение параметра
        /// </summary>
        /// <returns>Возвращаем объект представляющий наш параметр</returns>
        public override object GetParam()
        {
            try
            {
                return this.ParamValue;
            }
            catch (Exception ex)
            {
                // Логируем ошибку если её должен видеть пользователь или если взведён флаг трассировке в файле настройки программы
                ApplicationException xe = new ApplicationException(string.Format(@"Ошибка при чтении параметра:""{0}""", ex.Message));
                if (Config.Trace) base.EventSave(xe.Message, string.Format("{0}.GetParam", this.GetType().FullName), EventEn.Error, true, false);
                throw xe;
            }
        }
    }
}
