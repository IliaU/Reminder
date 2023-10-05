using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Common;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using System.Threading;

namespace Reminder
{
    public partial class FStart : Form
    {
        private Color DefBaskCoclortSSLabel;
        private object LockObj = new object();

        private NotifyIcon m_notifyicon;
        private ContextMenu m_menu;

        private Size WindowSize = SystemInformation.PrimaryMonitorMaximizedWindowSize;
        private bool isClosed = false;

        private Point FLocation;
        private Size FSize;
        private Boolean DoubleClickIsShow;

        // В каком статусе настройка блокировки настроечного меню
        //private bool GonfigBlock = true;
        //private bool IsRunAsinGonfigBlock = false;
        //private Thread ThrGonfigBlock;
        //private DateTime GonfigBlockDatetime = DateTime.Now;

        // Экспортируем неуправляемую библиотеку и создаём переменную и константы для работы нашей библиотеки
        private const int NotifyForThisSession = 0;
        private const int SessionChangeMessage = 0x02B1;
        private const int SessionLockParam = 0x7;
        private const int SessionUnlockParam = 0x8;
        //
        [DllImport("wtsapi32.dll")]
        private static extern bool WTSRegisterSessionNotification(IntPtr hWnd, int dwFlags);
        //
        [DllImport("wtsapi32.dll")]
        private static extern bool WTSUnRegisterSessionNotification(IntPtr hWnd);
        //
        private bool registered = false;            // Состояние блокировки   вместо переменной в форме стал использовать переменную в классе this.com.HashLock

        /// <summary>
        /// Для того чтобы статус в нижней части работал последлвательно
        /// </summary>
        private object LockEventLog = new object();

        /// <summary>
        /// Конструктор
        /// </summary>
        public FStart()
        {
            try
            {
                InitializeComponent();
                this.DefBaskCoclortSSLabel = this.tSSLabel.BackColor;
                this.Text = CustomizationFarm.CurCustomization.FStartText;
                this.PicStatRepOnline.Image = CustomizationFarm.CurCustomization.GetIconDbStatus(true);
                this.PicStatRepOfline.Image = CustomizationFarm.CurCustomization.GetIconDbStatus(false);

                m_menu = new ContextMenu();
                m_menu.MenuItems.Add(0, new MenuItem("Show", new System.EventHandler(Show_Click)));
                m_menu.MenuItems.Add(1, new MenuItem("Hide", new System.EventHandler(Hide_Click)));
                m_menu.MenuItems.Add(2, new MenuItem("Exit", new System.EventHandler(Exit_Click)));
                //m_menu.MenuItems.Add(3, new MenuItem("Тест всплывающего окна", new System.EventHandler(Test_Click)));
                //
                m_notifyicon = new NotifyIcon();
                m_notifyicon.Text = "Щёлкните правой кнопкой мыши для вызова контекстного меню.";
                m_notifyicon.Visible = true;
                m_notifyicon.Icon = CustomizationFarm.CurCustomization.GetIconStatus(EventEn.Message);
                m_notifyicon.ContextMenu = m_menu;
                m_notifyicon.DoubleClick += new EventHandler(DoubleClickIcon);

                // Подписываемся на события 
                Log.onEventLog += Log_onEventLog;                                           // Лог программы то что пишем в лог
                ProgramStatus.onEventChangeStatus += ProgramStatus_onEventChangeStatus;     // Изменение статуса системы для того чтобы поменять иконку в трее и в формах

                // Сообщаем о успешной загрузке приложения и меняем иконки
                Log.EventSave("Программа загружена", this.GetType().FullName, EventEn.Message, true, false);
            }
            catch (Exception ex)
            {
                ApplicationException ae = new ApplicationException(string.Format("Упали при инициализации конструктора с ошибкой: ({0})", ex.Message));
                Log.EventSave(ae.Message, string.Format("{0}.FStart", this.GetType().FullName), EventEn.Error);
                throw ae;
            }
        }

