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
        public Int64 ClassVersion { get; private set; } = 0;

        /// <summary>
        /// Класс плагина для которого предназначается это задание. (аналог метода который вызывается в классе)
        /// </summary>
        public string PluginClass;

        /// <summary>
        /// Тип задания для того чтобы работы логики в плагинах
        /// </summary>
        public IoTaskProcessTypEn TaskProcessTyp;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="PluginClass">Класс плагина для которого предназначается это задание. (аналог метода который вызывается в классе)</param>
        /// <param name="TaskProcessTyp">Тип задания для того чтобы работы логики в плагинах</param>
        public IoTaskFilter( string PluginClass, IoTaskProcessTypEn TaskProcessTyp)
        {
            try
            {
                this.GuidTask = Guid.NewGuid();
                this.DomainName = Environment.UserDomainName;
                this.HostName = Environment.MachineName;
                this.UserName = Environment.UserName;
                this.StatusLoockMachine = ProgramStatus.HashLock;
                this.ClassVersion = 0;
                this.PluginClass = PluginClass;
                this.TaskProcessTyp = TaskProcessTyp;
            }
            catch (Exception ex)
            {
                ApplicationException ae = new ApplicationException(string.Format("Упали при инициализации конструктора с ошибкой: ({0})", ex.Message));
                Log.EventSave(ae.Message, string.Format("{0}.IoTaskFilter", this.GetType().FullName), EventEn.Error);
                throw ae;
            }
        }
        //
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="ClassVersion">Минимальная версия ноды меньше ктоорой нельзя брать задания</param>
        /// <param name="PluginClass">Класс плагина для которого предназначается это задание. (аналог метода который вызывается в классе)</param>
        /// <param name="TaskProcessTyp">Тип задания для того чтобы работы логики в плагинах</param>
        public IoTaskFilter(int ClassVersion, string PluginClass, IoTaskProcessTypEn TaskProcessTyp) :this(PluginClass, TaskProcessTyp)
        {
            try
            {
                this.ClassVersion = ClassVersion;
            }
            catch (Exception ex)
            {
                ApplicationException ae = new ApplicationException(string.Format("Упали при инициализации конструктора с ошибкой: ({0})", ex.Message));
                Log.EventSave(ae.Message, string.Format("{0}.IoTaskFilter", this.GetType().FullName), EventEn.Error);
                throw ae;
            }
        }
        //
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="ClassVersion">Минимальная версия ноды меньше ктоорой нельзя брать задания</param>
        /// <param name="PluginClass">Класс плагина для которого предназначается это задание. (аналог метода который вызывается в классе)</param>
        /// <param name="TaskProcessTyp">Тип задания для того чтобы работы логики в плагинах</param>
        public IoTaskFilter(string ClassVersion, string PluginClass, IoTaskProcessTypEn TaskProcessTyp) : this(PluginClass, TaskProcessTyp)
        {
            try
            {
                string[] ver = ClassVersion.Split('.');
                int verrez = 0;

                for (int i = 0; i < ver.Length; i++)
                {
                    if (i == ver.Length - 1) verrez += int.Parse(ver[i]);
                    else verrez += int.Parse(ver[i]) * (int)(Math.Pow(1000, ver.Length - i - 1));
                }

                this.ClassVersion = verrez;
            }
            catch (Exception ex)
            {
                ApplicationException ae = new ApplicationException(string.Format("Упали при инициализации конструктора с ошибкой: ({0})", ex.Message));
                Log.EventSave(ae.Message, string.Format("{0}.IoTaskFilter", this.GetType().FullName), EventEn.Error);
                throw ae;
            }
        }

    }
}
