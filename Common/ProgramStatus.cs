using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    /// <summary>
    /// Состояние программы на сейчас
    /// </summary>
    public class ProgramStatus
    {
        /// <summary>
        /// Состояние блокировки операционной системы
        /// </summary>
        public static bool HashLock = false;

        /// <summary>
        /// Событие показывающее статус блокировки экрана
        /// </summary>
        public static event EventHandler <EventHashLock> onEventHashLock;

        /// <summary>
        /// Активация события блокироваки или разблокироваки рабочего стола
        /// </summary>
        /// <param name="SetHashLock"></param>
        public static void onEventHashLockActivation(bool SetHashLock)
        {
            HashLock = SetHashLock;

            // Собственно обработка события
            EventHashLock myArg = new EventHashLock(SetHashLock);
            if (ProgramStatus.onEventHashLock != null)
            {
                ProgramStatus.onEventHashLock.Invoke(null, myArg);
            }

            if (SetHashLock) Log.EventSave("Компьютер заблокирован", "ProgramStatus", EventEn.Message);
            else Log.EventSave("Компьютер разблокирован", "ProgramStatus", EventEn.Message);

        }

        /// <summary>
        /// Текущее состояние нашего приложения которое транслировалось в статус по событию можно понять какой статус иконок должен быть сейчас
        /// </summary>
        public static EventEn GlobalStatus = EventEn.Empty;

        /// <summary>
        /// Подписываемся на событие изменения статуса для того чтобы реализовать изменение иконок в зависимости от текущего статуса
        /// </summary>
        public static event EventHandler<EventChangeStatus> onEventChangeStatus;


        /// <summary>
        /// Список доступных пулов со своими классами
        /// </summary>
        public static List<IoList> CurentIoPulList = null;

        /// <summary>
        /// Процесс создания и запуска процессов в наших пулах с плагинами
        /// </summary>
        public static void CreateCurentPulList()
        {
            try
            {
                // Если список пулов ещё не создавали то создаём его
                if (CurentIoPulList == null)
                {
                    CurentIoPulList = new List<IoList>();

                    // Пробегаем по всем доступным объектам
                    foreach (PluginClassElementList itemList in IoListFarm.ListIoListName)
                    {
                        foreach (PluginClassElement item in itemList.Items)
                        {
                            // Создаём пул и добавляем его к списку
                            IoList nIoList = IoListFarm.CreateNewIoList(item);
                            CurentIoPulList.Add(nIoList);

                            // Запускаем процесс на нашем пуле в базовом классе
                            nIoList.StartCompileListing();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationException ae = new ApplicationException(string.Format(@"Ошибка при создании класса плагина:""{0}""", ex.Message));
                Log.EventSave(ae.Message, string.Format("{0}.EventSave", "IoFarm.CreateCurentPulList"), EventEn.Error);
                throw ae;
            }
        }

        /* 
 /// <summary>
 /// Асинхронный статус для фиксации состояния всей ноды целиком чтобы видеть работает она сейчас или нет на стороне базы
 /// </summary>
 private static void ACreateCurentPulList()
 {
     try
     {
         // Устанавливаем тайм из конфига
         int CountWhile = Com.Config.SecondPulRefreshStatus;

         while (IsRunThrCreateCurentPulList)
         {
             if (CountWhile == 0)
             {
                 // Устанавливаем тайм аут из конфига
                 CountWhile = Com.Config.SecondPulRefreshStatus;

                 // Если появилось подключение к базе данных и ещё небыло успешной регистрации нашего пула то делаем её в системе для того чтобы сервис знал о том что сервис такой существует 
                 if (Com.RepositoryFarm.CurentRep != null && Com.RepositoryFarm.CurentRep.HashConnect)
                 {
                     // Фиксируем версию нашего приложения и его статус
                     Version Ver = Assembly.GetExecutingAssembly().GetName().Version;
                     ((RepositoryI)Com.RepositoryFarm.CurentRep).NodeSetStatus(Environment.MachineName, DateTime.Now, Ver.ToString(), EventEn.Runned.ToString());
                 }
             }

             Thread.Sleep(1000);     // Тайм аут между проверками статуса
             CountWhile--;
         }
     }
     catch (Exception ex)
     {
         Com.Log.EventSave(string.Format(@"Ошибка при создании класса плагина:""{0}""", ex.Message), "IoFarm.CreateCurentPuulList", EventEn.Error, true, true);
         throw ex;
     }
 }


 /// <summary>
 /// Остановка аснхронных процессов перед выключением всех потоков
 /// </summary>
 public static void Stop()
 {
     try
     {
         // Если список пулов ещё создавали
         if (CurentPulList != null)
         {
             IsRunThrCreateCurentPulList = false;

             // Пробегаем по всем доступным объектам
             foreach (IoBase.IoListBase.CrossLink itemPul in CurentPulCrossLink)
             {
                 itemPul.StopCompileListing();
             }
         }
     }
     catch (Exception ex)
     {
         Com.Log.EventSave(string.Format(@"Ошибка при создании класса плагина:""{0}""", ex.Message), "IoFarm.Stop", EventEn.Error, true, true);
         throw ex;
     }
 }

 /// <summary>
 /// Остановка аснхронных процессов перед выключением всех потоков
 /// </summary>
 /// <param name="Aborting">True если с прерывением всех процессов жёстное отклучение всех процессов</param>
 public static void Join(bool Aborting)
 {
     try
     {
         // Если список пулов ещё не создавали то создаём его
         if (CurentPulList != null)
         {
             // Пробегаем по всем доступным объектам
             Stop();

             if (Com.RepositoryFarm.CurentRep != null && Com.RepositoryFarm.CurentRep.HashConnect)
             {
                 // Фиксируем версию нашего приложения и его статус
                 Version Ver = Assembly.GetExecutingAssembly().GetName().Version;
                 ((RepositoryI)Com.RepositoryFarm.CurentRep).NodeSetStatus(Environment.MachineName, DateTime.Now, Ver.ToString(), EventEn.Stoping.ToString());
             }

             // Пробегаем по всем доступным объектам
             foreach (IoBase.IoListBase.CrossLink itemPul in CurentPulCrossLink)
             {
                 itemPul.Join(Aborting);
             }

             if (ThrCreateCurentPulList != null) ThrCreateCurentPulList.Join();
             if (Com.RepositoryFarm.CurentRep != null && Com.RepositoryFarm.CurentRep.HashConnect)
             {
                 // Фиксируем версию нашего приложения и его статус
                 Version Ver = Assembly.GetExecutingAssembly().GetName().Version;
                 ((RepositoryI)Com.RepositoryFarm.CurentRep).NodeSetStatus(Environment.MachineName, DateTime.Now, Ver.ToString(), EventEn.Stop.ToString());
             }
         }
     }
     catch (Exception ex)
     {
         Com.Log.EventSave(string.Format(@"Ошибка при создании класса плагина:""{0}""", ex.Message), "IoFarm.Join", EventEn.Error, true, true);
         throw ex;
     }
 }
 */

    }
}
