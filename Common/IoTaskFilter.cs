using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    /// <summary>
    /// Класс для фильтрации заданий и передаваемый как параметр для получения необходимого списка заданий для выполнения
    /// </summary>
    public class IoTaskFilter
    {
        /// <summary>
        /// Идентификатор задания который получит база чтобы зафиксировать задание за этой нодой
        /// </summary>
        public Guid GuidTask;

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
        public int ClassVersion { get; private set; } = 0;

        /// <summary>
        /// Класс плагина для которого предназначается это задание. (аналог метода который вызывается в классе)
        /// </summary>
        public string PluginClass;
    }
}
