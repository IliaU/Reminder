using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading;
using Common.IoPlg;

namespace Common
{
    /// <summary>
    /// Класс реализующий логику работы с листом плагинов
    /// </summary>
    public class IoList : IoBase.IoListBase
    {

        /// <summary>
        /// Поток для мониторинга состояния ноды и её здооровбя целиком
        /// </summary>
        private static Thread ThrCreateCurentPulList;

        /// <summary>
        /// Состояние мониторинга по всей ноде
        /// </summary>
        private static bool IsRunThrCreateCurentPulList = false;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="FolderName">Путь к плагину от корня для того чтобы видеть вложения</param>
        /// <param name="PluginFileName">Имя файла для того чтобы понимать из какого файла подгружены классы </param>
        public IoList(string FolderName, string PluginFileName) : base(FolderName, PluginFileName)
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


        
    }
}
