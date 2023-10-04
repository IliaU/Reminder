using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Reflection;
using System.IO;

namespace Common
{
    /// <summary>
    /// Класс для работы с провайдером
    /// </summary>
    public class ProviderFarm
    {
        /// <summary>
        /// Корневая директория нашего плагина
        /// </summary>
        public static string ParentDirectoryForPlugin
        {
            get { return Environment.CurrentDirectory; }
            private set { }
        }

        /// <summary>
        /// FolderName нашего плагина
        /// </summary>
        public static string FolderForPlugin
        {
            get { return "ProviderPlg"; }
            private set { }
        }

        /// <summary>
        /// Путь к папке с плагинами нашего плагина
        /// </summary>
        public static string PathForPlugin
        {
            get { return string.Format("{0}\\ProviderPlg", ParentDirectoryForPlugin); }
            private set { }
        }

        /// <summary>
        /// Получаем список доступных провайдеров
        /// </summary>
        /// <returns>Список имён доступных провайдеров</returns>
        public static List<PluginClassElement> ListProviderName;

        /// <summary>
        /// Текущий провайдер c объектами нашей инфраструктуры используется как база с структурой предприятия и содержит списки компонент которые надо мониторить
        /// </summary>
        public static Provider CurProviderObj;

        /// <summary>
        /// Текущий провайдер c объектами с логами мониторинга в который складиваем информацию и на основе которой происходит мониторинг необходимых объектов. 
        /// Например CurProviderObj лежит список хостов а в  этом провайдере лажал сами логи по доступности их. Это позволяет разнести нагрузку по разными нодам.
        /// А рнепозиторий занят всеми задачами по связыванию много кластерного вычесления и обслуживанием этой задачи
        /// </summary>
        public static Provider CurProviderMon;

        /// <summary>
        /// Событие изменения текущего универсального провайдера для объектов
        /// </summary>
        public static event EventHandler<EventProviderFarm> onEventSetupObj;

        /// <summary>
        /// Событие изменения текущего универсального провайдера для мониторинга
        /// </summary>
        public static event EventHandler<EventProviderFarm> onEventSetupMon;

        /// <summary>
        /// Конструктор
        /// </summary>
        public ProviderFarm()
        {
            try
            {
                if (ListProviderName == null)
                {
                    Log.EventSave("Инициализация классов провайдеров", GetType().Name, EventEn.Message);
                    GetListProviderName();

                    // Установка текущего репозитория по умолчанию
                    //string dd = ListProviderName.Find(x => x == Config.ProviderName);
                    //if (!string.IsNullOrWhiteSpace(dd)) CurProvider = CreateNewProvider("DisplayDSP840");
                }
            }
            catch (Exception ex)
            {
                ApplicationException ae = new ApplicationException(string.Format("Упали при инициализации конструктора с ошибкой: ({0})", ex.Message));
                Log.EventSave(ae.Message, string.Format("{0}.ProviderFarm", this.GetType().FullName), EventEn.Error);
                throw ae;
            }
        }

        /// <summary>
        /// Установка текущего провайдера
        /// </summary>
        /// <param name="NewPrv">Провайдер который установить по умолчанию для базы с объектами</param>
        /// <param name="WriteLog">Запись в лог</param>
        public static void SetCurrentProviderObj(Provider NewPrv, bool WriteLog)
        {
            try
            {
                CurProviderObj = NewPrv;

                // Собственно обработка события
                EventProviderFarm myArg = new EventProviderFarm(NewPrv);
                if (onEventSetupObj != null)
                {
                    onEventSetupObj.Invoke(NewPrv, myArg);
                }

                // Логируем изменение подключения
                if (WriteLog && NewPrv != null) Log.EventSave(string.Format("Пользователь настроил провайдер для базы объектов: {0} ({1})", NewPrv.PrintConnectionString(), NewPrv.PlugInType), "ProviderFarm", EventEn.Message);
            }
            catch (Exception ex)
            {
                ApplicationException ae = new ApplicationException(string.Format("Упали при инициализации конструктора с ошибкой: ({0})", ex.Message));
                Log.EventSave(ae.Message, "ProviderFarm.SetCurrentProviderObj", EventEn.Error);
                throw ae;
            }
        }

        /// <summary>
        /// Установка текущего провайдера
        /// </summary>
        /// <param name="NewPrv">Провайдер который установить по умолчанию для базы мониторинга</param>
        /// <param name="WriteLog">Запись в лог</param>
        public static void SetCurrentProviderMon(Provider NewPrv, bool WriteLog)
        {
            try
            {
                CurProviderMon = NewPrv;

                // Собственно обработка события
                EventProviderFarm myArg = new EventProviderFarm(NewPrv);
                if (onEventSetupMon != null)
                {
                    onEventSetupMon.Invoke(NewPrv, myArg);
                }

                // Логируем изменение подключения
                if (WriteLog && NewPrv !=null) Log.EventSave(string.Format("Пользователь настроил провайдер для базы мониторинга: {0} ({1})", NewPrv.PrintConnectionString(), NewPrv.PlugInType), "ProviderFarm", EventEn.Message);
            }
            catch (Exception ex)
            {
                ApplicationException ae = new ApplicationException(string.Format("Упали при инициализации конструктора с ошибкой: ({0})", ex.Message));
                Log.EventSave(ae.Message, "ProviderFarm.SetCurrentProviderMon", EventEn.Error);
                throw ae;
            }
        }

