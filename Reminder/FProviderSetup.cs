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
    public partial class FProviderSetup : Form
    {
        /// <summary>
        /// Какой репозиторий настраиваем если true значит репозиторий для мониторинга есле false то для объектов
        /// </summary>
        private bool IsMon;

        // Список типов репозиториев
        private List<PluginClassElement> cmbBoxPrvTypList = new List<PluginClassElement>();

        // Текущий репозиторий
        private Provider CurentPrvTmp = null;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="IsMon">Какой репозиторий настраиваем если true значит репозиторий для мониторинга есле false то для объектов</param>
        public FProviderSetup(bool IsMon)
        {
            try
            {
                this.IsMon = IsMon;
                if (IsMon) this.CurentPrvTmp = ProviderFarm.CurProviderMon;
                else this.CurentPrvTmp = ProviderFarm.CurProviderObj;

                InitializeComponent();

                this.Icon = CustomizationFarm.CurCustomization.GetIconStatus(EventEn.Message);

                // Подгружаем список возможных провайдеров
                this.cmbBoxPrvTyp.Items.Clear();
                cmbBoxPrvTypList = ProviderFarm.ListProviderName;
                foreach (PluginClassElement item in cmbBoxPrvTypList)
                {
                    this.cmbBoxPrvTyp.Items.Add(item.Name);
                }

                // Если всего один тип провайдеров существует то устанавливаем по умолчанию этот тип
                if (this.cmbBoxPrvTyp.Items.Count == 1) this.cmbBoxPrvTyp.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                ApplicationException ae = new ApplicationException(string.Format("Упали при инициализации конструктора с ошибкой: ({0})", ex.Message));
                Log.EventSave(ae.Message, string.Format("{0}", this.GetType().FullName), EventEn.Error);
                throw ae;
            }
        }

        // Чтение формы
        private void FProviderSetup_Load(object sender, EventArgs e)
        {
            try
            {
                // Получаем текущий репозиторий
                if (IsMon) this.CurentPrvTmp = ProviderFarm.CurProviderMon;
                else this.CurentPrvTmp = ProviderFarm.CurProviderObj;

                // Если текущий репозиторий есть и он не выбран то нужно указать его тип
                if (this.CurentPrvTmp != null)
                {
                    for (int i = 0; i < this.cmbBoxPrvTyp.Items.Count; i++)
                    {
                        if (this.cmbBoxPrvTyp.Items[i].ToString() == this.CurentPrvTmp.PlugInType) this.cmbBoxPrvTyp.SelectedIndex = i;
                    }
                }

                //  Если текущий репозиторий не установлен то на выход
                if (this.CurentPrvTmp == null) return;
                this.txtBoxConnectionString.Text = this.CurentPrvTmp.PrintConnectionString();
            }
            catch (Exception ex)
            {
                ApplicationException ae = new ApplicationException(string.Format("Упали при чтении формы с ошибкой: ({0})", ex.Message));
                Log.EventSave(ae.Message, string.Format("{0}.FProviderSetup_Load", this.GetType().FullName), EventEn.Error);
                throw ae;
            }
        }

        // Пользователь решил изменить
        private void btnConfig_Click(object sender, EventArgs e)
        {
            if (this.cmbBoxPrvTyp.SelectedIndex == -1)
            {
                MessageBox.Show("Вы не выбрали тип провайдера который вы будите использовать.");
                return;
            }

            // Создаём ссылку на подключение которое будем править
            Provider PrvTmp = null;
            //
            // Если текущий провайдер не установлен то иницилизируем его новый экземпляр или создаём его на основе уже существующего провайдера
            if (this.CurentPrvTmp == null || (this.CurentPrvTmp != null && this.CurentPrvTmp.PlugInType != this.cmbBoxPrvTyp.Items[this.cmbBoxPrvTyp.SelectedIndex].ToString()))
            {
                PrvTmp = ProviderFarm.CreateNewProvider(this.cmbBoxPrvTyp.Items[this.cmbBoxPrvTyp.SelectedIndex].ToString());
            }
            else PrvTmp = ProviderFarm.CreateNewProvider(this.CurentPrvTmp.PlugInType, this.CurentPrvTmp.ConnectionString);
            // 
            // Запускаем правку нового подключения
            bool HashSaveProvider = PrvTmp.SetupConnectDB(ref PrvTmp);

            // Пользователь сохраняет данный провайдер в качестве текущего
            if (HashSaveProvider)
            {
                if (IsMon) ProviderFarm.SetCurrentProviderMon (PrvTmp, true);
                else ProviderFarm.SetCurrentProviderObj(PrvTmp, true);
            }

            // Перечитываем текущую форму
            FProviderSetup_Load(null, null);
        }
    }
}
