using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    /// <summary>
    /// Класс содержит имя файла из которого подтянулись объекты
    /// </summary>
    public class PluginClassElementList
    {
        /// <summary>
        /// Путь к плагину от корня для того чтобы видеть вложения
        /// </summary>
        public string FolderName = null;

        /// <summary>
        /// Имя файла для того чтобы понимать из какого файла подгружены классы 
        /// </summary>
        public string PluginFileName = null;

        /// <summary>
        /// Список подгруженных классов
        /// </summary>
        public List<PluginClassElement> Items = new List<PluginClassElement>();

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="FolderName">Путь к плагину от корня для того чтобы видеть вложения</param>
        /// <param name="PluginFileName">Список подгруженных классов</param>
        public PluginClassElementList (string FolderName, string PluginFileName)
        {
            try
            {
                this.FolderName = FolderName;

                if (!string.IsNullOrWhiteSpace(PluginFileName) && PluginFileName.IndexOf(".") > 0) this.PluginFileName = PluginFileName.Substring(0, PluginFileName.IndexOf("."));
                this.PluginFileName = PluginFileName;
            }
            catch (Exception ex)
            {
                ApplicationException ae = new ApplicationException(string.Format("Упали при инициализации конструктора с ошибкой: ({0})", ex.Message));
                Log.EventSave(ae.Message, string.Format("{0}.PluginClassElementList", this.GetType().FullName), EventEn.Error);
                throw ae;
            }
        }

        /// <summary>
        /// Поиск класса по имени
        /// </summary>
        /// <param name="Find">Строка по которой ищем можно либо полный путь FolderName.PluginFileName.class</param>
        /// <returns></returns>
        public PluginClassElement GetPlgForName(string Find)
        {
            try
            {
                if (Find.IndexOf(".")>0)
                {

                }
                else
                {
                    foreach (PluginClassElement item in Items)
                    {
                        if (item.Name == Find.Trim()) return item;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                ApplicationException ae = new ApplicationException(string.Format("Упали при получении иконки с ошибкой: ({0})", ex.Message));
                Log.EventSave(ae.Message, string.Format("{0}.GetTepyPlgForName", this.GetType().FullName), EventEn.Error);
                throw ae;
            }
        }
    }
}
