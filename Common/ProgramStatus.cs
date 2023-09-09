using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    /// <summary>
    /// Состояние программы на сейчас
    /// </summary>
    public class ProgramStatus
    {
        /// <summary>
        /// Состояние блокировки операционной системы
        /// </summary>
        public static bool HashLock = false;

        /// <summary>
        /// Событие показывающее статус блокировки экрана
        /// </summary>
        public static event EventHandler <EventHashLock> onEventHashLock;

        /// <summary>
        /// Активация события блокироваки или разблокироваки рабочего стола
        /// </summary>
        /// <param name="SetHashLock"></param>
        public static void onEventHashLockActivation(bool SetHashLock)
        {
            HashLock = SetHashLock;

            // Собственно обработка события
            EventHashLock myArg = new EventHashLock(SetHashLock);
            if (ProgramStatus.onEventHashLock != null)
            {
                ProgramStatus.onEventHashLock.Invoke(null, myArg);
            }

            if (SetHashLock) Log.EventSave("Компьютер заблокирован", "ProgramStatus", EventEn.Message);
            else Log.EventSave("Компьютер разблокирован", "ProgramStatus", EventEn.Message);

        }

    }
}
