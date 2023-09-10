using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    /// <summary>
    /// Класс для реализации репозитория нашей программы
    /// </summary>
    public class Repository : RepositoryPlg.RepositoryBase, RepositoryPlg.RepositoryI
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="PlugInType">Тип палгина - this.GetType().FullName || Assembly.GetExecutingAssembly().FullName</param>
        /// <param name="VersionPlg">Версия плагина - Assembly.GetExecutingAssembly().GetName().Version.ToString()</param>
        /// <param name="ConnectionString">Строка подключения к репозиторию</param>
        public Repository(string PlugInType, string VersionPlg, string ConnectionString) : base(PlugInType, VersionPlg, ConnectionString)
        {
            try
            {

            }
            catch (Exception ex)
            {
                ApplicationException ae = new ApplicationException(string.Format("Упали при инициализации конструктора с ошибкой: ({0})", ex.Message));
                Log.EventSave(ae.Message, string.Format("{0}.Repository", this.GetType().FullName), EventEn.Error);
                throw ae;
            }
        }
    }
}
