using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.IoPlg
{
    /// <summary>
    /// Интерфейс который должны реализовать все плагины которые мы будем видеть как пулы плагинов в интерфейсе
    /// </summary>
    public interface IoListI
    {
        /// <summary>
        /// Событие установки статуса пула для отслеживания зависаний и качества соединения с разными источниками относительно нод
        /// </summary>
        /// <param name="CurTaskMonitoring">Задачи предназначенные для нашего пула которые необходимо будет выполнять в режиме мониторинг</param>
        /// <returns>Возвращаем статус проверки</returns>
        EventEn SetStatusPul(List<IoTask> CurTaskMonitoring);
    }
}

