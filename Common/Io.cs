using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Common.IoPlg;

namespace Common
{
    /// <summary>
    /// Плагин который используется по нашему назначению
    /// </summary>
    public class Io : IoBase
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="PlugInType">Тип палгина - this.GetType().FullName || Assembly.GetExecutingAssembly().FullName</param>
        /// <param name="VersionPlg">Версия плагина - Assembly.GetExecutingAssembly().GetName().Version.ToString()</param>
        public Io(string PlugInType, string VersionPlg) : base(PlugInType, VersionPlg)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(PlugInType)) throw new ApplicationException("Данный клас является затычкой вам необходимо создать свой класс организовав наследование :Io, IoI");
            }
            catch (Exception ex)
            {
                ApplicationException ae = new ApplicationException(string.Format("Упали при инициализации конструктора с ошибкой: ({0})", ex.Message));
                Log.EventSave(ae.Message, string.Format("{0}.Io", this.GetType().FullName), EventEn.Error);
                throw ae;
            }
        }

    }
}
