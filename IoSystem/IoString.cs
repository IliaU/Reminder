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
    /// Переменная типа string
    /// </summary>
    public class IoString : Param, Common.ParamPlg.ParamTransferSqlI
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="PlugInType">Тип палгина - this.GetType().FullName</param>
        /// <param name="VersionPlg">Версия - плагина</param>
        public IoString() : base(Assembly.GetExecutingAssembly().FullName, Assembly.GetExecutingAssembly().GetName().Version.ToString())
        {
            try
            {
                //Log.EventSave("Плагин лугойла", "dd", EventEn.Message);
                //this.EventSave("Плагин лугойла yfcktljdfyysq", "dd", EventEn.Message);

            }
            catch (Exception ex)
            {
                ApplicationException ae = new ApplicationException(string.Format("Упали при инициализации конструктора с ошибкой: ({0})", ex.Message));
                this.EventSave(ae.Message, string.Format("{0}.IoString", this.GetType().FullName), EventEn.Error);
                throw ae;
            }
        }
    }
}
