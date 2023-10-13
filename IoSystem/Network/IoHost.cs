using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Common;
using System.Reflection;

namespace IoSystem.Network
{
    /// <summary>
    /// Класс представляет из себя пулл с обектами
    /// </summary>
    public class IoHost : IoList, Common.IoPlg.IoListI
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="PlugInType">Тип палгина - this.GetType().FullName</param>
        /// <param name="VersionPlg">Версия - плагина</param>
        public IoHost() : base(Assembly.GetExecutingAssembly().FullName, Assembly.GetExecutingAssembly().GetName().Version.ToString())
        {
            try
            {
                // Подгружаем наименование нешего параметра для того чтобы плагин знал как обозвать класс
                base.ClassMethods.Add(typeof(IoHostGetNetwork).Name);
                //Log.EventSave("Плагин лугойла", "dd", EventEn.Message);
                //this.EventSave("Плагин лугойла yfcktljdfyysq", "dd", EventEn.Message);

                // Создаём параметры объекта
                Param PHostName = ParamFarm.CreateNewParam(string.Format("IoSystem.{0}", typeof(IoString).Name));
                PHostName.ParamName = "HostName";
                base.CurrentPurams.Add(PHostName);

            }
            catch (Exception ex)
            {
                ApplicationException ae = new ApplicationException(string.Format("Упали при инициализации конструктора с ошибкой: ({0})", ex.Message));
                this.EventSave(ae.Message, string.Format("{0}.IoHost", this.GetType().FullName), EventEn.Error);
                throw ae;
            }
        }

        /// <summary>
        /// Событие установки статуса пула для отслеживания зависаний и качества соединения с разными источниками относительно нод
        /// </summary>
        /// <returns>Возвращаем статус проверки</returns>
        public EventEn SetStatusPul()
        {
            try
            {
                return EventEn.Message;
            }
            catch (Exception ex)
            {
                ApplicationException ae = new ApplicationException(string.Format("Упали при инициализации конструктора с ошибкой: ({0})", ex.Message));
                this.EventSave(ae.Message, string.Format("{0}.IoMonLanList", this.GetType().FullName), EventEn.Error);
                throw ae;
            }
        }

    }
}
