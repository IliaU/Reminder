using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Common.ParamPlg;

namespace Common
{
    /// <summary>
    /// Класс для реализации конкретного парметра
    /// </summary>
    public class Param : ParamBase
    {
        /// <summary>
        /// Имя параметра
        /// </summary>
        public string ParamName;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="PlugInType">Тип палгина - this.GetType().FullName || Assembly.GetExecutingAssembly().FullName</param>
        /// <param name="VersionPlg">Версия плагина - Assembly.GetExecutingAssembly().GetName().Version.ToString()</param>
        public Param(string PlugInType, string VersionPlg) : base(PlugInType, VersionPlg)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(PlugInType)) throw new ApplicationException("Данный клас является затычкой вам необходимо создать свой класс организовав наследование :Param, ParamTransfer%I");
            }
            catch (Exception ex)
            {
                ApplicationException ae = new ApplicationException(string.Format("Упали при инициализации конструктора с ошибкой: ({0})", ex.Message));
                Log.EventSave(ae.Message, string.Format("{0}.Param", this.GetType().FullName), EventEn.Error);
                throw ae;
            }
        }


        /// <summary>
        /// Вложенный список
        /// </summary>
        public ParamList LstParam;
    }
}