        /// <summary>
        /// Подписываемся на изменение иконок в форме
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProgramStatus_onEventChangeStatus(object sender, EventChangeStatus e)
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Отображает скрытую форму
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Show_Click(Object sender, System.EventArgs e)
        {
            try
            {
                // Опять отображаем форму
                Show();

                // Разворачиваем форму и востанавливаем размер и положение
                this.WindowState = FormWindowState.Normal;
                this.Location = (this.FLocation.X > 0 && this.FLocation.Y > 0 ? this.FLocation : this.Location);
                this.Size = (this.FSize.Height > 50 && this.FSize.Width > 200 ? this.FSize : this.Size);
                this.DoubleClickIsShow = true;

                // Иногда исчезает подписка на двойной клик. Если вдруг исчезла, то подписываемся снова
                m_notifyicon.DoubleClick -= new EventHandler(DoubleClickIcon);
                m_notifyicon.DoubleClick += new EventHandler(DoubleClickIcon);
            }
            catch (Exception ex)
            {
                ApplicationException ae = new ApplicationException(string.Format("Упали при попытке отобразить форму с ошибкой: ({0})", ex.Message));
                Log.EventSave(ae.Message, string.Format("{0}.Show_Click", this.GetType().FullName), EventEn.Error);
                //throw ae;
            }
        }

        /// <summary>
        /// Скрывает форму
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Hide_Click(Object sender, System.EventArgs e)
        {
            try
            {
                // Запоминаем положение и минимизируем форму
                this.FLocation = (this.Location.X > 0 && this.Location.Y > 0 ? this.Location : this.FLocation);
                this.FSize = (this.Size.Height > 50 && this.Size.Width > 200 ? this.Size : this.FSize);
                this.WindowState = FormWindowState.Minimized;
                this.DoubleClickIsShow = false;

                // Скрываем нашу форму
                Hide();

                // Переподписываемся на новое событие
                //m_notifyicon.DoubleClick -= new EventHandler(Hide_Click);
                //m_notifyicon.DoubleClick += new EventHandler(Show_Click);
            }
            catch (Exception ex)
            {
                ApplicationException ae = new ApplicationException(string.Format("Упали при попытке скрыть форму с ошибкой: ({0})", ex.Message));
                Log.EventSave(ae.Message, string.Format("{0}.Show_Click", this.GetType().FullName), EventEn.Error);
                //throw ae;
            }
        }

        /// <summary>
        /// Закрывает форму
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Exit_Click(Object sender, System.EventArgs e)
        {
            try
            {
                //this.IsRunAsinGonfigBlock = false;
                isClosed = true;
                this.m_notifyicon.Dispose();
                Close();
            }
            catch (Exception ex)
            {
                ApplicationException ae = new ApplicationException(string.Format("Упали при попытке закрыть форму с ошибкой: ({0})", ex.Message));
                Log.EventSave(ae.Message, string.Format("{0}.Exit_Click", this.GetType().FullName), EventEn.Error);
                //throw ae;
            }
        }

