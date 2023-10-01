using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.ParamPlg
{
    /// <summary>
    /// Класс для реализации конкретного парметра
    /// </summary>
    public abstract partial class ParamBase
    {
        #region Param (private)

        /// <summary>
        /// Интерфейс для базового класса чтобы он мог дёргать скрытыем методы
        /// </summary>
        private ParamTransferSqlI InterfIo = null;

        /// <summary>
        /// Тип палгина
        /// </summary>
        public string PlugInType { get; private set; } = null;

        /// <summary>
        /// Возвращает версию плагина
        /// </summary>
        public string VersionPlg { get; private set; }

        #endregion

        #region Param (public get; protected set;)

        /// <summary>
        /// Индекс в списке IoList
        /// </summary>
        public int index { get; protected set; } = -1;

        /// <summary>
        /// Информация по файлу
        /// </summary>
        public PluginClassElementList FileInfo;

        /// <summary>
        /// Информация по классу
        /// </summary>
        public PluginClassElement ElementDll;

        /// <summary>
        /// Доставка параметров к получателям которые подписаны через базу данных. 
        /// Сначала мы обращаемся к базе данных получаем задание с параметрами и получаем список получателей
        /// Выполняем задачу и результат сразу складываем в базу 
        /// (Способ позволянет гарантировать полный результат так как получатель уже определён и половинчатый результат не возможен)
        /// </summary>
        public ParamTransferSqlI TransferSql { get; protected set; } = null;

        /// <summary>
        /// Доставка параметров к элементам которые подписаны через механизм проверки флагов в базе данных
        /// В базу записывается флаг об изменении параметра. Это позволяет избежать постоянных промежуточных вызовов маленькими порциями 
        /// но тогда класс должен обеспечить вывод всех данных относительно заданного временного периода за  указанную глубину в конфиге 
        /// так как класс получивший события по флагу может быть не один и у каждого может потребоваться свой набор данных
        /// получается подписанты сами опрашивают по мере необходимости
        /// </summary>
        public ParamTransferFlagI TransferFlag { get; protected set; } = null;

        /// <summary>
        /// Доставка параметров к элементам которые подписаны через систему подписок
        /// в этом варианте класс после выполнения задачи проверяет кто на него подписан и передаёт информацию напрямую подписанту без базы данных
        /// Это позволяет базу данных не нагружать не сильно нужными данными и организовать скорость но для атомарности процессов надо чтобы инфа по доставке была сохранениа в базе чтобы можно было повторить операцию
        /// </summary>
        public ParamTransferEventI TransferEvent { get; protected set; } = null;
        #endregion

        #region Method (public)

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="PlugInType">Тип палгина - this.GetType().FullName || Assembly.GetExecutingAssembly().FullName</param>
        /// <param name="VersionPlg">Версия плагина - Assembly.GetExecutingAssembly().GetName().Version.ToString()</param>
        public ParamBase(string PlugInType, string VersionPlg)
        {
            try
            {
                if (PlugInType.IndexOf(",") > 0)
                {
                    this.PlugInType = PlugInType.Substring(0, PlugInType.IndexOf(","));
                }
                else this.PlugInType = PlugInType;
                this.VersionPlg = VersionPlg;
            }
            catch (Exception ex)
            {
                ApplicationException ae = new ApplicationException(string.Format("Упали при инициализации конструктора с ошибкой: ({0})", ex.Message));
                Log.EventSave(ae.Message, string.Format("{0}.ParamBase", this.GetType().FullName), EventEn.Error);
                throw ae;
            }
        }

        #endregion


        #region Method (public virtual)


        #endregion

        #region Method (protected)

        /// <summary>
        /// Метод для записи информации в лог
        /// </summary>
        /// <param name="Message">Сообщение</param>
        /// <param name="Source">Источник</param>
        /// <param name="evn">Тип события</param>
        /// <param name="IsLog">Писать в лог или нет</param>
        /// <param name="Show">Отобразить сообщение пользователю или нет</param>
        protected void EventSave(string Message, string Source, EventEn evn, bool IsLog, bool Show)
        {
            try
            {
                Log.EventSave(Message, string.Format("({0}).{1}", this.PlugInType, Source), evn, IsLog, Show);
            }
            catch (Exception ex)
            {
                ApplicationException ae = new ApplicationException(string.Format("Упали при обработке сообщения для лога с ошибкой: ({0})", ex.Message));
                Log.EventSave(ae.Message, string.Format("{0}.EventSave", this.GetType().FullName), EventEn.Error);
                throw ae;
            }
        }

        /// <summary>
        /// Событие программы
        /// </summary>
        /// <param name="Message">Сообщение</param>
        /// <param name="Source">Источник</param>
        /// <param name="evn">Тип события</param>
        /// <param name="IsLog">Писать в лог или нет</param>
        /// <param name="Show">Отобразить сообщение пользователю или нет</param>
        protected void EventSave(string Message, string Source, EventEn evn)
        {
            try
            {
                EventSave(Message, Source, evn, true, false);
            }
            catch (Exception ex)
            {
                ApplicationException ae = new ApplicationException(string.Format("Упали при обработке сообщения для лога с ошибкой: ({0})", ex.Message));
                Log.EventSave(ae.Message, string.Format("{0}.EventSave", this.GetType().FullName), EventEn.Error);
                throw ae;
            }
        }


        #endregion
    }
}
