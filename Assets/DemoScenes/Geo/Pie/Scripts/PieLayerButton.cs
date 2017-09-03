public class PieLayerButton : StandardSimpleButton {

    public PieController.PieLayerSelected LayerNeeded;

    protected override void singleTapAction()
    {
        base.singleTapAction();

        switch (LayerNeeded)
        {
            case PieController.PieLayerSelected.Sedimentary:
                PieController.Instance.ShowSedimentaryLayer();
                break;
            case PieController.PieLayerSelected.Granit:
                PieController.Instance.ShowGranitLayer();
                break;
            case PieController.PieLayerSelected.Basalt:
                PieController.Instance.ShowBasaltLayer();
                break;
            case PieController.PieLayerSelected.Magma:
                PieController.Instance.ShowMagmaLayer();
                break;
        }
    }
}
