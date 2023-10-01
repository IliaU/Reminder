using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.ParamPlg
{
    /// <summary>
    /// Класс для обработки события добавления инстанса
    /// </summary>
    public class EventParamAdd
    {
        /// <summary>
        /// Добавляемый объект
        /// </summary>
        public Param nParam { get; private set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="nParam">Добавляемый объект</param>
        public EventParamAdd(Param nParam)
        {
            try
            {
                this.nParam = nParam;
            }
            catch (Exception ex)
            {
                ApplicationException ae = new ApplicationException(string.Format("Упали при инициализации конструктора с ошибкой: ({0})", ex.Message));
                Log.EventSave(ae.Message, string.Format("{0}.EventParamAdd", this.GetType().FullName), EventEn.Error);
                throw ae;
            }
        }
    }
}
