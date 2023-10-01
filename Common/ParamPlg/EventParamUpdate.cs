using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.ParamPlg
{
    /// <summary>
    /// Класс для обработки события изменения инстанса
    /// </summary>
    public class EventParamUpdate
    {
        /// <summary>
        /// Добавляемый объект
        /// </summary>
        public Param nParam { get; private set; }

        /// <summary>
        /// Удаляемый объект объект
        /// </summary>
        public Param dParam { get; private set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="nParam">Добавляемый объект</param>
        /// <param name="dParam">Удаляемый объект</param>
        public EventParamUpdate(Param nParam, Param dParam)
        {
            try
            {
                this.nParam = nParam;
                this.dParam = dParam;
            }
            catch (Exception ex)
            {
                ApplicationException ae = new ApplicationException(string.Format("Упали при инициализации конструктора с ошибкой: ({0})", ex.Message));
                Log.EventSave(ae.Message, string.Format("{0}.EventParamUpdate", this.GetType().FullName), EventEn.Error);
                throw ae;
            }
        }
    }
}
