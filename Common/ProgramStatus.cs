﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading;
using System.Reflection;
using Common.RepositoryPlg;

namespace Common
{
    /// <summary>
    /// Состояние программы на сейчас
    /// </summary>
    public class ProgramStatus
    {

        /// <summary>
        /// Поток с асинхронной обработкой
        /// </summary>
        private static Thread ThrAStartCompileListing;

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
        /// Флаг для аснхронного процесса который отображает текущий статус процесса
        /// </summary>
        public static bool IsRunThrCreateCurentPulList { get; private set; } = false;

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
                            // nIoList.StartCompileListing();
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

        /// <summary>
        /// Запуск асинхронного процесса обслуживающенго наш пул с точки зрения общих вещей на уровне базового класса
        /// </summary>
        public static void StartCompileListing()
        {
            try
            {
                // Если список пулов ещё не создавали то создаём его
                if (CurentIoPulList == null) throw new ApplicationException("Список пулов ещё не подгружен и не создан.");

                if (IsRunThrCreateCurentPulList) throw new ApplicationException("Процесс уже запущен. Надо сначало остановить текущий процесс и дождаться окончания его работы.");
                IsRunThrCreateCurentPulList = true;

                // Асинхронный запуск процесса
                ThrAStartCompileListing = new Thread(ACreateCurentPulList);
                ThrAStartCompileListing.Name = "ACreateCurentPulList";
                ThrAStartCompileListing.IsBackground = true;
                ThrAStartCompileListing.Start();


                // Пробегаем по всем доступным объектам и запускаем в них процесс
                foreach (IoList itemPul in CurentIoPulList)
                {
                    itemPul.StartCompileListing();
                }
            }
            catch (Exception ex)
            {
                ApplicationException ae = new ApplicationException(string.Format("Упали при инициализации конструктора с ошибкой: ({0})", ex.Message));
                Log.EventSave(ae.Message, "ProgramStatus.StartCompileListing", EventEn.Error);
                throw ae;
            }
        }

        /// <summary>
        /// Асинхронный статус для фиксации состояния всей ноды целиком чтобы видеть работает она сейчас или нет на стороне базы
        /// </summary>
        private static void ACreateCurentPulList()
        {
            try
            {
                // Устанавливаем тайм из конфига
                int CountWhile = Config.SecondPulRefreshStatus;

                while (IsRunThrCreateCurentPulList)
                {
                    if (CountWhile == 0)
                    {
                        // Устанавливаем тайм аут из конфига
                        CountWhile = Config.SecondPulRefreshStatus;

                        // Если появилось подключение к базе данных и ещё небыло успешной регистрации нашего пула то делаем её в системе для того чтобы сервис знал о том что сервис такой существует 
                        if (RepositoryFarm.CurRepository != null && RepositoryFarm.CurRepository.HashConnect)
                        {
                            // Фиксируем версию нашего приложения и его статус
                            Version Ver = Assembly.GetExecutingAssembly().GetName().Version;
                            ((RepositoryI)RepositoryFarm.CurRepository).NodeSetStatus(Environment.MachineName, DateTime.Now, Ver.ToString(), EventEn.Running.ToString());
                        }
                    }

                    Thread.Sleep(1000);     // Тайм аут между проверками статуса
                    CountWhile--;
                }
            }
            catch (Exception ex)
            {
                ApplicationException ae = new ApplicationException(string.Format(@"Ошибка при создании класса плагина:""{0}""", ex.Message));
                Log.EventSave(ae.Message, "ProgramStatus.CreateCurentPulList", EventEn.Error);
                throw ae;
            }
        }

         /// <summary>
         /// Остановка аснхронных процессов перед выключением всех потоков
         /// </summary>
         public static void Stop()
         {
             try
             {
                // Если список пулов создавали
                if (CurentIoPulList != null)
                {
                    IsRunThrCreateCurentPulList = false;

                    // Пробегаем по всем доступным объектам
                    foreach (IoList itemPul in CurentIoPulList)
                    {
                        itemPul.StopCompileListing();
                    }
                }
             }
             catch (Exception ex)
             {
                ApplicationException ae = new ApplicationException(string.Format("Упали при остановке мониторинга ноды с ошибкой: ({0})", ex.Message));
                Log.EventSave(ae.Message, "ProgramStatus.StartCompileListing", EventEn.Error);
                throw ae;
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
                 if (CurentIoPulList != null)
                 {
                     // Пробегаем по всем доступным объектам
                     Stop();

                     if (RepositoryFarm.CurRepository != null && RepositoryFarm.CurRepository.HashConnect)
                     {
                         // Фиксируем версию нашего приложения и его статус
                         Version Ver = Assembly.GetExecutingAssembly().GetName().Version;
                         ((RepositoryI)RepositoryFarm.CurRepository).NodeSetStatus(Environment.MachineName, DateTime.Now, Ver.ToString(), EventEn.Stoping.ToString());
                     }

                     // Пробегаем по всем доступным объектам
                     foreach (IoList itemPul in CurentIoPulList)
                     {
                         itemPul.Join(Aborting);
                     }

                     if (ThrAStartCompileListing != null) ThrAStartCompileListing.Join();
                     if (RepositoryFarm.CurRepository != null && RepositoryFarm.CurRepository.HashConnect)
                     {
                         // Фиксируем версию нашего приложения и его статус
                         Version Ver = Assembly.GetExecutingAssembly().GetName().Version;
                         ((RepositoryI)RepositoryFarm.CurRepository).NodeSetStatus(Environment.MachineName, DateTime.Now, Ver.ToString(), EventEn.Stop.ToString());
                     }
                 }
             }
             catch (Exception ex)
             {
                ApplicationException ae = new ApplicationException(string.Format("Упали при ожидании завершения процессов остановке мониторинга ноды с ошибкой: ({0})", ex.Message));
                Log.EventSave(ae.Message, "ProgramStatus.StartCompileListing", EventEn.Error);
                throw ae;
             }
         }
    }
}
