using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Common;
using System.Reflection;
using System.Windows.Forms;
using IoSystem.Properties;
using System.Drawing;

namespace IoSystem.Network
{
    /// <summary>
    /// Класс представляет из себя пулл с обектами
    /// </summary>
    public class IoHost : IoList, Common.IoPlg.IoListI
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="Plg">Информация по плагину чтобы можно было найти файл откудазагрузился и путь в том числе</param>
        public IoHost(PluginClassElement Plg) : base(Plg, Assembly.GetExecutingAssembly().FullName, Assembly.GetExecutingAssembly().GetName().Version.ToString())
        {
            try
            {
                // Подгружаем наименование нешего параметра для того чтобы плагин знал как обозвать класс
                base.ClassMethods.Add(typeof(IoHostGetNetwork).Name);
                //Log.EventSave("Плагин лугойла", "dd", EventEn.Message);
                //this.EventSave("Плагин лугойла yfcktljdfyysq", "dd", EventEn.Message);

                // Создаём параметры объекта
                Param PHostName = ParamFarm.CreateNewParam(string.Format("IoSystem.{0}", typeof(IoString).Name));
                PHostName.ParamName = "HostName";
                base.CurrentPurams.Add(PHostName);

            }
            catch (Exception ex)
            {
                ApplicationException ae = new ApplicationException(string.Format("Упали при инициализации конструктора с ошибкой: ({0})", ex.Message));
                this.EventSave(ae.Message, string.Format("{0}.IoHost", this.GetType().FullName), EventEn.Error);
                throw ae;
            }
        }

        /// <summary>
        /// Событие установки статуса пула для отслеживания зависаний и качества соединения с разными источниками относительно нод
        /// </summary>
        /// <returns>Возвращаем статус проверки</returns>
        public EventEn SetStatusPul()
        {
            try
            {
                return EventEn.Message;
            }
            catch (Exception ex)
            {
                ApplicationException ae = new ApplicationException(string.Format("Упали при инициализации конструктора с ошибкой: ({0})", ex.Message));
                this.EventSave(ae.Message, string.Format("{0}.IoMonLanList", this.GetType().FullName), EventEn.Error);
                throw ae;
            }
        }


        /// <summary>
        /// Получение элементов в главную форму для настройки глобальных свойств
        /// Данное событие обязательно. Необходимо для настройки глобальных методов которые будут доступны всем и для управления пулами на нодах их колличеством и правилами кто будет выполнять из нод задания
        /// </summary>
        /// <returns>Объект контектсного меню в главной форме, который будет видель наш пользователь</returns>
        public override ToolStripMenuItem GetFormMainCustSetup()
        {
            try
            {
                ToolStripMenuItem nitemS = new ToolStripMenuItem(Resources.IoHost_WinPng);
                nitemS.Click += NitemS_Click;
                //nitemS.BackColor = Color.Khaki;
                nitemS.Image = Resources.IoHost_WinPng;
                nitemS.Text = this.GetType().Name;
                //nitemS.Tag = item;

                return nitemS;
            }
            catch (Exception ex) { throw ex; }
        }

        // Пользователь вызвал меню настройки плагина
        private void NitemS_Click(object sender, EventArgs e)
        {
            try
            {
                // Проверяем на наличие подключения к репозиотрию если его нет то некуда сохранять строку подключения и неоткуда её брать
                if (RepositoryFarm.CurRepository != null && RepositoryFarm.CurRepository.HashConnect)
                {
                    using (FIoHostSetup Frm = new FIoHostSetup())
                    {
                        Frm.ShowDialog();
                    }
                }
                else throw new ApplicationException("Нет подключения к репозиторию по этой причине нет возможности сделать настройку провайдера");

            }
            catch (Exception ex)
            {
                ApplicationException ae = new ApplicationException(string.Format(@"Ошибка в методе NitemS_Click:""{0}""", ex.Message));
                Log.EventSave(ae.Message, string.Format("{0}.NitemS_Click", this.GetType().FullName), EventEn.Error, true, true);
            }
        }


        //// <summary>
        /// Получение элементов в главную форму в меню самих плагинов
        /// Данный метод не обязательны нужен только для корневого эелоемента плагина. Например это может быть отдельная программа которая потом будет дёргать другие объекты и генерить собственные события и подписки в ядре
        /// </summary>
        /// <returns>Объект контектсного меню в главной форме в менюшке плагинов как рутовый объект, который будет видель наш пользователь</returns>
        public override List<ToolStripMenuItem> GetFormMainCust()
        {
            try
            {
                List<ToolStripMenuItem> rez = new List<ToolStripMenuItem>();

                ToolStripMenuItem nitemS = new ToolStripMenuItem(Resources.IoHost_WinPng);
                nitemS.Click += NitemS_Click;
                //nitemS.BackColor = Color.Khaki;
                nitemS.Image = Resources.IoHost_WinPng;
                nitemS.Text = this.GetType().Name;
                //nitemS.Tag = item;

                rez.Add(nitemS);

                return rez;
            }
            catch (Exception ex) { throw ex; }
        }

    }
}
