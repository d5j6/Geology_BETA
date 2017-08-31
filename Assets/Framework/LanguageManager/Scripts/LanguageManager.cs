using HoloToolkit.Unity;

/// <summary>
/// Набор доступных в приложении языков
/// </summary>
public enum Language { English = 0, Russian = 1 }

/// <summary>
/// Синглетон для хранения текущей настройки языка.
/// </summary>
public class LanguageManager : Singleton<LanguageManager>
{
    public Language CurrentLanguage = Language.Russian;
}