        /// <summary>
        /// Пользователь вызвал двойной клик по иконке
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DoubleClickIcon(Object sender, System.EventArgs e)
        {
            try
            {
                lock (this)
                {
                    if (!this.DoubleClickIsShow)
                    {
                        this.Show_Click(sender, e);
                        this.DoubleClickIsShow = true;
                    }
                    else
                    {
                        this.Hide_Click(sender, e);
                        this.DoubleClickIsShow = false;
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationException ae = new ApplicationException(string.Format("Упали при попытке обработки двойного нажатия в трее с ошибкой: ({0})", ex.Message));
                Log.EventSave(ae.Message, string.Format("{0}.DoubleClickIcon", this.GetType().FullName), EventEn.Error);
                //throw ae;
            }
        }

        /// <summary>
        /// Тест всплывающего уведомления
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Test_Click(Object sender, System.EventArgs e)
        {   // А вот так можно вызвать стандартное уведомление
            this.m_notifyicon.ShowBalloonTip(5000, "Заголовок", "Текст\r\n gggg\r\n jjjjjjjjjjjjj", ToolTipIcon.Info);

            //F_Message frm = new F_Message(this.com);
            //frm.Show();
            //frm.Location = new System.Drawing.Point(WindowSize.Width - frm.Size.Width-5, WindowSize.Height - frm.Size.Height - 10);

            m_notifyicon.Icon = new Icon(GetType(), "IconE.ico");
        }

        // Отписываемся от события в внешней dll при закрытии формы
        private void MyDispose(bool disposing)      // ОСОБЕННОСТЬ Добавил вызов этого метода в конструкторе который ведёт сама студия F_Start_Designer.cs
        {
            // Надеюсь поможет от появления нескольких экземпляров этого приложения
            try
            {
                m_notifyicon.DoubleClick -= new EventHandler(DoubleClickIcon);
                //this.com.onEventStatus -= new EventHandler<Lib.EventStatus>(com_onEventStatus);
                //this.com.onEventStatusTime -= new EventHandler<Lib.EventStatus>(com_onEventStatusTime);

                WTSUnRegisterSessionNotification(Handle); // Handle -- Дескриптор текущего окна
                registered = false;
            }
            catch (Exception) { }

            if (registered)
            {
                WTSUnRegisterSessionNotification(Handle); // Handle -- Дескриптор текущего окна
                registered = false;
            }

            return;
        }

        // Переписываем метод подписания на системные события тем самым получаем событие из внешней библиотеки WndProc
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            // WtsRegisterSessionNotification requires Windows XP or higher           (На старых виндах это не работает)
            bool haveXp = Environment.OSVersion.Platform == PlatformID.Win32NT &&
                                (Environment.OSVersion.Version.Major > 5 ||
                                    (Environment.OSVersion.Version.Major == 5 &&
                                     Environment.OSVersion.Version.Minor >= 1));

            if (haveXp)
                registered = WTSRegisterSessionNotification(Handle, NotifyForThisSession);      // Собственно подписываемся на событие

            return;
        }

        // Событие из внешней библиотеки
        protected override void WndProc(ref Message m)
        {
            // check for session change notifications   (Сравниваем сообщение пришедшее из внешней dll с нашей константой)
            if (m.Msg == SessionChangeMessage)
            {
                if (m.WParam.ToInt32() == SessionLockParam)
                    OnSessionLock();
                else if (m.WParam.ToInt32() == SessionUnlockParam)
                    OnSessionUnlock();
            }

            base.WndProc(ref m);                        // Отдаём управление внешней dll
            return;
        }

        // Произошла блокировка компа
        protected virtual void OnSessionLock()
        {
            //Log.EventSave("Компьютер заблокирован", this.ToString(), EventEn.Message);

            //ProgramStatus.   .onEventStatus -= new EventHandler<Lib.EventStatus>(com_onEventStatus);
            //this.com.onEventStatusTime -= new EventHandler<Lib.EventStatus>(com_onEventStatusTime);

            ProgramStatus.onEventHashLockActivation(true);

            return;
        }

        // Комп разблокирован
        protected virtual void OnSessionUnlock()
        {
            // Log.EventSave("Компьютер разблокирован", this.ToString(), EventEn.Message);
            // Иногда исчезает подписка на двойной клик. Если вдруг исчезла, то подписываемся снова
            m_notifyicon.DoubleClick -= new EventHandler(DoubleClickIcon);
            m_notifyicon.DoubleClick += new EventHandler(DoubleClickIcon);
            // this.com.onEventStatus -= new EventHandler<Lib.EventStatus>(com_onEventStatus);
            //  this.com.onEventStatus += new EventHandler<Lib.EventStatus>(com_onEventStatus);
            // this.com.onEventStatusTime -= new EventHandler<Lib.EventStatus>(com_onEventStatusTime);
            //  this.com.onEventStatusTime += new EventHandler<Lib.EventStatus>(com_onEventStatusTime);
            ProgramStatus.onEventHashLockActivation(false);
            //  this.com.RefreshEvent = true;       // принудительно заставляем перечитать таблицу с результатами
            return;
        }

        /// <summary>
        /// Произошло событие системное правим текущий статус
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        delegate void delig_Log_onEventLog(object sender, EventLog e);
        /// <summary>
        /// Произошло событие системное правим текущий статус
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Log_onEventLog(object sender, EventLog e)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    lock (this.LockEventLog)
                    {
                        delig_Log_onEventLog dl = new delig_Log_onEventLog(Log_onEventLog);
                        this.Invoke(dl, new object[] { sender, e });
                    }
                }
                else
                {
                    lock (this.LockObj)
                    {
                        bool HashConnectRep = (RepositoryFarm.CurRepository == null ? false:RepositoryFarm.CurRepository.HashConnect);
                        if (HashConnectRep)
                        {
                            this.PicStatRepOnline.Visible = true;
                            this.PicStatRepOfline.Visible = false;
                        }
                        else
                        {
                            this.PicStatRepOnline.Visible = false;
                            this.PicStatRepOfline.Visible = true;
                        }

                        bool HashConnectPrvObj = (ProviderFarm.CurProviderObj == null ? false : ProviderFarm.CurProviderObj.HashConnect);
                        bool HashConnectPrvMon = (ProviderFarm.CurProviderMon == null ? false : ProviderFarm.CurProviderMon.HashConnect);
                        if (HashConnectPrvObj && HashConnectPrvMon)
                        {
                            this.PicStatPrvOnline.Visible = true;
                            this.PicStatPrvOfline.Visible = false;
                        }
                        else
                        {
                            this.PicStatPrvOnline.Visible = false;
                            this.PicStatPrvOfline.Visible = true;
                        }

                        if (e == null)
                        {
                            if (!HashConnectRep)
                            {
                                this.tSSLabel.BackColor = Color.Khaki;
                                this.tSSLabel.Text = "Подключения к репозиторию не установлено.";
                            }
                            else
                            {
                                this.tSSLabel.Text = string.Format("Подключение с базой данных версии {0} ({1}) установлено.", RepositoryFarm.CurRepository.VersionDB, RepositoryFarm.CurRepository.PlugInType);
                            }
                        }


                        // Для сообщения
                        string AggMessage = "";

                        if (e != null)
                        {
                            if (e.Message.Length < 200) AggMessage = e.Message;
                            else
                            {
                                AggMessage = e.Message.Substring(0, 200);
                            }

                            switch (e.Evn)
                            {
                                case EventEn.Empty:
                                case EventEn.Dump:
                                    break;
                                case EventEn.Warning:
                                    this.tSSLabel.BackColor = Color.Khaki;
                                    this.tSSLabel.Text = e.Message;
                                    break;
                                case EventEn.Error:
                                case EventEn.FatalError:
                                    this.tSSLabel.BackColor = Color.Tomato;
                                    this.tSSLabel.Text = e.Message;
                                    break;
                                default:
                                    this.tSSLabel.BackColor = this.DefBaskCoclortSSLabel;
                                    this.tSSLabel.Text = e.Message;
                                    break;
                            }
                        }

                        
                        // Если нет фатальной ошибки то нужно обработать результат
                        if (e.Evn != EventEn.FatalError || e.Tab != null)
                        {

                            /*
                                // Если результата нет, то не нужно лазеть по таблице
                                if (e.Tab != null && e.Tab.Rows.Count > 0)
                                {
                                    // Запоминаем подсвеченную ячейку
                                    Point OldPoz = new Point(-1, -1);
                                    int OldRol = 0;
                                    string[] OldData = new string[this.dataGridView1.ColumnCount];
                                    try
                                    {
                                        if (this.dataGridView1 != null && this.dataGridView1.CurrentCell != null)
                                        {
                                            OldPoz = new Point(this.dataGridView1.CurrentCell.ColumnIndex, this.dataGridView1.CurrentCell.RowIndex);
                                            OldRol = this.dataGridView1.RowCount;

                                            // Запоминаем значения в выделенной строке
                                            for (int i = 0; i < OldData.Count(); i++)
                                            {
                                                OldData[i] = this.dataGridView1.Rows[this.dataGridView1.CurrentCell.RowIndex].Cells[i].Value.ToString();
                                            }
                                        }
                                    }
                                    catch (Exception) { }



                                    // Получаем монопольный доступ для того чтобы склонировать таблицу в нашу форму
                                    lock (e.Tab)
                                    {
                                        // Делаем клон структуры таблицы если у нас его ещё нет и привязываем этот клон к нашему представлению
                                        if (this.Tab == null) { this.Tab = e.Tab.Clone(); this.View = new DataView(this.Tab); }

                                        // Чистим нашу собственную таблицу и заполняем данными которые передало событие. Это позволяет нам отвязаться от той таблицы, что передавалась в событии
                                        this.Tab.Clear();
                                        for (int ri = 0; ri < e.Tab.Rows.Count; ri++)
                                        {
                                            DataRow nrow = this.Tab.NewRow();
                                            for (int ci = 0; ci < e.Tab.Columns.Count; ci++)
                                            {
                                                nrow[ci] = e.Tab.Rows[ri][ci];
                                            }
                                            this.Tab.Rows.Add(nrow);
                                        }
                                    }

                                    // Если есть ошибка то добавляем её в нашу таблицу
                                    //                        if (e.Evn == Lib.EventEn.FatalError)
                                    //                         {
                                    //                             foreach (string item in e.obj.ErrMessage)
                                    //                            {
                                    //                                 if (item != null)
                                    //                                {
                                    //                                     DataRow nrow = this.Tab.NewRow();
                                    //                                     nrow[0] = @"Provider Err";
                                    //                                      nrow[1] = @"Ошибка: " + item + " при выполнении запроса в провайдере";
                                    //                                      nrow[2] = "Error";
                                    //                                      this.Tab.Rows.Add(nrow);
                                    //                                  }
                                    //                             }  
                                    //                          }

                                    // Применяем новую таблицу с данными
                                    this.dataGridView1.DataSource = this.View;

                                    // Выделяем обратно выделенную ячейку
                                    try
                                    {
                                        if (OldPoz.X > -1 && OldPoz.Y > -1 && this.dataGridView1.Columns.Count > OldPoz.X && this.dataGridView1.RowCount > OldPoz.Y && OldRol == this.dataGridView1.RowCount)
                                        {
                                            // Проверяем значения в выделенной строке
                                            for (int i = 0; i < OldData.Count(); i++)
                                            {
                                                // Если данные не совпадают то не нужно выделять ячейку
                                                if (OldData[i] != this.dataGridView1.Rows[OldPoz.Y].Cells[i].Value.ToString())
                                                {
                                                    OldPoz.X = -1;
                                                    OldPoz.Y = -1;
                                                }
                                            }
                                            if (OldPoz.X > -1 && OldPoz.Y > -1)
                                            {
                                                this.dataGridView1.CurrentCell = this.dataGridView1.Rows[OldPoz.Y].Cells[OldPoz.X];
                                            }
                                        }
                                    }
                                    catch (Exception) { }

                                    // Применяем цвет к строчкам, для красоты
                                    foreach (DataGridViewRow r in this.dataGridView1.Rows)
                                    {
                                        r.DefaultCellStyle.BackColor = Color.YellowGreen;
                                        r.DefaultCellStyle.SelectionBackColor = Color.Green;
                                        r.DefaultCellStyle.SelectionForeColor = Color.Black;
                                        if (r.Cells[2].Value.ToString() == "Warning") { r.DefaultCellStyle.BackColor = Color.Khaki; r.DefaultCellStyle.SelectionBackColor = Color.Yellow; }
                                        if (r.Cells[2].Value.ToString() == "Error" || r.Cells[2].Value.ToString() == "Fatal") { r.DefaultCellStyle.BackColor = Color.Tomato; r.DefaultCellStyle.SelectionBackColor = Color.Red; }
                                    }

                                    // строим то что хотим отобразить в уведомлениии тея
                                    for (int i = 0; i < this.Tab.Rows.Count; i++)
                                    {
                                        // Сообщеие ограничено по длине по этому выкидываем строки без ошибок
                                        if (this.Tab.Rows[i]["STATUS"].ToString() == "Error" || this.Tab.Rows[i]["STATUS"].ToString() == "Fatal")
                                        {
                                            if (s != "") s = s + "\r\n";
                                            s = s + this.Tab.Rows[i]["SHORT_NAME"].ToString(); // Нафиг надо итак отображаем только ожибки +" - " + this.Tab.Rows[i]["STATUS"].ToString();

                                            // Тут вызовем менюшку чтобы передать ей ошибку о которой нужно очень сильно напомнить пользователю
                                            if (this.Tab.Rows[i]["STATUS"].ToString() == "Fatal")
                                            {
                                                FFatalMessage Fform = new FFatalMessage(this.Tab.Rows[i]["DETAIL"].ToString());
                                                Fform.ShowDialog();
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (this.Tab != null) this.Tab.Rows.Clear();
                                    if (e.Evn != Lib.EventEn.FatalError)
                                    {
                                        s = "Всё супер.";
                                    }
                                }*/

                            // Вызов уведомления отображаем всплывающее окно только если комп не заблокирован
                            if (e.ShowNotification && AggMessage.Length > 0 && registered && Config.ShowNotification) this.m_notifyicon.ShowBalloonTip(5000, e.Evn.ToString(), AggMessage, ToolTipIcon.Info);
                        }
                        else // Возникла ошибка результат чистим и выводим сообщение о том что есть серьёзнве ошибки
                        {
                            //if (this.Tab != null) this.Tab.Rows.Clear();
                            //s = "Возникла ошибка при подключении к данным. " + e.Message;
                        }

                        // Меняем иконку
                        EventStatusIcon(e);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.EventSave(string.Format(@"Ошибка в методе {0}:""{1}""", "Log_onEventLog", ex.Message), this.GetType().FullName, EventEn.Error, true, true);
            }
        }

