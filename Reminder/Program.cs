using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using Common;
using System.Threading;

namespace Reminder
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                bool oneOnlyProg;
                Mutex m = new Mutex(true, "AlgoritmPrizm", out oneOnlyProg);
                if (oneOnlyProg == true) // Если процесс существует надо его убить
                {
                    // Подключаем логирование
                    Log Lg = new Log();

                    // Подключаем конфиг
                    Config Cnf = new Config();

                    // Тест плагинной технологии
                    Customization tt = Common.CustomizationFarm.CreateNewCustomization(Common.CustomizationFarm.GetListCustomizationName()[0].Name);
                    Common.CustomizationFarm.SetCurrentCustomization(tt);

                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new FStart());

                    Log.EventSave("Остановка приложения", " Program", EventEn.Message);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(String.Format("Не смогли запуститься ошибка: {0}", ex.Message));
            }

        }
    }
}
