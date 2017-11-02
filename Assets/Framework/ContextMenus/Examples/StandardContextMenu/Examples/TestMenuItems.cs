using UnityEngine;
using System.Collections.Generic;

public class TestMenuItems : StandardContextMenuItems
{
    // Use this for initialization
    void Start()
    {
        menuItems = new List<ContextMenuItem>();

        StandardContextMenuItem test1 = new StandardContextMenuItem();
        test1.Name = "Test 1";
        menuItems.Add(test1);
        StandardContextMenuItem test2 = new StandardContextMenuItem();
        test2.Name = "Test 2";
        test2.Icon = Resources.Load("ok_icon") as Texture;
        menuItems.Add(test2);
        StandardContextMenuItem test3 = new StandardContextMenuItem();
        test3.Name = "Submenu";
        menuItems.Add(test3);
        test3.SubmenuItems = new List<ContextMenuItem>();


        StandardContextMenuItem submenu1 = new StandardContextMenuItem();
        submenu1.Name = "Submenu Item 1";
        test3.SubmenuItems.Add(submenu1);
        StandardContextMenuItem submenu2 = new StandardContextMenuItem();
        submenu2.Name = "Submenu Item 2";
        test3.SubmenuItems.Add(submenu2);
        StandardContextMenuItem submenu3 = new StandardContextMenuItem();
        submenu3.Name = "Submenu Item 3";
        submenu3.Icon = Resources.Load("whether_icon") as Texture;
        submenu3.Action = () =>
        {
            DemoShowStateMachine.playing = true;
            ChooseScenePanelScript.Instance.MakeAllButtonsIndifferent();          
            ChooseScenePanelScript.Instance.Hide(() =>
            {
                Loader.Instance.LoadScene("DemoGeoScene"); 
            });
        };
        test3.SubmenuItems.Add(submenu3);
        StandardContextMenuItem submenu4 = new StandardContextMenuItem();
        submenu4.Name = "Submenu Item 4";
        test3.SubmenuItems.Add(submenu4);
        StandardContextMenuItem submenu5 = new StandardContextMenuItem();
        submenu5.Name = "Back";
        submenu5.Type = ContextMenuItemType.BackButton;
        test3.SubmenuItems.Add(submenu5);

        //StandardContextMenu.Instance.Show(this);

        /*LeanTween.delayedCall(3f, () =>
        {
            StandardContextMenu.Instance.Hide();
        });

        LeanTween.delayedCall(6f, () =>
        {
            StandardContextMenu.Instance.Show(this);
        });

        LeanTween.delayedCall(9f, () =>
        {
            StandardContextMenu.Instance.Hide();
        });

        LeanTween.delayedCall(12f, () =>
        {
            StandardContextMenu.Instance.Show(this);
        });*/
    }
}
