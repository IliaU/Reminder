using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    /// <summary>
    /// Класс представляет из себя задачу которая получена от плагина
    /// </summary>
    public class Task
    {
        /// <summary>
        /// Минимальное количество секунд через которое может задача повторяться если правила срабатывания оказались такими же как в предыдущий раз. По умолчанию 15 секунд.
        /// </summary>
        public int StartTimeOutSec { get; private set; } = 15;

        /// <summary>
        /// Приоритет срабатывания если по условиям подходит несколько правил то срабатывает то которое имеет самый маленький приоритет что считается самым первым в списке правил. По умолчанию 1000
        /// </summary>
        public int Prioritet { get; private set; } = 1000;

        /// <summary>
        /// Реализация хранилища данных
        /// </summary>
        public Segment segment { get; private set; }

        /// <summary>
        /// Фмзическое расположение ноды в дереве с разделителем \. По умолчанию % что означает любое расположение
        /// </summary>
        public string Location { get; private set; } = "%";

        /// <summary>
        /// Имя хоста на котором крутится наша нода
        /// </summary>
        public string HostName { get; private set; } = Environment.MachineName;

        /// <summary>
        /// Имя пользователя из под которого крутится наша нода
        /// </summary>
        public string UserName { get; private set; } = Environment.UserName;

        /// <summary>
        /// Имя домена для которого предназначено задание
        /// </summary>
        public string DomainName { get; private set; } = Environment.UserDomainName;

        /// <summary>
        /// Задаётся статус машины в которых разрешено запускать задание если пусто значит в любых состояниях
        /// </summary>
        public bool? StatusLoockMachine { get; private set; } = null;

        /// <summary>
        /// Минимальная версия ноды меньше ктоорой нельзя брать задания
        /// </summary>
        public int NodeVersion { get; private set; } = 0;

        /// А тут должен быть список плагинов например версия которая должна быть минимальна и статус их компонент например есть ли нужный тип базы а есть ли у этой базы подключение по какому - то имени может нет подключения и нет смысла тогда брать
        /// тут сам плагин должен понять всё у него хорошо или нет относительно той ноды на которой он живёт
    }
}
