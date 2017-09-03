using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using Zenject;

public class StandardContextMenuItem : ContextMenuItem
{
    public string Name;
    public Color NameColor = Color.white;
    public string Description;
    public Color DescriptionColor = Color.white;
    public Texture Icon;
    public ContextMenuActiveState ActiveState;
    public ContextMenuSelectedState SelectedState;
    public GameObject GOFromPool;
}

public class StandardContextMenuItems : MonoBehaviour, IContextMenuItems
{
    [Inject]
    protected IContextMenu desiredContextMenu;
    public IContextMenu DesiredContextMenu
    {
        get
        {
            return desiredContextMenu;
        }

        protected set
        {
            desiredContextMenu = value;
        }
    }

    protected List<ContextMenuItem> menuItems;
    public List<ContextMenuItem> MenuItems
    {
        get
        {
            return menuItems;
        }

        protected set
        {
            menuItems = value;
        }
    }
}
