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

namespace Reminder
{
    public partial class FRepositorySetup : Form
    {
        // Список типов репозиториев
        private List<PluginClassElement> cmbBoxRepTypList = new List<PluginClassElement>();

        // Текущий репозиторий
        private Repository CurentRepTmp = RepositoryFarm.CurRepository;

        public FRepositorySetup()
        {
            try
            {
                InitializeComponent();

                this.Icon = CustomizationFarm.CurCustomization.GetIconStatus(EventEn.Message);

                // Подгружаем список возможных провайдеров
                this.cmbBoxRepTyp.Items.Clear();
                cmbBoxRepTypList = RepositoryFarm.ListRepositoryName;
                foreach (PluginClassElement item in cmbBoxRepTypList)
                {
                    this.cmbBoxRepTyp.Items.Add(item.Name);
                }

                // Если всего один тип провайдеров существует то устанавливаем по умолчанию этот тип
                if (this.cmbBoxRepTyp.Items.Count == 1) this.cmbBoxRepTyp.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                ApplicationException ae = new ApplicationException(string.Format("Упали при инициализации конструктора с ошибкой: ({0})", ex.Message));
                Log.EventSave(ae.Message, string.Format("{0}", this.GetType().FullName), EventEn.Error);
                throw ae;
            }
        }

        // Чтение формы
        private void FRepositorySetup_Load(object sender, EventArgs e)
        {
            try
            {
                // Получаем текущий репозиторий
                this.CurentRepTmp = RepositoryFarm.CurRepository;

                // Если текущий репозиторий есть и он не выбран то нужно указать его тип
                if (this.CurentRepTmp != null)
                {
                    for (int i = 0; i < this.cmbBoxRepTyp.Items.Count; i++)
                    {
                        if (this.cmbBoxRepTyp.Items[i].ToString() == this.CurentRepTmp.PlugInType) this.cmbBoxRepTyp.SelectedIndex = i;
                    }
                }

                //  Если текущий репозиторий не установлен то на выход
                if (this.CurentRepTmp == null) return;
                this.txtBoxConnectionString.Text = this.CurentRepTmp.PrintConnectionString();
            }
            catch (Exception ex)
            {
                ApplicationException ae = new ApplicationException(string.Format("Упали при чтении формы с ошибкой: ({0})", ex.Message));
                Log.EventSave(ae.Message, string.Format("{0}.FRepositorySetup_Load", this.GetType().FullName), EventEn.Error);
                throw ae;
            }
        }

        // Пользователь решил изменить
        private void btnConfig_Click(object sender, EventArgs e)
        {
            if (this.cmbBoxRepTyp.SelectedIndex == -1)
            {
                MessageBox.Show("Вы не выбрали тип репозитория который вы будите использовать.");
                return;
            }

            // Создаём ссылку на подключение которое будем править
            Repository RepTmp = null;
            //
            // Если текущий провайдер не установлен то иницилизируем его новый экземпляр или создаём его на основе уже существующего провайдера
            if (this.CurentRepTmp == null || (this.CurentRepTmp != null && this.CurentRepTmp.PlugInType != this.cmbBoxRepTyp.Items[this.cmbBoxRepTyp.SelectedIndex].ToString()))
            {
                RepTmp = RepositoryFarm.CreateNewRepository(this.cmbBoxRepTyp.Items[this.cmbBoxRepTyp.SelectedIndex].ToString());
            }
            else RepTmp = RepositoryFarm.CreateNewRepository(this.CurentRepTmp.PlugInType, this.CurentRepTmp.ConnectionString);
            // 
            // Запускаем правку нового подключения
            bool HashSaveRepository = RepTmp.SetupConnectDB(ref RepTmp);

            // Пользователь сохраняет данный репозиорий в качестве текущего
            if (HashSaveRepository)
            {
                RepositoryFarm.SetCurrentRepository(RepTmp, true);
            }

            // Перечитываем текущую форму
            FRepositorySetup_Load(null, null);
        }
    }
}
