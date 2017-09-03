using UnityEngine;
using System;
using HoloToolkit.Unity;

public class PositioningMenu : MonoBehaviour {

    public PositioningButton[] MenuButtons;
    public AllTinter[] MenuButtonsTinter;

    /*void Awake()
    {
        for (int i = 0; i < MenuButtons.Length; i++)
        {
            MenuButtons[i].DeactivateImmediately();
        }
    }*/

    public void ActivateAllButtons(Action callback = null)
    {
        for (int i = 1; i < MenuButtons.Length; i++)
        {
            MenuButtons[i].Activate();
        }

        if (MenuButtons.Length > 0)
        {
            MenuButtons[0].Activate(callback);
        }
    }

    public void DeactivateAllButtons(Action callback = null)
    {
        for (int i = 1; i < MenuButtons.Length; i++)
        {
            MenuButtons[i].Deactivate();
        }

        if (MenuButtons.Length > 0)
        {
            MenuButtons[0].Deactivate(callback);
        }
    }

    /*void SetGestureManipulator(WantedManipulation manipullation)
    {

        GestureManipulator manipulator = StartPositioning.Instance.GetComponent<GestureManipulator>();

        if (manipulator == null)
        {
            manipulator = StartPositioning.Instance.gameObject.AddComponent<GestureManipulator>();
        }

        manipulator.WantedManipulation = manipullation;
    }*/

    public void Move()
    {
        /*DisableAllButtonsByFunctionaluity();
        TintAll();
        HandManager.Instance.PerformMoveAction(StartPositioning.Instance.transform, ReactivateButtons);*/
        //SetGestureManipulator(WantedManipulation.Move);
    }

    public void Rotate()
    {
        /*DisableAllButtonsByFunctionaluity();
        TintAll();
        HandManager.Instance.PerformRotatingAction(StartPositioning.Instance.transform, ReactivateButtons);*/
        //SetGestureManipulator(WantedManipulation.Rotate);
    }

    public void Scale()
    {
        /*DisableAllButtonsByFunctionaluity();
        TintAll();
        HandManager.Instance.PerformScalingAction(StartPositioning.Instance.transform, ReactivateButtons);*/
        //SetGestureManipulator(WantedManipulation.Scale);
    }

    public void ReactivateButtons()
    {
        EnableAllButtonsByFunctionaluity();
        UntintAll();
    }

    void DisableAllButtonsByFunctionaluity()
    {
        for (int i = 0; i < MenuButtons.Length; i++)
        {
            MenuButtons[i].DisableByFunctionality();
        }
    }

    void EnableAllButtonsByFunctionaluity()
    {
        for (int i = 0; i < MenuButtons.Length; i++)
        {
            MenuButtons[i].EnableByFunctionality();
        }
    }

    void TintAll()
    {
        for (int i = 0; i < MenuButtonsTinter.Length; i++)
        {
            MenuButtonsTinter[i].Tint();
        }
    }

    void UntintAll()
    {
        for (int i = 0; i < MenuButtonsTinter.Length; i++)
        {
            MenuButtonsTinter[i].Untint();
        }
    }

    public void OK()
    {
        //GestureManipulator manipulator = StartPositioning.Instance.GetComponent<GestureManipulator>();

        /*if (manipulator != null)
        {
            Destroy(manipulator);
        }

        StartPositioning.Instance.HidePositioningHolos(() =>
        {
            SceneStateMachine.Instance.GoToDefaultState();
        });*/
    }
}