        delegate void delig_Log_onEventStatusIcon(EventLog e);
        /// <summary>
        /// Событие происходящее с заданным интервалом в трейдах
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void EventStatusIcon(EventLog e)
        {
            if (this.InvokeRequired)
            {
                //if (this.KeyCtrl) return;
                delig_Log_onEventStatusIcon dl = new delig_Log_onEventStatusIcon(EventStatusIcon);
                this.Invoke(dl, new object[] {e});
            }
            else
            {
                try
                {
                    if (e.ShowNotification)
                    {
                        lock (LockObj)
                        {
                            if (CustomizationFarm.CurCustomization != null && !isClosed)
                            {
                                this.Icon = CustomizationFarm.CurCustomization.GetIconStatus(e.Evn);
                                m_notifyicon.Icon = this.Icon;
                                Image Img = (Image)this.Icon.ToBitmap();
                                this.toolStripStatusLabelIcon.Image = Img;

                                // Меняем иконку
                                switch (e.Evn)
                                {
                                    case EventEn.Message:
                                        break;
                                    case EventEn.Warning:
                                        //this.tSSLabel.ForeColor = Color.Khaki;
                                        this.tSSLabel.BackColor = Color.Khaki;
                                        break;
                                    case EventEn.Error:
                                        this.tSSLabel.BackColor = Color.Tomato;
                                        break;
                                    case EventEn.FatalError:
                                        this.tSSLabel.BackColor = Color.Red;
                                        break;
                                    case EventEn.Empty:
                                        break;
                                    default:
                                        this.tSSLabel.BackColor = Color.Green;
                                        break;
                                }

                                this.toolStripStatusLabelIcon.Text = String.Format("Последнее получение статуса произошло: {0}", DateTime.Now.ToString());
                            }
                            // Иногда исчезает подписка на двойной клик. Если вдруг исчезла, то подписываемся снова
                            m_notifyicon.DoubleClick -= new EventHandler(DoubleClickIcon);
                            m_notifyicon.DoubleClick += new EventHandler(DoubleClickIcon);
                        }
                    }
                }
                catch (Exception ex)
                {
                    ApplicationException ae = new ApplicationException(string.Format("Возникла ошибка в интерфейсе: {0}", ex.Message));
                    Log.EventSave(ae.Message, string.Format("{0}.Log_onEventStatusIcon", this.GetType().FullName), EventEn.Error, true, true);
                    //throw ae;
                }
            }
        }

