using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;

namespace Common.CustomizationPlg
{
    /// <summary>
    /// Инетерфейс для того чтобы базовый класс или любой другой мог дёргать что-то специфическое
    /// </summary>
    public interface CustomizationI
    {
        /// <summary>
        /// Получение иконок для статусов в трее по событию к которому нужно получить иконку
        /// </summary>
        /// <param name="evn">Событие к которому нужно получить иконку</param>
        /// <returns>Возвращаем иконку</returns>
        Icon GetIconStatus(EventEn evn);
    }
}
