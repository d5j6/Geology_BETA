using Zenject;

public class DefaultBindingsInstaller : MonoInstaller<DefaultBindingsInstaller>
{
    public override void InstallBindings()
    {
        //Container.Bind<IContextMenu>().To<StandardContextMenu>().FromInstance(StandardContextMenu.Instance).AsSingle();
    }
}