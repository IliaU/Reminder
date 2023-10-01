using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    /// <summary>
    /// Для событий обработки изменений провайдера в текущей ферме
    /// </summary>
    public class EventProviderFarm
    {
        /// <summary>
        /// Репозиторий
        /// </summary>
        public Provider Prv { get; private set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="Prv">Кастомный провайдер</param>
        public EventProviderFarm(Provider Prv)
        {
            this.Prv = Prv;
        }
    }
}
