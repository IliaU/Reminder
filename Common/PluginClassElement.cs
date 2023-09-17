using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Reflection;

namespace Common
{
    /// <summary>
    /// Эелемент для ферм с плагинами для создания списков чтобы удобнее было работать
    /// </summary>
    public class PluginClassElement
    {
        /// <summary>
        /// Информация по файлу dll
        /// </summary>
        public PluginClassElementList FileInfo;

        /// <summary>
        /// Имя плагина
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Ссылка на длагин
        /// </summary>
        public Type EmptTyp { get; private set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="Name">Имя плагина</param>
        /// <param name="EmptTyp">Ссылка на длагин</param>
        /// <param name="FileInfo">Информация по файлу dll</param>
        public PluginClassElement(string Name, Type EmptTyp, PluginClassElementList FileInfo)
        {
            try
            {
                this.Name = Name;
                this.EmptTyp = EmptTyp;
                this.FileInfo = FileInfo;
            }
            catch (Exception ex)
            {
                ApplicationException ae = new ApplicationException(string.Format("Упали при инициализации конструктора с ошибкой: ({0})", ex.Message));
                Log.EventSave(ae.Message, string.Format("{0}.PluginClassElement", this.GetType().FullName), EventEn.Error);
                throw ae;
            }


        }
    }
}
