using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Reflection;
using System.Linq;
using System.IO;

namespace Common
{
    /// <summary>
    /// Класс для работы с кастомизацияами для разных организаций
    /// </summary>
    public class CustomizationFarm
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
            get { return "CustomizationPlg"; }
            private set { }
        }

        /// <summary>
        /// Путь к папке с плагинами нашего плагина
        /// </summary>
        public static string PathForPlugin
        {
            get { return string.Format("{0}\\CustomizationPlg", ParentDirectoryForPlugin); }
            private set { }
        }

        /// <summary>
        /// Получаем список доступных кастомизаций
        /// </summary>
        /// <returns>Список имён доступных дисплеев</returns>
        public static List<PluginClassElement> ListCustomizationName;

        /// <summary>
        /// Текущая кастомизация
        /// </summary>
        public static Customization CurCustomization;

        /// <summary>
        /// Конструктор
        /// </summary>
        public CustomizationFarm()
        {
            try
            {
                if (ListCustomizationName == null)
                {
                    Log.EventSave("Инициализация классов катомизации", GetType().Name, EventEn.Message);
                    GetListCustomizationName();

                    // Установка текущей кастомизации по умолчанию
                    //string dd = ListCustomizationName.Find(x => x == Config.CustomizationName);
                    //if (!string.IsNullOrWhiteSpace(dd)) CurCustomization = CreateNewCustomization("DisplayDSP840");
                }
            }
            catch (Exception ex)
            {
                ApplicationException ae = new ApplicationException(string.Format("Упали при инициализации конструктора с ошибкой: ({0})", ex.Message));
                Log.EventSave(ae.Message, string.Format("{0}.CustomizationFarm", this.GetType().FullName), EventEn.Error);
                throw ae;
            }
        }

        /// <summary>
        /// Установка текущей кастомизации
        /// </summary>
        /// <param name="NewCst">Кастомизация которую нужно установить по умолчанию</param>
        public static void SetCurrentCustomization(Customization NewCst)
        {
            try
            {
                CurCustomization = NewCst;
            }
            catch (Exception ex)
            {
                ApplicationException ae = new ApplicationException(string.Format("Упали при инициализации конструктора с ошибкой: ({0})", ex.Message));
                Log.EventSave(ae.Message, "CustomizationFarm.SetCurrentCustomization", EventEn.Error);
                throw ae;
            }
        }

        /// <summary>
        /// Получаем список кастомизаций
        /// </summary>
        /// <returns>Список доступных кастомизаций</returns>
        public static List<PluginClassElement> GetListCustomizationName()
        {
            try
            {
                // Если список ещё не получали то получаем его
                if (ListCustomizationName == null)
                {
                    ListCustomizationName = new List<PluginClassElement>();

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
                            // Проверяем реализовывает ли класс наш интерфейс если да то это провайдер который можно подкрузить
                            bool flagI = false;

                            foreach (Type i in item.GetInterfaces())
                            {
                                string Search = "CustomizationPlg.CustomizationI";
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
                                string Search = "CustomizationPlg.CustomizationBase";
                                if (mi.DeclaringType.FullName.Length > Search.Length && mi.DeclaringType.FullName.Substring(mi.DeclaringType.FullName.Length - Search.Length) == Search)
                                {
                                    flagB = true;
                                    break;
                                }
                            }
                            if (!flagB) continue;

                            // Проверяем конструктор нашего класса  
                            //bool flag5 = false;
                            bool flag0 = false;
                            string nameConstructor;
                            foreach (ConstructorInfo ctor in item.GetConstructors())
                            {
                                nameConstructor = item.Name;

                                // получаем параметры конструктора  
                                ParameterInfo[] parameters = ctor.GetParameters();

                                /*
                                // если в этом конструктаре 11 параметров то проверяем тип и имя параметра 
                                if (parameters.Length == 5)
                                {
                                    bool flag5 = true;
                                    if (parameters[0].ParameterType.Name != "Int32" || parameters[0].Name != "Port") flag = false;
                                    if (parameters[1].ParameterType.Name != "Int32" || parameters[1].Name != "BaudRate") flag = false;
                                    if (parameters[2].ParameterType.Name != "Parity" || parameters[2].Name != "Parity") flag = false;
                                    if (parameters[3].ParameterType.Name != "Int32" || parameters[3].Name != "DataBits") flag = false;
                                    if (parameters[4].ParameterType.Name != "StopBits" || parameters[4].Name != "StpBits") flag = false;
                                }
                                */

                                // Проверяем конструктор для создания документа пустого по умолчанию
                                if (parameters.Length == 0) flag0 = true;
                            }
                            //if (!flag5) continue;
                            if (!flag0) continue;

                            ListCustomizationName.Add(new PluginClassElement(item.Name, item, null));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.EventSave(ex.Message, "CustomizationFarm.GetListCustomizationName", EventEn.Error);
            }

            return ListCustomizationName;
        }

        /// <summary>
        /// Создание катомизации определённого типа с параметрами из конфига
        /// </summary>
        /// <param name="PlugInType">Имя плагина определяющего тип кастомизации который создаём</param>
        /// <returns>Возвращаем кастомизацию</returns>
        public static Customization CreateNewCustomization(string PlugInType)
        {
            Customization rez = null;
            try
            {
                // Если списка кастомизаций ещё нет то создаём его
                GetListCustomizationName();


                // Проверяем наличие существование этого типа плагина
                foreach (PluginClassElement item in ListCustomizationName)
                {
                    if (item.Name == PlugInType.Trim())
                    {
                        // Создаём экземпляр объекта
                        //object[] targ = { (string.IsNullOrWhiteSpace(ConnectionString) ? (object)null : ConnectionString) };
                        rez = (Customization)Activator.CreateInstance(item.EmptTyp);//, targ);

                        // Линкуем в базовый класс специальный скрытый интерфейс для того чтобы базовый класс мог что-то специфическое вызывать в дочернем объекте
                        //RepositoryPlg.Lib.RepositoryBase.CrossLink CrLink = new RepositoryPlg.Lib.RepositoryBase.CrossLink(rez);

                        break;
                    }
                }


            }
            catch (Exception ex)
            {
                Log.EventSave(ex.Message, "CustomizationFarm.CreateNewCustomization", EventEn.Error);
            }

            return rez;
        }
    }
}
