using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Reflection;
using System.Drawing;

namespace Common
{
    /// <summary>
    /// Класс для реализации события смены текущего статуса. Срабатывает и передаёт иконку на которую надо изменить значение в формах
    /// </summary>
    public class EventChangeStatus : EventArgs
    {
        /// <summary>
        /// Тип события которое произошло на уровне всей системы
        /// </summary>
        public EventEn evn { get; private set; } = EventEn.Empty;

        /// <summary>
        /// Сообщение которое нужно в трее сделать всплывающим
        /// </summary>
        public string Message { get; private set; }

        /// <summary>
        /// Флаг который говорит о том что необходимо обработать это событие
        /// </summary>
        public bool EventEnable=true;

        /// <summary>
        /// Возвращаем иконку чтобы не вызывать и не искать иконку в текущей кастомизации
        /// </summary>
        public Icon NewIcon
        { 
            get
            {
                if (CustomizationFarm.CurCustomization != null)
                { 
                    return CustomizationFarm.CurCustomization.GetIconStatus(this.evn);
                }
                else
                {
                    Customization Def = new Customization(Assembly.GetExecutingAssembly().FullName, Assembly.GetExecutingAssembly().GetName().Version.ToString());
                    return Def.GetIconStatus(this.evn);
                }
            }
            private set { }
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="evn">Тип события которое произошло на уровне всей системы</param>
        /// <param name="Message">Сообщение которое нужно в трее сделать всплывающим</param>
        public EventChangeStatus (EventEn evn, string Message)
        {
            try
            {
                this.evn = evn;
                this.Message = Message;
                this.EventEnable = true;
            }
            catch (Exception ex)
            {
                ApplicationException ae = new ApplicationException(string.Format("Упали при инициализации конструктора с ошибкой: ({0})", ex.Message));
                Log.EventSave(ae.Message, string.Format("{0}.EventChangeStatus", this.GetType().FullName), EventEn.Error);
                throw ae;
            }
        }
    }
}
