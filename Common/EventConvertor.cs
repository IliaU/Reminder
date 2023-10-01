using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    /// <summary>
    /// Класс для конверсации cобытий из строк в энумератор
    /// </summary>
    public static class EventConvertor
    {
        /// <summary>
        /// Конвертация в объект eventEn
        /// </summary>
        /// <param name="EventStr">Строка которую надо конвертнуть</param>
        /// <param name="DefaultEvent">Если не можем конвертнуть что в этом случае вернуть</param>
        /// <returns></returns>
        public static EventEn Convert(string EventStr, EventEn DefaultEvent)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(EventStr))
                {
                    foreach (EventEn item in EventEn.GetValues(typeof(EventEn)))
                    {
                        if (item.ToString().ToUpper() == EventStr.Trim().ToUpper()) return item;
                    }
                }
                return DefaultEvent;
            }
            catch (Exception)
            {
                return DefaultEvent;
            }
        }

    }
}
