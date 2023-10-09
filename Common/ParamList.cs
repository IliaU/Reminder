using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Common.ParamPlg;

namespace Common
{
    /// <summary>
    /// Реализация списка параметров
    /// </summary>
    public sealed class ParamList : ParamBase.ParamListBase
    {
        /// <summary>
        /// Идентификатор параметров для которых предназначено задание и объекта с которым эти параметры связаны
        /// </summary>
        public Guid ObjParamListId;

        /// <summary>
        /// Имя параметра
        /// </summary>
        public string ParamName;

        /// <summary>
        /// Значение параметра которое может быть кастомным созданное в плагине пользователя
        /// </summary>
        public Param ParamValue;

        /// <summary>
        /// Параметр может содержать вложения
        /// </summary>
        public ParamList ParamListChildrens;
    }
}
