using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    /// <summary>
    /// Тип задания для того чтобы работы логики в плагинах
    /// </summary>
    public enum IoTaskProcessTypEn
    {
        /// <summary>
        /// Задание работает в режиме монитолринг это позволяет не удалаять это задание после того как взял пул на обпработку а говорт что это задание для многих пулов
        /// </summary>
        Monitoring,

        /// <summary>
        /// Это говорит о том что это задание срабатывает на изменение флагав каком то другом объекте который указан в параметрах
        /// </summary>
        Flag,

        /// <summary>
        /// Этот режим говорит что работает задание в режиме события и нужно проверять подписку на которую подписан объект
        /// </summary>
        Event
    }
}
