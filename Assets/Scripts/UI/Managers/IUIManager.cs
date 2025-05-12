using UI.Popups;
using UI.Screens;

namespace UI.Managers
{
    public interface IUIManager
    {
        IScreensManager ScreensManager { get; }
        IPopupsManager PopupsManager { get; }
    }
}