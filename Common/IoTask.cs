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
    public class IoTask
    {
        /// <summary>
        /// Идентификатор параметров для которых предназначено задание и объекта с которым эти параметры связаны
        /// </summary>
        public Guid ObjParamListId;

        /// <summary>
        /// Класс плагина для которого предназначается это задание. (аналог метода который вызывается в классе)
        /// </summary>
        public string PluginClass;

        /// <summary>
        /// Фмзическое расположение ноды в дереве с разделителем \. По умолчанию % что означает любое расположение
        /// </summary>
        public string Location { get; private set; } = "%";

        /// <summary>
        /// Идентификатор параметров которые надо подать на вход и на который мы подписаны. По нему мы должны понять откуда получать параметры (например сожно десериализовать из папки либо из базы в зависимости от типа параметра, возможно напрямую из источника обращаясь на определённый хост ноды)
        /// </summary>
        public Guid ObjInParamListId;

        /// <summary>
        /// Класс метода который хотим вызвать в нашем классе
        /// </summary>
        public string PluginClassMethod;

        /// <summary>
        /// Имя домена для которого предназначено задание
        /// </summary>
        public string DomainName { get; private set; } = Environment.UserDomainName;

        /// <summary>
        /// Имя хоста для которой предназначена задача
        /// </summary>
        public string HostName { get; private set; } = Environment.MachineName;

        /// <summary>
        /// Имя пользователя для которого предназначено задание
        /// </summary>
        public string UserName { get; private set; } = Environment.UserName;

        /// <summary>
        /// Задаётся статус машины в которых разрешено запускать задание если пусто значит в любых состояниях
        /// </summary>
        public bool? StatusLoockMachine { get; private set; } = null;

        /// <summary>
        /// Минимальная версия ноды меньше ктоорой нельзя брать задания
        /// </summary>
        public int ClassVersion { get; private set; } = 1000000000;

        /// <summary>
        /// Минимальное количество секунд через которое может задача повторятся если правила срабатывания оказались такими же как в предыдущий раз. По умолчанию 15 секунд.
        /// </summary>
        public int StartTimeOutSec { get; private set; } = 15;

        /// <summary>
        /// Реализация хранилища данных
        /// </summary>
        public IoSegment segment { get; private set; }

        /// <summary>
        /// Тип задания для того чтобы работы логики в плагинах
        /// </summary>
        public IoTaskProcessTypEn TaskProcessTyp;

        /// <summary>
        /// Количество повторов в случае сбоя на других нодах (При условии что  все оберации атомарны)
        /// </summary>
        public int ErrorRetryCount { get; private set; } = 3;

        /// <summary>
        /// Таймаут в секундах заданный к повторному повторению в случае сбоя (При условии что  все оберации атомарны)
        /// </summary>
        public int ErrorRetryTimeOutSec { get; private set; } = 30;

        /// <summary>
        /// Количество секунд после которого считать что задание свалилось если небыло ответа от выполняющей ноды
        /// </summary>
        public int ReflexionTimeOutSec { get; private set; } = 3600;

        /// <summary>
        /// Приоритет срабатывания если по условиям подходит несколько правил то срабатывает то которое имеет самый маленький приоритет что считается самым первым в списке правил. По умолчанию 1000
        /// </summary>
        public int Prioritet { get; private set; } = 1000;

        /// <summary>
        /// Идентификатор задания который получит база чтобы зафиксировать задание за этой нодой
        /// </summary>
        public Guid GuidTask;

        /// А тут должен быть список плагинов например версия которая должна быть минимальна и статус их компонент например есть ли нужный тип базы а есть ли у этой базы подключение по какому - то имени может нет подключения и нет смысла тогда брать
        /// тут сам плагин должен понять всё у него хорошо или нет относительно той ноды на которой он живёт
    }
}
