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
    public partial class FSetup : Form
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public FSetup()
        {
            try
            {
                InitializeComponent();

                this.Icon = CustomizationFarm.CurCustomization.GetIconStatus(EventEn.Message);
            }
            catch (Exception ex)
            {
                ApplicationException ae = new ApplicationException(string.Format("Упали при инициализации конструктора с ошибкой: ({0})", ex.Message));
                Log.EventSave(ae.Message, string.Format("{0}", this.GetType().FullName), EventEn.Error);
                throw ae;
            }

        }
    }
}
