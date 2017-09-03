using UnityEngine;
using System;
using System.Collections.Generic;

public enum ContextMenuActiveState { Inactive = 0, Active = 1 }
public enum ContextMenuSelectedState { Unselected = 0, Selected = 1 }
public enum ContextMenuItemType { BackButton = 0, Regular = 1 }

/// <summary>
/// Родительский класс для элементов меню. Т.к. элементы меню могут теоретически представлять собой все, что может прийти в голову, элементов тут мало.
/// Единственные, пожалуй, неотъемлемые свойства элементов контектного меню - возможность наличия подменю и действия, совершаемого при нажатии на данный элемент.
/// </summary>
public class ContextMenuItem
{
    public ContextMenuItemType Type = ContextMenuItemType.Regular;
    public List<ContextMenuItem> SubmenuItems;
    public Action Action;
}

/// <summary>
/// При поднятии руки голограмма, на которую мы смотрим проверяется на наличие IContextMenuItems и если оно есть - запускается метод Show контекстного меню, 
/// указанного в DesiredContextMenu, при опускании руки или отведении взгляда запускается его метод Hide
/// При этом, естественно, в желаемое контекстное меню передаются элементы меню, указанные в MenuItems
/// </summary>
public interface IContextMenuItems
{
    IContextMenu DesiredContextMenu { get; }
    List<ContextMenuItem> MenuItems { get; }
}
