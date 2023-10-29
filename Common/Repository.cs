using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Common.RepositoryPlg;

namespace Common
{
    /// <summary>
    /// Класс для реализации репозитория нашей программы
    /// </summary>
    public class Repository : RepositoryBase
    {

        /// <summary>
        /// Получить интерфейс созданный плагином для репозиториев
        /// </summary>
        /// <returns>Интерфейс созданный для весех репозиториев</returns>
        public RepositoryI GetRepI
        {
            get 
            {
                try
                {
                    return (RepositoryI)this;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

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
