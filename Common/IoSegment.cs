using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    /// <summary>
    /// Реализация хранилища данных
    /// </summary>
    public class IoSegment
    {
        /// <summary>
        /// К какой категории относится нода. По этому параметру определяется как резервировать процесс для отказоустойчивости.
        /// </summary>
        public IoTaskSpace Space { get; private set; } = IoTaskSpace.None;

        /// <summary>
        /// Имя сегмента которое хранит данные
        /// </summary>
        public string SegmentNamee { get; private set; } = "root";
    }
}
