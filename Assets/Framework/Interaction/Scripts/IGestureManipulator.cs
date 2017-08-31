//using Zenject;

/// <summary>
/// Интерфейс манипулятора - он должен как-либо перемещать-скейлить-вращать объект, на который он помещен. Как именно это делать он решает сам, а агент обеспечивает его стандартизированной информацией.
/// </summary>
public interface IGestureManipulator
{
    //[Inject]
    IGestureManipulationAgent ManipulationAgent { get; }
    float MovingSensitivity { get; set; }
    float ScanlingSensitivity { get; set; }
    float RotationSensitivity { get; set; }
}
