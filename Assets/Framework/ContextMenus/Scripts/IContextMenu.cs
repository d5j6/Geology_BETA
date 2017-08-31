/// <summary>
/// При необходимости показать контектное меню - вызывается метод Show и передаются в него элементы меню (IContextMenuItems). При закрытии - Hide. Ваш кэп.
/// </summary>
public interface IContextMenu {
    
    void Show(IContextMenuItems items, System.Action callback = null);
    
    void Hide(System.Action callback = null);
}