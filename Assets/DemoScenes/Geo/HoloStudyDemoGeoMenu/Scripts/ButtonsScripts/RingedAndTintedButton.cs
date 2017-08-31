using UnityEngine;
public class RingedAndTintedButton : StandardSimpleButton
{
    [SerializeField]
    protected RingsSelectingController MyRings;
    [SerializeField]
    protected AllTinter MyTinter;

    protected virtual void enable()
    {
        enabledByFunctionality = true;
        MyTinter.Untint();
    }

    protected virtual void disable()
    {
        enabledByFunctionality = false;
        MyTinter.Tint();
    }
}
