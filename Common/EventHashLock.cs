using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class EventHashLock : EventArgs
    {
        /// <summary>
        /// Состояние блокировки операционной системы
        /// </summary>
        public bool HashLock = false;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="HashLock">Состояние блокировки операционной системы</param>
        public EventHashLock (bool HashLock)
        {
            try
            {
                this.HashLock = HashLock;
            }
            catch (Exception ex)
            {
                ApplicationException ae = new ApplicationException(string.Format("Упали при инициализации конструктора с ошибкой: ({0})", ex.Message));
                Log.EventSave(ae.Message, string.Format("{0}.EventHashLock", this.GetType().FullName), EventEn.Error);
                throw ae;
            }
        }
    }
}
