using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections;

namespace Common.ParamPlg
{
    /// <summary>
    /// Класс для реализации конкретного парметра
    /// </summary>
    public abstract partial class ParamBase
    {
        /// <summary>
        /// Класс для реализации списка параметров
        /// </summary>
        public class ParamListBase
        {
            #region Param (private)

            /// <summary>
            /// Интерфейс для базового класса пула чтобы он мог дёргать скрытыем методы
            /// </summary>
            private ParamListI ParamListI = null;

            /// <summary>
            /// Внутренний список
            /// </summary>
            private List<Param> ParamL = new List<Param>();

            #endregion

            #region Param (public get; private set;)

            /// <summary>
            /// Получить текущие количество элементов в списке
            /// </summary>
            public int Count
            {
                get
                {
                    try
                    {
                        int rez = 0;

                        lock (this.ParamL)
                        {
                            rez = this.ParamL.Count;
                        }

                        return rez;
                    }
                    catch (Exception ex)
                    {
                        ApplicationException ae = new ApplicationException(string.Format(@"Ошибка в методе Count:""{0}""", ex.Message));
                        Log.EventSave(ae.Message, string.Format("{0}.EventSave", this.GetType().FullName), EventEn.Error);
                        throw ae;
                    }
                }
                private set { }
            }
                        

            #endregion

            #region Param (public get; protected set;)



            #endregion

            #region События

            /// <summary>
            /// Событие добавления объекта во внутренний список
            /// </summary>
            public event EventHandler<EventParamAdd> onParamAdd;

            /// <summary>
            /// Событие обновления объекта во внутренний список
            /// </summary>
            public event EventHandler<EventParamUpdate> onParamUpdate;

            /// <summary>
            /// Событие удаления объекта из внутреннего списока
            /// </summary>
            public event EventHandler<EventParamDelete> onParamDelete;

            #endregion

            #region Индексаторы

            /// <summary>
            /// Индексатор для поиска объекта по его индексу
            /// </summary>
            /// <param name="index">Индекс по которому мы ищем объект</param>
            /// <returns>Возвращаем найденый объект</returns>
            public Param this[int index]
            {
                get
                {
                    Param rez = null;
                    try
                    {
                        lock (this.ParamL)
                        {
                            rez = this.ParamL[index];
                        }
                    }
                    catch (Exception ex)
                    {
                        ApplicationException ae = new ApplicationException(string.Format(@"Ошибка в индексаторе Io this[int index]:""{0}""", ex.Message));
                        Log.EventSave(ae.Message, string.Format("{0}.Param[int]", this.GetType().FullName), EventEn.Error);
                        throw ae;
                    }
                    return rez;
                }
                private set { }
            }

            #endregion

            #region Method (public)

            /// <summary>
            /// Конструктор
            /// </summary>
            public ParamListBase()
            {
                try
                {
                }
                catch (Exception ex)
                {
                    ApplicationException ae = new ApplicationException(string.Format("Упали при инициализации конструктора с ошибкой: ({0})", ex.Message));
                    Log.EventSave(ae.Message, string.Format("{0}.ParamListBase", this.GetType().FullName), EventEn.Error);
                    throw ae;
                }
            }

            /// <summary>
            /// IEnumerator для успешной работы конструкции foreach
            /// </summary>
            /// <returns>IEnumerator</returns>
            public IEnumerator GetEnumerator()
            {
                try
                {
                    lock (this.ParamL)
                    {
                        //return this.IoL.GetEnumerator();
                        return new ParamListBaseEnumerator(ref this.ParamL);
                    }
                }
                catch (Exception ex)
                {
                    ApplicationException ae = new ApplicationException(string.Format(@"Ошибка в методе Add:""{0}""", ex.Message));
                    Log.EventSave(ae.Message, string.Format("{0}.GetEnumerator", this.GetType().FullName), EventEn.Error);
                    throw ae;
                }
            }

            #endregion

            #region Method (public virtual)

            /// <summary>
            /// Добавление элемента в список
            /// </summary>
            /// <param name="nIParam">Элемент который мы добавили в список</param>
            public virtual void Add(Param nParam)
            {
                try
                {
                    lock (this.ParamL)
                    {
                        nParam.index = this.Count;
                        this.ParamL.Add(nParam);
                    }

                    if (onParamAdd != null)
                    {
                        EventParamAdd MyArg = new EventParamAdd(nParam);
                        onParamAdd.Invoke(this, MyArg);
                    }
                }
                catch (Exception ex)
                {
                    ApplicationException ae = new ApplicationException(string.Format(@"Ошибка в методе Add:""{0}""", ex.Message));
                    Log.EventSave(ae.Message, string.Format("{0}.Add", this.GetType().FullName), EventEn.Error);
                    throw ae;
                }
            }

            /// <summary>
            /// Обновление элемента в списоке
            /// </summary>
            /// <param name="nParam">Элемент знаяение которого мы применим</param>
            /// <param name="dParam">Элемент который необходимо обновить</param>
            public virtual void Update(Param nParam, Param dParam)
            {
                try
                {
                    lock (this.ParamL)
                    {
                        int index = dParam.index;
                        nParam.index = index;
                        ParamL[index] = nParam;
                    }

                    if (onParamUpdate != null)
                    {
                        EventParamUpdate MyArg = new EventParamUpdate(nParam, dParam);
                        onParamUpdate.Invoke(this, MyArg);
                    }
                }
                catch (Exception ex)
                {
                    ApplicationException ae = new ApplicationException(string.Format(@"Ошибка в методе Update:""{0}""", ex.Message));
                    Log.EventSave(ae.Message, string.Format("{0}.Update", this.GetType().FullName), EventEn.Error);
                    throw ae;
                }
            }

