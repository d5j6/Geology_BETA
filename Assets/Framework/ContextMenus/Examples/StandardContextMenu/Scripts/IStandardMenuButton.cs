using System;

/// <summary>
/// Интерфейс кнопки для стандартного меню. Стандартное меню - это радиальное меню, поэтому каждой кнопке при создании передаем угол, на котором она должна отогбражаться
/// </summary>
public interface IStandardMenuButton
{
    /// <summary>
    /// Чтобы не тормозить в рантайме создаем пул кнопок, индекс в котором и обозначается это переменной
    /// </summary>
    int IndexInPool { get; set; }
    void Show(StandardContextMenuItem itemInfo, float angle, float distance, Action callback = null);
    void Hide(Action callback = null);
}
