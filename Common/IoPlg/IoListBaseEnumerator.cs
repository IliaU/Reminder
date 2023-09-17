using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections;

namespace Common.IoPlg
{
    /// <summary>
    /// Собственный нумератор
    /// </summary>
    public class IoListBaseEnumerator : IEnumerator
    {
        /// <summary>
        /// Внутренний список
        /// </summary>
        private List<Io> IoL;

        /// <summary>
        /// Позиция
        /// </summary>
        int position = -1;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="IoL">Список по которому строим перечеслитель</param>
        public IoListBaseEnumerator(ref List<Io> IoL)
        {
            try
            {
                this.IoL = IoL;
            }
            catch (Exception ex)
            {
                ApplicationException ae = new ApplicationException(string.Format("Упали при инициализации конструктора с ошибкой: ({0})", ex.Message));
                Log.EventSave(ae.Message, string.Format("{0}.IoListBaseEnumerator", this.GetType().FullName), EventEn.Error);
                throw ae;
            }
        }

        /// <summary>
        /// Получение текущего объекта
        /// </summary>
        public object Current
        {
            get
            {
                try
                {
                    lock (this.IoL)
                    {
                        if (position == -1 || position >= this.IoL.Count) throw new ArgumentException();
                        return this.IoL[position];
                    }
                }
                catch (Exception ex)
                {
                    ApplicationException ae = new ApplicationException(string.Format(@"Ошибка в параметре Current:""{0}""", ex.Message));
                    Log.EventSave(ae.Message, string.Format("{0}.Current", this.GetType().FullName), EventEn.Error);
                    throw ae;
                }
            }
        }

        /// <summary>
        /// Переключение перечислитиля на следующий элемент
        /// </summary>
        /// <returns>статус успешно илинет</returns>
        public bool MoveNext()
        {
            try
            {
                lock (this.IoL)
                {
                    if (position < this.IoL.Count - 1)
                    {
                        position++;
                        return true;
                    }
                    else return false;
                }
            }
            catch (Exception ex)
            {
                ApplicationException ae = new ApplicationException(string.Format(@"Ошибка в методе MoveNext:""{0}""", ex.Message));
                Log.EventSave(ae.Message, string.Format("{0}.MoveNext", this.GetType().FullName), EventEn.Error);
                throw ae;
            }
        }

        /// <summary>
        /// Сброс перечислителя
        /// </summary>
        public void Reset()
        {
            try
            {
                lock (this.IoL)
                {
                    position = -1;
                }
            }
            catch (Exception ex)
            {
                ApplicationException ae = new ApplicationException(string.Format(@"Ошибка в методе Reset:""{0}""", ex.Message));
                Log.EventSave(ae.Message, string.Format("{0}.Reset", this.GetType().FullName), EventEn.Error);
                throw ae;
            }
        }
    }
}
