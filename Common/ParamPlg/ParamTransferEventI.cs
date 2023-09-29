﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.ParamPlg
{
    /// <summary>
    /// Доставка параметров к элементам которые подписаны через систему подписок
    /// в этом варианте класс после выполнения задачи проверяет кто на него подписан и передаёт информацию напрямую подписанту без базы данных
    /// Это позволяет базу данных не нагружать не сильно нужными данными и организовать скорость но для атомарности процессов надо чтобы инфа по доставке была сохранениа в базе чтобы можно было повторить операцию
    /// </summary>
    public interface ParamTransferEventI
    {
    }
}
