using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.IoPlg
{
    /// <summary>
    /// Класс для обработки события удаления инстанса
    /// </summary>
    public class EventIoDelete : EventArgs
    {
        /// <summary>
        /// Удаляемый объект объект
        /// </summary>
        public Io dIo { get; private set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="dIo">Удаляемый объект</param>
        public EventIoDelete(Io dIo)
        {
            try
            {
                this.dIo = dIo;
            }
            catch (Exception ex)
            {
                ApplicationException ae = new ApplicationException(string.Format("Упали при инициализации конструктора с ошибкой: ({0})", ex.Message));
                Log.EventSave(ae.Message, string.Format("{0}.EventIoDelete", this.GetType().FullName), EventEn.Error);
                throw ae;
            }
        }
    }
}
