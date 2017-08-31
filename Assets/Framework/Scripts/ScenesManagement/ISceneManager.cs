/// <summary>
/// Интерфейс сцены
/// </summary>
public interface ISceneManager
{
    /// <summary>
    /// Для тестирования - сцена стартует автоматически
    /// </summary>
    /// <returns></returns>
    bool StartsAutomatically { get; }

    /// <summary>
    /// "Точка входа" для сцены. Вызывается из Loader'а, после загрузки сцены и когда завершена выгрузка предыдущей сцены (если это требуется)
    /// </summary>
    void StartScene();

    /// <summary>
    /// При выгрузке сцены нужно сделать это плавно и красиво - спрятать объекты сцены, заглушить все звуки. И чисто - отписаться от событий, прервать анимации и т.д..
    /// </summary>
    /// <param name="callback">После завершения подготовки к выгрузке надо вызвать этот Action</param>
    void PrepareToUnload(System.Action callback);
}
