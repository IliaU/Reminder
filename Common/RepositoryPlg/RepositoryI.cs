﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.RepositoryPlg
{
    /// <summary>
    /// Интерфейс который должны реализовать все классы для работы с нашим репозиторием но сами пулы дёргать и знать о них ничего не должны
    /// </summary>
    public interface RepositoryI
    {
        /// <summary>
        /// Метод для регистрации состояния пула с потоками
        /// </summary>
        /// <param name="MachineName">Имя машины где работает пул потоков</param>
        /// <param name="CustomClassTyp">Имя класса с потоками этого вида</param>
        /// <param name="LastDateReflection">Дата и время последнего получения статуса</param>
        /// <param name="VersionPul">Версия пула с потоками</param>
        /// <param name="LastStatusCustom">Статус который получили от самих потоков в этом пуле</param>
        void PulBasicSetStatus(string MachineName, string CustomClassTyp, DateTime LastDateReflection, string VersionPul, string LastStatusCustom);

        /// <summary>
        /// Метод для регистрации состояния всей ноды воркера
        /// </summary>
        /// <param name="MachineName">Имя машины где работает пул потоков</param>
        /// <param name="LastDateReflection">Дата и время последнего получения статуса</param>
        /// <param name="VersionNode">Версия воркера</param>
        /// <param name="LastStatusNode">Статус который получили от самой ноды</param>
        void NodeSetStatus(string MachineName, DateTime LastDateReflection, string VersionNode, string LastStatusNode);

        /// <summary>
        /// Сохранение провайдеоа в репозиторий
        /// </summary>
        /// <param name="Prv">Провайдер который сохраняем</param>
        /// <param name="Mon">Какой тип провайдера для мониторинга или для объектов</param>
        void SaveProvider(Provider Prv, bool Mon);

        /// <summary>
        /// Получение провайдера из базы
        /// </summary>
        /// <param name="Mon">Какой тип провайдера для мониторинга или для объектов</param>
        /// <returns>Провайдер полученный из базы</returns>
        Provider SelectProvider(bool Mon);
    }
}
