using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml;
using System.IO;

namespace Common
{
    /// <summary>
    /// Класс для работы с конфигурационным фалом
    /// </summary>
    public class Config
    {
        #region Private Param
        private static Config obj = null;

        /// <summary>
        /// Объект XML файла
        /// </summary>
        private static XmlDocument Document = new XmlDocument();

        /// <summary>
        /// Корневой элемент нашего документа
        /// </summary>
        private static XmlElement xmlRoot;

        /// <summary>
        /// Версия XML файла
        /// </summary>
        private static int _Version = 1;

        /// <summary>
        /// Флаг трассировки
        /// </summary>
        private static bool _Trace = false;

        /// <summary>
        /// Отображение всплывающих подсказок
        /// </summary>
        private static bool _ShowNotification = true;

        /// <summary>
        /// Корневой элемент плагинов
        /// </summary>
        private static XmlElement xmlCustomizationPlg;

        /// <summary>
        /// Плагин кастомизации по умолчанию
        /// </summary>
        private static string _DefCustPlg=string.Empty;
        #endregion

        #region Public Param

        /// <summary>
        /// Файл в котором мы храним конфиг
        /// </summary>
        public static string FileXml { get; private set; }

        /// <summary>
        /// Версия XML файла
        /// </summary>
        public static int Version { get { return _Version; } private set { } }

        /// <summary>
        /// Флаг трассировки
        /// </summary>
        public static bool Trace
        {
            get
            {
                return _Trace;
            }
            set
            {
                xmlRoot.SetAttribute("Trace", value.ToString());
                Save();
                _Trace = value;
            }
        }

        /// <summary>
        /// Отображение всплывающих подсказок
        /// </summary>
        public static bool ShowNotification
        {
            get
            {
                return _ShowNotification;
            }
            set
            {
                xmlRoot.SetAttribute("ShowNotification", value.ToString());
                Save();
                _ShowNotification = value;
            }
        }

        /// <summary>
        /// Плагин кастомизации по умолчанию
        /// </summary>
        public static string DefCustPlg
        {
            get
            {
                return _DefCustPlg;
            }
            set
            {
                xmlRoot.SetAttribute("DefCustPlg", value);
                Save();
                _DefCustPlg = value;
            }
        }

        /*
        /// <summary>
        /// Порт дисплея покупателя
        /// </summary>
        public static int DisplayPort
        {
            get
            {
                return _DisplayPort;
            }
            set
            {
                xmlRoot.SetAttribute("DisplayPort", value.ToString());
                Save();
                _DisplayPort = value;
            }
        }
        */
        #endregion

