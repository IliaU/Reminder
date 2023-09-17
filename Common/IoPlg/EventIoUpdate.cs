using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.IoPlg
{
    /// <summary>
    /// Класс для обработки события изменения инстанса
    /// </summary>
    public class EventIoUpdate : EventArgs
    {
        /// <summary>
        /// Добавляемый объект
        /// </summary>
        public Io nIo { get; private set; }

        /// <summary>
        /// Удаляемый объект объект
        /// </summary>
        public Io dIo { get; private set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="nIo">Добавляемый объект</param>
        /// <param name="dIo">Удаляемый объект</param>
        public EventIoUpdate(Io nIo, Io dIo)
        {
            try
            {
                this.nIo = nIo;
                this.dIo = dIo;
            }
            catch (Exception ex)
            {
                ApplicationException ae = new ApplicationException(string.Format("Упали при инициализации конструктора с ошибкой: ({0})", ex.Message));
                Log.EventSave(ae.Message, string.Format("{0}.EventIoUpdate", this.GetType().FullName), EventEn.Error);
                throw ae;
            }
        }
    }
}
