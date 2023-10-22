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
    /// Ферма с типками переменных
    /// </summary>
    public class ParamFarm
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
        /// FolderName нашего плагина распологается вместе с основными классами так как чаще используется с ними
        /// </summary>
        public static string FolderForPlugin
        {
            get { return "IoPlg"; }
            private set { }
        }

        /// <summary>
        /// Путь к папке с плагинами нашего плагина
        /// </summary>
        public static string PathForPlugin
        {
            get { return string.Format("{0}\\IoPlg", ParentDirectoryForPlugin); }
            private set { }
        }

        /// <summary>
        /// Получаем список наших плагинов
        /// </summary>
        /// <returns>Список имён доступных плагинов</returns>
        public static List<PluginClassElementList> ListParamName = null;

        /// <summary>
        /// Конструктор
        /// </summary>
        public ParamFarm()
        {
            try
            {
                if (ListParamName == null)
                {
                    Log.EventSave("Инициализация классов наших плагинов", GetType().Name, EventEn.Message);
                    GetListParamName();

                    // Установка текущего репозитория по умолчанию
                    //string dd = ListRepositoryName.Find(x => x == Config.RepositoryName);
                    //if (!string.IsNullOrWhiteSpace(dd)) CurRepository = CreateNewRepository("DisplayDSP840");
                }
            }
            catch (Exception ex)
            {
                ApplicationException ae = new ApplicationException(string.Format("Упали при инициализации конструктора с ошибкой: ({0})", ex.Message));
                Log.EventSave(ae.Message, string.Format("{0}.ParamFarm", this.GetType().FullName), EventEn.Error);
                throw ae;
            }
        }


        /// <summary>
        /// Получаем список плагинов
        /// </summary>
        /// <returns>Список доступных репозиториев</returns>
        public static List<PluginClassElementList> GetListParamName()
        {
            try
            {
                // Если список ещё не получали то получаем его
                if (ListParamName == null)
                {
                    ListParamName = new List<PluginClassElementList>();

                    // Проверяем наличие папки и если её нет то создаём её
                    if (!Directory.Exists(PathForPlugin)) Directory.CreateDirectory(PathForPlugin);
                    //
                    // Пробегаем по папке и подгружаем все плагины из неё
                    foreach (string filePath in Directory.GetFiles(PathForPlugin, "*.dll"))
                    {
                        Assembly asm = Assembly.LoadFrom(filePath);
                        //Type[] typelist = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.Namespace == "AlgoritmPrizm.Com.DisplayPlg").ToArray();
                        Type[] typelist = asm.GetTypes();//.Where(t => t.Namespace == "AlgoritmPrizm.Com.DisplayPlg").ToArray();

                        // Создаём список с описанием файла где нашли плагин
                        PluginClassElementList nIoList = new PluginClassElementList(FolderForPlugin, Path.GetFileName(filePath));

                        foreach (Type item in typelist)
                        {
                            // Проверяем реализовывает ли класс наш интерфейс если да то это провайдер который можно подкрузить
                            bool flagISql = false;

                            foreach (Type i in item.GetInterfaces())
                            {
                                string Search = "ParamPlg.ParamTransferSqlI";
                                if (i.FullName.Length > Search.Length && i.FullName.Substring(i.FullName.Length - Search.Length) == Search)
                                {
                                    flagISql = true;
                                    break;
                                }
                            }
                            if (!flagISql) continue;

                            // Проверяем что наш клас наследует PlugInBase 
                            bool flagB = false;
                            foreach (MemberInfo mi in item.GetMembers())
                            {
                                string Search = "ParamPlg.ParamBase";
                                if (mi.DeclaringType.FullName.Length > Search.Length && mi.DeclaringType.FullName.Substring(mi.DeclaringType.FullName.Length - Search.Length) == Search)
                                {
                                    flagB = true;
                                    break;
                                }
                            }
                            if (!flagB) continue;

                            // Проверяем конструктор нашего класса  
                            //bool flag1 = false;
                            bool flag0 = false;
                            string nameConstructor;

                            foreach (ConstructorInfo ctor in item.GetConstructors())
                            {
                                nameConstructor = item.Name;

                                // получаем параметры конструктора  
                                ParameterInfo[] parameters = ctor.GetParameters();

                                /*
                                // если в этом конструктаре должно быть несколько параметров
                                if (parameters.Length == 1)
                                {
                                    bool flag = true;
                                    if (parameters[0].ParameterType.Name != "String" || parameters[0].Name != "ConnectionString") flag = false;

                                    flag1 = flag;
                                }
                                */

                                // Проверяем конструктор для создания документа пустого по умолчанию
                                if (parameters.Length == 0) flag0 = true;
                            }
                            //if (!flag1) continue;
                            if (!flag0) continue;

                            // Создаём описание нашего класса
                            nIoList.Items.Add(new PluginClassElement(item.Name, item, nIoList));
                        }

                        // Если классы обнаружены то добавляем в наш список нужнное описание источника со своими классами
                        if (nIoList.Items.Count > 0)
                        {
                            ListParamName.Add(nIoList);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.EventSave(ex.Message, "IoFarm.GetListIoName", EventEn.Error);
            }

            return ListParamName;
        }

        /// <summary>
        /// Создание репозитория определённого типа с параметрами из конфига
        /// </summary>
        /// <param name="Plg">Описание плагина который хотим создать</param>
        /// <returns>Возвращаем плагин</returns>
        public static Param CreateNewParam(PluginClassElement Plg)
        {
            Param rez = null;
            try
            {
                // Если списка репозиториев ещё нет то создаём его
                GetListParamName();

                if (Plg != null)
                {
                    // Создаём экземпляр объекта
                    //object[] targ = { (string.IsNullOrWhiteSpace(ConnectionString) ? (object)null : ConnectionString) };
                    rez = (Param)Activator.CreateInstance(Plg.EmptTyp/*, targ*/);

                    rez.FileInfo = Plg.FileInfo;
                    rez.ElementDll = Plg;

                    // Линкуем в базовый класс специальный скрытый интерфейс для того чтобы базовый класс мог что-то специфическое вызывать в дочернем объекте
                    ParamPlg.ParamBase.CrossLink CrLink = new ParamPlg.ParamBase.CrossLink(rez);
                }
            }
            catch (Exception ex)
            {
                Log.EventSave(ex.Message, "RepositoryFarm.CreateNewParam", EventEn.Error);
            }

            return rez;
        }

        /// <summary>
        /// Создание репозитория определённого типа с параметрами из конфига
        /// </summary>
        /// <param name="PlugInType">Имя плагина определяющего тип который создаём</param>
        /// <returns>Возвращаем плагин</returns>
        public static Param CreateNewParam(string PlugInType)
        {
            Param rez = null;
            try
            {
                // Если списка репозиториев ещё нет то создаём его
                GetListParamName();

                foreach (PluginClassElementList item in ListParamName)
                {
                    string FileName = "IoSystem";
                    string Plugin = PlugInType;
                    if (PlugInType.IndexOf(".") > 0)
                    {
                        FileName = PlugInType.Substring(0, PlugInType.IndexOf("."));
                    //    PlugInType = PlugInType.Substring(PlugInType.IndexOf(".")+1);
                    }

                    if (item.PluginFileName.Replace(".dll", "") == FileName)
                    {
                        PluginClassElement Plg = item.GetPlgForName(PlugInType);
                        rez=CreateNewParam(Plg);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.EventSave(ex.Message, "RepositoryFarm.CreateNewParam", EventEn.Error);
            }

            return rez;
        }

    }
}