        /// <summary>
        /// Скрывает форму при нажатии на крестик
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FStart_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!this.isClosed)
            {
                this.Hide_Click(null, null);
                e.Cancel = true;
            }
        }

        // Пользователь решил настроить репозиторий
        private void TsmItemConfigRepository_Click(object sender, EventArgs e)
        {
            try
            {
                using (FRepositorySetup Frm = new FRepositorySetup())
                {
                    Frm.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                ApplicationException ae = new ApplicationException(string.Format(@"Ошибка в методе TsmItemConfigRepository_Click:""{0}""", ex.Message));
                Log.EventSave(ae.Message, string.Format("{0}.TsmItemConfigRepository_Click", this.GetType().FullName), EventEn.Error, true, true);
            }
        }

        // Пользователь решил настроить провайдер базы объектов
        private void TsmItemConfigProviderObj_Click(object sender, EventArgs e)
        {
            try
            {
                // Проверяем на наличие подключения к репозиотрию если его нет то некуда сохранять строку подключения и неоткуда её брать
                if (RepositoryFarm.CurRepository != null && RepositoryFarm.CurRepository.HashConnect)
                {
                    using (FProviderSetup Frm = new FProviderSetup(false))
                    {
                        Frm.ShowDialog();
                    }
                }
                else throw new ApplicationException("Нет подключения к репозиторию по этой причине нет возможности сделать настройку провайдера");

            }
            catch (Exception ex)
            {
                ApplicationException ae = new ApplicationException(string.Format(@"Ошибка в методе TsmItemConfigProviderObj_Click:""{0}""", ex.Message));
                Log.EventSave(ae.Message, string.Format("{0}.TsmItemConfigProviderObj_Click", this.GetType().FullName), EventEn.Error, true, true);
            }
        }

        // Пользователь решил настроить провайдер базы мониторинга
        private void TsmItemConfigProviderMon_Click(object sender, EventArgs e)
        {
            try
            {
                // Проверяем на наличие подключения к репозиотрию если его нет то некуда сохранять строку подключения и неоткуда её брать
                if (RepositoryFarm.CurRepository != null && RepositoryFarm.CurRepository.HashConnect)
                {
                    using (FProviderSetup Frm = new FProviderSetup(true))
                    {
                        Frm.ShowDialog();
                    }
                }
                else throw new ApplicationException("Нет подключения к репозиторию по этой причине нет возможности сделать настройку провайдера");

            }
            catch (Exception ex)
            {
                ApplicationException ae = new ApplicationException(string.Format(@"Ошибка в методе TsmItemConfigProviderMon_Click:""{0}""", ex.Message));
                Log.EventSave(ae.Message, string.Format("{0}.TsmItemConfigProviderMon_Click", this.GetType().FullName), EventEn.Error, true, true);
            }
        }

        // Пользователь настраивает дополнительные параметры программы
        private void TsmItemConfigParam_Click(object sender, EventArgs e)
        {
            try
            {
                // Проверяем на наличие подключения к репозиотрию если его нет то некуда сохранять строку подключения и неоткуда её брать
                if (RepositoryFarm.CurRepository != null && RepositoryFarm.CurRepository.HashConnect)
                {
                    using (FSetup Frm = new FSetup())
                    {
                        Frm.ShowDialog();
                    }
                }
                else throw new ApplicationException("Нет подключения к репозиторию по этой причине нет возможности сделать настройку провайдера");
            }
            catch (Exception ex)
            {
                ApplicationException ae = new ApplicationException(string.Format(@"Ошибка в методе TsmItemConfigParam_Click:""{0}""", ex.Message));
                Log.EventSave(ae.Message, string.Format("{0}.TsmItemConfigParam_Click", this.GetType().FullName), EventEn.Error, true, true);
            }
        }

    }
}
