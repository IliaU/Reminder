using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading;
using System.Windows.Forms;
using System.Collections;
using Common.IoPlg;

namespace Common
{
    /// <summary>
    /// Класс реализующий логику работы с листом плагинов
    /// </summary>
    public class IoList : IoBase.IoListBase
    {

        #region Param (private)

        /// <summary>
        /// Поток для мониторинга состояния ноды и её здооровбя целиком
        /// </summary>
        private static Thread ThrCreateCurentPulList;

        /// <summary>
        /// Состояние мониторинга по всей ноде
        /// </summary>
        private static bool IsRunThrCreateCurentPulList = false;

        #endregion

        #region Param (public get; protected set;)

        /// <summary>
        /// Список параметров которым обладает объект
        /// </summary>
        public ParamList CurrentPurams = new ParamList();

        #endregion

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="Plg">Информация по плагину чтобы можно было найти файл откудазагрузился и путь в том числе</param>
        /// <param name="FolderName">Путь к плагину от корня для того чтобы видеть вложения</param>
        /// <param name="PluginFileName">Имя файла для того чтобы понимать из какого файла подгружены классы </param>
        public IoList(PluginClassElement Plg, string FolderName, string PluginFileName) : base(Plg, FolderName, PluginFileName)
        {
            try
            {

            }
            catch (Exception ex)
            {
                ApplicationException ae = new ApplicationException(string.Format("Упали при инициализации конструктора с ошибкой: ({0})", ex.Message));
                Log.EventSave(ae.Message, string.Format("{0}.IoList", this.GetType().FullName), EventEn.Error);
                throw ae;
            }
        }

        /// <summary>
        /// Добавление элемента в список
        /// </summary>
        /// <param name="nIo">Элемент который мы добавили в список</param>
        public override void Add(Io nIo)
        {
            try
            {
                // Если всё успешно то добавляем в список
                base.Add(nIo);
            }
            catch (Exception ex)
            {
                ApplicationException ae = new ApplicationException(string.Format(@"Ошибка в методе Add:""{0}""", ex.Message));
                Log.EventSave(ae.Message, string.Format("{0}.Add", this.GetType().FullName), EventEn.Error);
                throw ae;
            }
        }

        /// <summary>
        /// Получение элементов в главную форму для настройки глобальных свойств
        /// Данное событие обязательно. Необходимо для настройки глобальных методов которые будут доступны всем и для управления пулами на нодах их колличеством и правилами кто будет выполнять из нод задания
        /// </summary>
        /// <returns>Объект контектсного меню в главной форме, который будет видель наш пользователь</returns>
        public virtual ToolStripMenuItem GetFormMainCustSetup()
        {
            try
            {
                throw new ApplicationException("Необходимо переписать метод чтобы подставить правильный элемент меню по нашему плагину");
            }
            catch (Exception ex)
            {
                ApplicationException ae = new ApplicationException(string.Format(@"Ошибка в методе GetFormMainCustSetup:""{0}""", ex.Message));
                Log.EventSave(ae.Message, string.Format("{0}.GetFormMainCustSetup(", this.GetType().FullName), EventEn.Error);
                throw ae;
            }
        }

        /// <summary>
        /// Получение элементов в главную форму в меню самих плагинов
        /// Данный метод не обязательны нужен только для корневого эелоемента плагина. Например это может быть отдельная программа которая потом будет дёргать другие объекты и генерить собственные события и подписки в ядре
        /// </summary>
        /// <returns>Объект контектсного меню в главной форме в менюшке плагинов как рутовый объект, который будет видель наш пользователь</returns>
        public virtual List<ToolStripMenuItem> GetFormMainCust()
        {
            try
            {
                return null;
            }
            catch (Exception ex)
            {
                ApplicationException ae = new ApplicationException(string.Format(@"Ошибка в методе GetFormMainCust:""{0}""", ex.Message));
                Log.EventSave(ae.Message, string.Format("{0}.GetFormMainCust(", this.GetType().FullName), EventEn.Error);
                throw ae;
            }
        }

    }
}
