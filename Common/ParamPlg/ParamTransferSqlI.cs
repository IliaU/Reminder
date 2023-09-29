using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.ParamPlg
{
    /// <summary>
    /// Доставка параметров к получателям которые подписаны через базу данных. 
    /// Сначала мы обращаемся к базе данных получаем задание с параметрами и получаем список получателей
    /// Выполняем задачу и результат сразу складываем в базу 
    /// (Способ позволянет гарантировать полный результат так как получатель уже определён и половинчатый результат не возможен)
    /// </summary>
    public interface ParamTransferSqlI
    {
    }
}
