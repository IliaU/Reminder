using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.IoPlg
{
    /// <summary>
    /// Класс для обработки события добавления инстанса
    /// </summary>
    public class EventIoAdd : EventArgs
    {
        /// <summary>
        /// Добавляемый объект
        /// </summary>
        public Io nIo { get; private set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="nIo">Добавляемый объект</param>
        public EventIoAdd(Io nIo)
        {
            try
            {
                this.nIo = nIo;
            }
            catch (Exception ex)
            {
                ApplicationException ae = new ApplicationException(string.Format("Упали при инициализации конструктора с ошибкой: ({0})", ex.Message));
                Log.EventSave(ae.Message, string.Format("{0}.EventIoAdd", this.GetType().FullName), EventEn.Error);
                throw ae;
            }
        }
    }
}