        /// <summary>
        /// Получаем список провайдеров
        /// </summary>
        /// <returns>Список доступных провайдеров</returns>
        public static List<PluginClassElement> GetListProviderName()
        {
            try
            {
                // Если список ещё не получали то получаем его
                if (ListProviderName == null)
                {
                    ListProviderName = new List<PluginClassElement>();

                    // Проверяем наличие папки и если её нет то создаём её
                    if (!Directory.Exists(PathForPlugin)) Directory.CreateDirectory(PathForPlugin);
                    //
                    // Пробегаем по папке и подгружаем все плагины из неё
                    foreach (string filePath in Directory.GetFiles(PathForPlugin, "*.dll"))
                    {
                        Assembly asm = Assembly.LoadFrom(filePath);
                        //Type[] typelist = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.Namespace == "AlgoritmPrizm.Com.DisplayPlg").ToArray();
                        Type[] typelist = asm.GetTypes();//.Where(t => t.Namespace == "AlgoritmPrizm.Com.DisplayPlg").ToArray();

                        foreach (Type item in typelist)
                        {
                            // Проверяем реализовывает ли класс наш интерфейс если да то это провайдер который можно подгрузить
                            bool flagI = false;

                            foreach (Type i in item.GetInterfaces())
                            {
                                string Search = "ProviderPlg.ProviderI";
                                if (i.FullName.Length > Search.Length && i.FullName.Substring(i.FullName.Length - Search.Length) == Search)
                                {
                                    flagI = true;
                                    break;
                                }
                            }
                            if (!flagI) continue;

                            // Проверяем что наш клас наследует PlugInBase 
                            bool flagB = false;
                            foreach (MemberInfo mi in item.GetMembers())
                            {
                                string Search = "ProviderPlg.ProviderBase";
                                if (mi.DeclaringType.FullName.Length > Search.Length && mi.DeclaringType.FullName.Substring(mi.DeclaringType.FullName.Length - Search.Length) == Search)
                                {
                                    flagB = true;
                                    break;
                                }
                            }
                            if (!flagB) continue;

                            // Проверяем конструктор нашего класса  
                            bool flag1 = false;
                            bool flag0 = false;
                            string nameConstructor;
                            foreach (ConstructorInfo ctor in item.GetConstructors())
                            {
                                nameConstructor = item.Name;

                                // получаем параметры конструктора  
                                ParameterInfo[] parameters = ctor.GetParameters();


                                // если в этом конструктаре должно быть несколько параметров
                                if (parameters.Length == 1)
                                {
                                    bool flag = true;
                                    if (parameters[0].ParameterType.Name != "String" || parameters[0].Name != "ConnectionString") flag = false;

                                    flag1 = flag;
                                }


                                // Проверяем конструктор для создания документа пустого по умолчанию
                                if (parameters.Length == 0) flag0 = true;
                            }
                            if (!flag1) continue;
                            if (!flag0) continue;

                            ListProviderName.Add(new PluginClassElement(item.Name, item, null));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.EventSave(ex.Message, "ProviderFarm.GetListProviderName", EventEn.Error);
            }

            return ListProviderName;
        }

        /// <summary>
        /// Создание провайдера определённого типа с параметрами из конфига
        /// </summary>
        /// <param name="PlugInType">Имя плагина определяющего тип провайдера который создаём</param>
        /// <param name="ConnectionString">Строка подключения к провайдеру</param>
        /// <returns>Возвращаем провайдер</returns>
        public static Provider CreateNewProvider(string PlugInType, string ConnectionString)
        {
            Provider rez = null;
            try
            {
                // Если списка репозиториев ещё нет то создаём его
                GetListProviderName();


                // Проверяем наличие существование этого типа плагина
                foreach (PluginClassElement item in ListProviderName)
                {
                    if (item.Name == PlugInType.Trim())
                    {
                        // Создаём экземпляр объекта
                        object[] targ = { (string.IsNullOrWhiteSpace(ConnectionString) ? (object)null : ConnectionString) };
                        rez = (Provider)Activator.CreateInstance(item.EmptTyp, targ);

                        // Линкуем в базовый класс специальный скрытый интерфейс для того чтобы базовый класс мог что-то специфическое вызывать в дочернем объекте
                        //ProviderPlg.Lib.ProviderBase.CrossLink CrLink = new ProviderPlg.Lib.ProviderBase.CrossLink(rez);

                        break;
                    }
                }


            }
            catch (Exception ex)
            {
                Log.EventSave(ex.Message, "ProviderFarm.CreateNewProvider", EventEn.Error);
            }

            return rez;
        }

        /// <summary>
        /// Создание провайдера определённого типа с параметрами из конфига
        /// </summary>
        /// <param name="PlugInType">Имя плагина определяющего тип провайдера который создаём</param>
        /// <returns>Возвращаем провайдер</returns>
        public static Provider CreateNewProvider(string PlugInType)
        {
            Provider rez = null;
            try
            {
                // Если списка репозиториев ещё нет то создаём его
                GetListProviderName();


                // Проверяем наличие существование этого типа плагина
                foreach (PluginClassElement item in ListProviderName)
                {
                    if (item.Name == PlugInType.Trim())
                    {
                        // Создаём экземпляр объекта
                        //object[] targ = { (string.IsNullOrWhiteSpace(ConnectionString) ? (object)null : ConnectionString) };
                        rez = (Provider)Activator.CreateInstance(item.EmptTyp);//, targ);

                        // Линкуем в базовый класс специальный скрытый интерфейс для того чтобы базовый класс мог что-то специфическое вызывать в дочернем объекте
                        //ProviderPlg.Lib.ProviderBase.CrossLink CrLink = new ProviderPlg.Lib.ProviderBase.CrossLink(rez);

                        break;
                    }
                }


            }
            catch (Exception ex)
            {
                Log.EventSave(ex.Message, "ProviderFarm.CreateNewProvider", EventEn.Error);
            }

            return rez;
        }
    }
}
