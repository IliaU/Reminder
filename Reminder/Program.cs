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
                Mutex m = new Mutex(true, "Reminder", out oneOnlyProg);
                if (oneOnlyProg == true) // Если процесс существует надо его убить
                {
                    // Подключаем логирование
                    Log Lg = new Log();

                    // Подключаем конфиг
                    Config Cnf = new Config();

                    // Запускаем объект наших провайдеров
                    ProviderFarm PrvF = new ProviderFarm();

                    // Создаём Ферму с пулами и с доступными элементами для них
                    IoFarm IoF = new IoFarm();
                    IoListFarm ILisF = new IoListFarm();
                    //IoFarm.CreateNewIo(IoFarm.ListIoName[0].Items[0]);
                    //IoListFarm.CreateNewIoList(IoListFarm.ListIoListName[0].Items[0]);
                    ProgramStatus.CreateCurentPulList();        // Запускаем объект который содержит нужные пуля со спписком объектов внутри пулов чтобы пул был того же типа как и объекты которые для него
                    ProgramStatus.StartCompileListing();        // Запуск асинхронного процесса мониторинга ноды

                    // Тест плагинной технологии
                    //Customization tt = Common.CustomizationFarm.CreateNewCustomization(Common.CustomizationFarm.GetListCustomizationName()[0].Name);
                    //Common.CustomizationFarm.SetCurrentCustomization(tt);
                    //Repository rep = RepositoryFarm.CreateNewRepository("RepositoryMsSql");
                    //ProgramStatus.CreateCurentPulList();

                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new FStart());

                    Log.EventSave("Остановка приложения", "Program", EventEn.Message);
                    ProgramStatus.Stop();
                    ProgramStatus.Join(false);
                    Log.EventSave("Приложение остановлено", "Program", EventEn.Message);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(String.Format("Не смогли запуститься ошибка: {0}", ex.Message));
            }

        }
    }
}
