using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.ParamPlg
{
    /// <summary>
    /// Класс для обработки события удаления инстанса
    /// </summary>
    public class EventParamDelete
    {
        /// <summary>
        /// Удаляемый объект объект
        /// </summary>
        public Param dParam { get; private set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="dParam">Удаляемый объект</param>
        public EventParamDelete(Param dParam)
        {
            try
            {
                this.dParam = dParam;
            }
            catch (Exception ex)
            {
                ApplicationException ae = new ApplicationException(string.Format("Упали при инициализации конструктора с ошибкой: ({0})", ex.Message));
                Log.EventSave(ae.Message, string.Format("{0}.EventParamDelete", this.GetType().FullName), EventEn.Error);
                throw ae;
            }
        }
    }
}
