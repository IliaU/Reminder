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
    public class Segment
    {
        /// <summary>
        /// К какой категории относится нода. По этому параметру определяется как резервировать процесс для отказоустойчивости.
        /// </summary>
        public TaskSpace Space { get; private set; } = TaskSpace.None;

        /// <summary>
        /// Имя сегмента которое хранит данные
        /// </summary>
        public string SegmentNamee { get; private set; } = "root";
    }
}
