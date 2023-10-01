using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Common.ProviderPlg;

namespace Common
{
    /// <summary>
    /// Класс для реализации репозитория нашей программы
    /// </summary>
    public class Provider : ProviderBase
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="PlugInType">Тип палгина - this.GetType().FullName || Assembly.GetExecutingAssembly().FullName</param>
        /// <param name="VersionPlg">Версия плагина - Assembly.GetExecutingAssembly().GetName().Version.ToString()</param>
        /// <param name="ConnectionString">Строка подключения к репозиторию</param>
        public Provider(string PlugInType, string VersionPlg, string ConnectionString) : base(PlugInType, VersionPlg, ConnectionString)
        {
            try
            {

            }
            catch (Exception ex)
            {
                ApplicationException ae = new ApplicationException(string.Format("Упали при инициализации конструктора с ошибкой: ({0})", ex.Message));
                Log.EventSave(ae.Message, string.Format("{0}.Provider", this.GetType().FullName), EventEn.Error);
                throw ae;
            }
        }
    }
}