            /// <summary>
            /// Удаление элемента из списка
            /// </summary>
            /// <param name="nIo">Элемент который необходимо обновить</param>
            public virtual void Delete(Param dParam)
            {
                try
                {
                    if (dParam.index == -1) throw new ApplicationException("У элемента не задан индекс скорее всего он не является членом списка.");

                    lock (this.ParamL)
                    {
                        int index = dParam.index;

                        this.ParamL.RemoveAt(index);

                        // Настраиваем индексы так как при считывании с базы их нет
                        for (int i = index; i < this.ParamL.Count; i++)
                        {
                            this.SetIndex(this.ParamL[i], i);
                        }
                    }

                    if (onParamDelete != null)
                    {
                        EventParamDelete MyArg = new EventParamDelete(dParam);
                        onParamDelete.Invoke(this, MyArg);
                    }
                }
                catch (Exception ex)
                {
                    ApplicationException ae = new ApplicationException(string.Format(@"Ошибка в методе Update:""{0}""", ex.Message));
                    Log.EventSave(ae.Message, string.Format("{0}.Delete", this.GetType().FullName), EventEn.Error);
                    throw ae;
                }
            }

            #endregion

            #region Method (protected)

            /// <summary>
            /// Установка индекса в классе
            /// </summary>
            /// <param name="nParam">Объект в котором нужно установить</param>
            /// <param name="index">Индекс который надо установить</param>
            protected void SetIndex(Param nParam, int index)
            {
                try
                {
                    nParam.index = index;
                }
                catch (Exception ex)
                {
                    ApplicationException ae = new ApplicationException(string.Format(@"Ошибка в методе SetIndex:""{0}""", ex.Message));
                    Log.EventSave(ae.Message, string.Format("{0}.SetIndex", this.GetType().FullName), EventEn.Error);
                    throw ae;
                }
            }

            #endregion

            #region Method (private)


            #endregion


            #region CrossClass

            /// <summary>
            /// Внутренний класс для линковки интерфейсов састомного класса скрытых для пользователя
            /// </summary>
            public class CrossLink
            {
                // Кастомный объект пула
                private ParamListBase CustParamList;

                /// <summary>
                /// Линкуеминтерфейс ParamI скрытый для пользователя
                /// </summary>
                /// <param name="CustParamList">Кастомный обьект для линковки</param>
                public CrossLink(ParamListBase CustParamList)
                {
                    try
                    {
                        this.CustParamList = CustParamList;
                        CustParamList.ParamListI = (ParamListI)CustParamList;
                    }
                    catch (Exception ex)
                    {
                        ApplicationException ae = new ApplicationException(string.Format("Упали при инициализации конструктора с ошибкой: ({0})", ex.Message));
                        Log.EventSave(ae.Message, string.Format("{0}.CrossLink", this.GetType().FullName), EventEn.Error);
                        throw ae;
                    }
                }

                /*             /// <summary>
                             /// Запуск асинхронного процесса обслуживающенго наш пул с точки зрения общих вещей на уровне базового класса
                             /// </summary>
                             public void StartCompileListing()
                             {
                                 try
                                 {
                                     this.CustIoList.StartCompileListing();
                                 }
                                 catch (Exception ex)
                                 {
                                     ApplicationException ae = new ApplicationException(string.Format("Упали при запуске асинхронного процесса: ({0})", ex.Message));
                                     Log.EventSave(ae.Message, string.Format("{0}.StartCompileListing", this.GetType().FullName), EventEn.Error);
                                     throw ae;
                                 }
                             }

                             /// <summary>
                             /// Остановка асинхронного процесса обслуживающенго наш пул с точки зрения общих вещей на уровне базового класса без ожидания завершения процесса
                             /// </summary>
                             public void StopCompileListing()
                             {
                                 try
                                 {
                                     this.CustIoList.StopCompileListing();
                                 }
                                 catch (Exception ex)
                                 {
                                     ApplicationException ae = new ApplicationException(string.Format("Упали при остановке асинхронного процесса: ({0})", ex.Message));
                                     Log.EventSave(ae.Message, string.Format("{0}.StopCompileListing", this.GetType().FullName), EventEn.Error);
                                     throw ae;
                                 }
                             }

                             /// <summary>
                             /// Остановка аснхронных процессов перед выключением всех потоков
                             /// </summary>
                             /// <param name="Aborting">True если с прерывением всех процессов жёстное отклучение всех процессов</param>
                             public void Join(bool Aborting)
                             {
                                 try
                                 {
                                     this.CustIoList.Join(Aborting);
                                 }
                                 catch (Exception ex)
                                 {
                                     ApplicationException ae = new ApplicationException(string.Format("Упали при ожиданииостановки асинхронного процесса: ({0})", ex.Message));
                                     Log.EventSave(ae.Message, string.Format("{0}.Join", this.GetType().FullName), EventEn.Error);
                                     throw ae;
                                 }
                             }
                         }
                */
            }
            #endregion    







        }
    }
}
