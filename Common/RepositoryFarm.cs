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
    /// Класс для работы с репозиторием
    /// </summary>
    public class RepositoryFarm
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
            get { return "RepositoryPlg"; }
            private set { }
        }

        /// <summary>
        /// Путь к папке с плагинами нашего плагина
        /// </summary>
        public static string PathForPlugin
        {
            get { return string.Format("{0}\\RepositoryPlg", ParentDirectoryForPlugin); }
            private set { }
        }

        /// <summary>
        /// Получаем список доступных репозиториев
        /// </summary>
        /// <returns>Список имён доступных репозиториев</returns>
        public static List<PluginClassElement> ListRepositoryName;

        /// <summary>
        /// Текущий репозиторий
        /// </summary>
        public static Repository CurRepository;


        /// <summary>
        /// Событие изменения текущего универсального репозитория
        /// </summary>
        public static event EventHandler<EventRepositoryFarm> onEventSetup;

        /// <summary>
        /// Конструктор
        /// </summary>
        public RepositoryFarm()
        {
            try
            {
                if (ListRepositoryName == null)
                {
                    Log.EventSave("Инициализация классов репозитория", GetType().Name, EventEn.Message);
                    GetListRepositoryName();

                    // Установка текущего репозитория по умолчанию
                    //string dd = ListRepositoryName.Find(x => x == Config.RepositoryName);
                    //if (!string.IsNullOrWhiteSpace(dd)) CurRepository = CreateNewRepository("DisplayDSP840");
                }
            }
            catch (Exception ex)
            {
                ApplicationException ae = new ApplicationException(string.Format("Упали при инициализации конструктора с ошибкой: ({0})", ex.Message));
                Log.EventSave(ae.Message, string.Format("{0}.RepositoryFarm", this.GetType().FullName), EventEn.Error);
                throw ae;
            }
        }

        /// <summary>
        /// Установка текущего репозитория
        /// </summary>
        /// <param name="NewRep">Репозиторий который установить по умолчанию</param>
        /// <param name="WriteLog">Запись в лог</param>
        public static void SetCurrentRepository(Repository NewRep, bool WriteLog)
        {
            try
            {
                CurRepository = NewRep;

                // Собственно обработка события
                EventRepositoryFarm myArg = new EventRepositoryFarm(NewRep);
                if (onEventSetup != null)
                {
                    onEventSetup.Invoke(NewRep, myArg);
                }

                // Логируем изменение подключения
                if (WriteLog) Log.EventSave(string.Format("Пользователь настроил новый репозиторий: {0} ({1})", NewRep.PrintConnectionString(), NewRep.PlugInType), "RepositoryFarm", EventEn.Message);
            }
            catch (Exception ex)
            {
                ApplicationException ae = new ApplicationException(string.Format("Упали при инициализации конструктора с ошибкой: ({0})", ex.Message));
                Log.EventSave(ae.Message, "RepositoryFarm.SetCurrentRepository", EventEn.Error);
                throw ae;
            }
        }

        /// <summary>
        /// Получаем список репозиториев
        /// </summary>
        /// <returns>Список доступных репозиториев</returns>
        public static List<PluginClassElement> GetListRepositoryName()
        {
            try
            {
                // Если список ещё не получали то получаем его
                if (ListRepositoryName == null)
                {
                    ListRepositoryName = new List<PluginClassElement>();

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
                                string Search = "RepositoryPlg.RepositoryI";
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
                                string Search = "RepositoryPlg.RepositoryBase";
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

                            ListRepositoryName.Add(new PluginClassElement(item.Name, item, null));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.EventSave(ex.Message, "RepositoryFarm.GetListRepositoryName", EventEn.Error);
            }

            return ListRepositoryName;
        }

        /// <summary>
        /// Создание репозитория определённого типа с параметрами из конфига
        /// </summary>
        /// <param name="PlugInType">Имя плагина определяющего тип репозитория который создаём</param>
        /// <param name="ConnectionString">Строка подключения к репозиторию</param>
        /// <returns>Возвращаем репозиторий</returns>
        public static Repository CreateNewRepository(string PlugInType, string ConnectionString)
        {
            Repository rez = null;
            try
            {
                // Если списка репозиториев ещё нет то создаём его
                GetListRepositoryName();


                // Проверяем наличие существование этого типа плагина
                foreach (PluginClassElement item in ListRepositoryName)
                {
                    if (item.Name == PlugInType.Trim())
                    {
                        // Создаём экземпляр объекта
                        object[] targ = { (string.IsNullOrWhiteSpace(ConnectionString) ? (object)null : ConnectionString) };
                        rez = (Repository)Activator.CreateInstance(item.EmptTyp, targ);

                        // Линкуем в базовый класс специальный скрытый интерфейс для того чтобы базовый класс мог что-то специфическое вызывать в дочернем объекте
                        //RepositoryPlg.Lib.RepositoryBase.CrossLink CrLink = new RepositoryPlg.Lib.RepositoryBase.CrossLink(rez);

                        break;
                    }
                }


            }
            catch (Exception ex)
            {
                Log.EventSave(ex.Message, "RepositoryFarm.CreateNewRepository", EventEn.Error);
            }

            return rez;
        }

        /// <summary>
        /// Создание репозитория определённого типа с параметрами из конфига
        /// </summary>
        /// <param name="PlugInType">Имя плагина определяющего тип репозитория который создаём</param>
        /// <returns>Возвращаем репозиторий</returns>
        public static Repository CreateNewRepository(string PlugInType)
        {
            Repository rez = null;
            try
            {
                // Если списка репозиториев ещё нет то создаём его
                GetListRepositoryName();


                // Проверяем наличие существование этого типа плагина
                foreach (PluginClassElement item in ListRepositoryName)
                {
                    if (item.Name == PlugInType.Trim())
                    {
                        // Создаём экземпляр объекта
                        //object[] targ = { (string.IsNullOrWhiteSpace(ConnectionString) ? (object)null : ConnectionString) };
                        rez = (Repository)Activator.CreateInstance(item.EmptTyp);//, targ);

                        // Линкуем в базовый класс специальный скрытый интерфейс для того чтобы базовый класс мог что-то специфическое вызывать в дочернем объекте
                        //RepositoryPlg.Lib.RepositoryBase.CrossLink CrLink = new RepositoryPlg.Lib.RepositoryBase.CrossLink(rez);

                        break;
                    }
                }


            }
            catch (Exception ex)
            {
                Log.EventSave(ex.Message, "RepositoryFarm.CreateNewRepository", EventEn.Error);
            }

            return rez;
        }

        /// <summary>
        /// Получение провайдера из репозитория
        /// </summary>
        /// <param name="Mon">Если True то хотим получить по мониторингу, если false то по базе объектов</param>
        /// <returns>Возвращаем провайдер</returns>
        public static Provider GetProvider(bool Mon)
        {
            Provider rez = null;
            try
            {
                if (CurRepository == null) throw new ApplicationException("Операцию выполнить не возможно. Не настроено подключение к репозиторию");

                rez = ((RepositoryPlg.RepositoryI)CurRepository).SelectProvider(Mon);
            }
            catch (Exception ex)
            {
                Log.EventSave(ex.Message, "RepositoryFarm.GetProvider", EventEn.Error);
                throw ex;
            }

            return rez;
        }

        /// <summary>
        /// Получение провайдера из репозитория
        /// </summary>
        /// <param name="Mon">Провайдер который необходимо сохранить</param>
        /// <param name="Mon">Если True то хотим получить по мониторингу, если false то по базе объектов</param>
        public static void SetProvider(Provider Prv, bool Mon)
        {
            try
            {
                if (CurRepository == null) throw new ApplicationException("Операцию выполнить не возможно. Не настроено подключение к репозиторию");

                ((RepositoryPlg.RepositoryI)CurRepository).SaveProvider(Prv, Mon);
            }
            catch (Exception ex)
            {
                Log.EventSave(ex.Message, "RepositoryFarm.SetProvider", EventEn.Error);
                throw ex;
            }
        }
    }
}