        #region Puplic Method

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="FileConfig"></param>
        public Config(string FileConfig)
        {
            try
            {
                if (obj == null) FileXml = "Reminder.xml";
                else FileXml = FileConfig;

                obj = this;
                Log.EventSave("Чтение конфигурационного файла", GetType().Name, EventEn.Message);

                // Читаем файл или создаём
                if (File.Exists(Environment.CurrentDirectory + @"\" + FileXml)) { Load(); }
                else { Create(); }

                // Получаем кастомизированный объект
                GetDate();

                // Подписываемся на события
                //Com.Lic.onCreatedLicKey += new EventHandler<LicLib.onLicEventKey>(Lic_onCreatedLicKey);
            }
            catch (Exception ex)
            {
                ApplicationException ae = new ApplicationException(string.Format("Упали при загрузке конфигурации с ошибкой: {0}", ex.Message));
                Log.EventSave(ae.Message, string.Format("{0}.Config", this.GetType().FullName), EventEn.Error);
                throw ae;
            }
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="FileConfig"></param>
        public Config()
            : this(null)
        {
        }


        /// <summary>
        /// Создание нового списка плагинов
        /// </summary>
        /// <param name="NewPluginClassElement">Новый список который надо сохранить</param>
        public static void SetPluginClassElement(List<PluginClassElement> NewPluginClassElement)
        {
            try
            {
                // Если корневого элемента нет создаём его
                if (xmlCustomizationPlg == null)
                {
                    //xmlRoot.RemoveChild(xmlPlg);
                    xmlCustomizationPlg = Document.CreateElement("CustomizationPlg");
                    xmlRoot.AppendChild(xmlCustomizationPlg);
                }

                XmlNodeList itemList = xmlCustomizationPlg.ChildNodes;
                int itemListCount = itemList.Count - 1;
                for (int i = itemListCount; i >= 0; i--)
                {
                    xmlCustomizationPlg.RemoveChild(itemList[i]);
                }

                foreach (PluginClassElement item in NewPluginClassElement)
                {
                    XmlElement xmlPlgNew = Document.CreateElement(item.Name);
                    //xmlPlgNew.SetAttribute("Login", item.login);
                    xmlCustomizationPlg.AppendChild(xmlPlgNew);
                }

                Save();

                //_customers = NewCustumers;
            }
            catch (Exception ex)
            {
                ApplicationException ae = new ApplicationException(string.Format("Упали при сохранении нового списка плагнов кастомизации с ошибкой: {0}", ex.Message));
                Log.EventSave(ae.Message, "Config.SetPluginClassElement", EventEn.Error);
                throw ae;
            }
        }
        
        #endregion

        #region Private Method

        /// <summary>
        /// Читеам файл конфигурации
        /// </summary>
        private static void Load()
        {
            try
            {
                lock (obj)
                {
                    Document.Load(Environment.CurrentDirectory + @"\" + FileXml);
                }
            }
            catch (Exception ex)
            {
                ApplicationException ae = new ApplicationException(string.Format("Упали при загрузке конфигурации с ошибкой: {0}", ex.Message));
                Log.EventSave(ae.Message, "Config.Load()", EventEn.Error);
                throw ae;
            }
        }

        /// <summary>
        /// Сохраняем конфигурацию  в файл
        /// </summary>
        private static void Save()
        {
            try
            {
                lock (obj)
                {
                    Document.Save(Environment.CurrentDirectory + @"\" + FileXml);
                }
            }
            catch (Exception ex)
            {
                ApplicationException ae = new ApplicationException(string.Format("Упали при сохранении конфигурации в файл с ошибкой: {0}", ex.Message));
                Log.EventSave(ae.Message, "Config.Save()", EventEn.Error);
                throw ae;
            }
        }

        /// <summary>
        /// Создание нового файла
        /// </summary>
        private static void Create()
        {
            try
            {
                lock (obj)
                {
                    // создаём строку инициализации
                    XmlElement wbRoot = Document.DocumentElement;
                    XmlDeclaration wbxmdecl = Document.CreateXmlDeclaration("1.0", "UTF-8", "yes");
                    Document.InsertBefore(wbxmdecl, wbRoot);

                    // Создаём начальное тело с которым мы будем потом работать
                    XmlElement xmlMain = Document.CreateElement("Reminder");
                    xmlMain.SetAttribute("Version", _Version.ToString());
                    xmlMain.SetAttribute("Trace", _Trace.ToString());
                    xmlMain.SetAttribute("ShowNotification", _ShowNotification.ToString());
                    Document.AppendChild(xmlMain);

                    // Создаём список в который будем помещать элементы с кастомизацией
                    XmlElement xmlCustomizationPlg = Document.CreateElement("CustomizationPlg");
                    xmlCustomizationPlg.SetAttribute("DefCustPlg", _DefCustPlg.ToString());
                    xmlMain.AppendChild(xmlCustomizationPlg);
                    //XmlElement xmlCustomerTest = Document.CreateElement("Customer");
                    //xmlCustomerTest.SetAttribute("Login", "sysadmin");
                    //xmlCustomerTest.SetAttribute("Fio", "Сидоров Иван Петрович");
                    //xmlCustomerTest.SetAttribute("INN", "1234567890");
                    //xmlCustomers.AppendChild(xmlCustomerTest);

                    // Сохраняем документ
                    Save();
                }
            }
            catch (Exception ex)
            {
                ApplicationException ae = new ApplicationException(string.Format("Упали при соpдании конфигурационного файла с ошибкой: {0}", ex.Message));
                Log.EventSave(ae.Message, "Config.Create()", EventEn.Error);
                throw ae;
            }
        }

        /// <summary>
        /// Получение кастомизированного запроса
        /// </summary>
        private static void GetDate()
        {
            ApplicationException appM = new ApplicationException("Неправильный настроечный файл, скорее всего не от этой программы.");
            ApplicationException appV = new ApplicationException(string.Format("Неправильная версия настроечного яайла, требуется {0} версия", _Version));
            try
            {
                lock (obj)
                {
                    xmlRoot = Document.DocumentElement;

                    // Проверяем значения заголовка
                    if (xmlRoot.Name != "Reminder") throw appM;
                    if (Version < int.Parse(xmlRoot.GetAttribute("Version"))) throw appV;
                    if (Version > int.Parse(xmlRoot.GetAttribute("Version"))) UpdateVersionXml(xmlRoot, int.Parse(xmlRoot.GetAttribute("Version")));

                    // Получаем значения из заголовка
                    for (int i = 0; i < xmlRoot.Attributes.Count; i++)
                    {
                        if (xmlRoot.Attributes[i].Name == "Trace") try { _Trace = bool.Parse(xmlRoot.Attributes[i].Value.ToString()); } catch (Exception) { }
                        if (xmlRoot.Attributes[i].Name == "ShowNotification") try { _ShowNotification = bool.Parse(xmlRoot.Attributes[i].Value.ToString()); } catch (Exception) { }
                    }

                    // Получаем список вложенных объектов
                    foreach (XmlElement iMain in xmlRoot.ChildNodes)
                    {
                        switch (iMain.Name)
                        {
                            case "CustomizationPlg":
                                xmlCustomizationPlg = iMain;

                                // Получаем значения из заголовка
                                for (int i = 0; i < iMain.Attributes.Count; i++)
                                {
                                    if (iMain.Attributes[i].Name == "DefCustPlg") try { _DefCustPlg = iMain.Attributes[i].Value.ToString(); } catch (Exception) { }

                                }

                                // Получаем список вложенных объектов 
                                foreach (XmlElement iMainItems in iMain.ChildNodes)
                                {
                                    switch (iMainItems.Name)
                                    {
                                        /*
                                        case "Customer":

                                            string login = null;
                                            string fio = null;
                                            string Job = null;
                                            string inn = null;

                                            // Получаем значения из заголовка
                                            for (int i = 0; i < iCustomers.Attributes.Count; i++)
                                            {
                                                if (iCustomers.Attributes[i].Name == "Login") try { login = iCustomers.Attributes[i].Value.ToString(); } catch (Exception) { }
                                                if (iCustomers.Attributes[i].Name == "Fio") try { fio = iCustomers.Attributes[i].Value.ToString(); } catch (Exception) { }
                                                if (iCustomers.Attributes[i].Name == "Job") try { Job = iCustomers.Attributes[i].Value.ToString(); } catch (Exception) { }
                                                if (iCustomers.Attributes[i].Name == "INN") try { inn = iCustomers.Attributes[i].Value.ToString(); } catch (Exception) { }

                                            }

                                            if (!string.IsNullOrWhiteSpace(login) && !string.IsNullOrWhiteSpace(fio) && !string.IsNullOrWhiteSpace(inn))
                                            {
                                                bool HashFlagCustomer = true;
                                                foreach (Custumer itemCustumerF in _customers)
                                                {
                                                    if (itemCustumerF.login == login)
                                                    {
                                                        HashFlagCustomer = false;
                                                        break;
                                                    }
                                                }

                                                if (HashFlagCustomer) _customers.Add(new Custumer(login, fio, Job, inn));
                                            }

                                            break;
                                        */
                                        default:
                                            break;
                                    }
                                }
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationException ae = new ApplicationException(string.Format("Упали при разборе конфигурационного файла с ошибкой: {0}", ex.Message));
                Log.EventSave(ae.Message, "Config.GetDate()", EventEn.Error);
                throw ae;
            }
        }

        /// <summary>
        /// Обновление до нужной версии
        /// </summary>
        /// <param name="root">Корневой элемент</param>
        /// <param name="oldVersion">Версия файла из конфига</param>
        private static void UpdateVersionXml(XmlElement root, int oldVersion)
        {
            try
            {
                if (oldVersion <= 2)
                {

                }

                root.SetAttribute("Version", _Version.ToString());
                Save();
            }
            catch (Exception ex)
            {
                ApplicationException ae = new ApplicationException(string.Format("Упали при обновлении конфигурационного в файла с ошибкой: {0}}", ex.Message));
                Log.EventSave(ae.Message, "Config.UpdateVersionXml(XmlElement root, int oldVersion)", EventEn.Error);
                throw ae;
            }
        }

        /*
        // Событие изменения текщего провайдера
        private void ProviderFarm_onEventSetup(object sender, EventProviderFarm e)
        {
            try
            {
                XmlElement root = Document.DocumentElement;

                root.SetAttribute("PrvFullName", e.Uprv.PrvInType);
                try { root.SetAttribute("ConnectionString", Com.Lic.InCode(e.Uprv.ConnectionString)); }
                catch (Exception) { }



                // Получаем список объектов
                //foreach (XmlElement item in root.ChildNodes)
                //{
                //}

                Save();
            }
            catch (Exception ex)
            {
                ApplicationException ae = new ApplicationException("Упали при изменении файла конфигурации с ошибкой: " + ex.Message);
                Log.EventSave(ae.Message, obj.GetType().Name + ".ProviderFarm_onEventSetup()", EventEn.Error);
                throw ae;
            }
        }
        */

        #endregion
    }
}
