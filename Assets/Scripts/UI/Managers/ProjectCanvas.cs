using Audio;
using UI.Popups;
using UI.Screens;
using UnityEngine;
using Zenject;

namespace UI.Managers
{
    public class ProjectCanvas : MonoBehaviour, IUIManager
    {
        [SerializeField]
        private ScreensManager _screensManager;

        [SerializeField]
        private PopupsManager _popupsManager;

        [SerializeField]
        private WindowsConfig _windowsConfig;

        private DiContainer _diContainer;
        private IAudioManager _audioManager;


        public IScreensManager ScreensManager => _screensManager;
        public IPopupsManager PopupsManager => _popupsManager;

        [Inject]
        private void Construct(DiContainer diContainer, IAudioManager audioManager)
        {
            _diContainer = diContainer;
            _audioManager = audioManager;
        }
        

        public int Priority => 1;

        public void Init()
        {
            DontDestroyOnLoad(gameObject);
            _screensManager.Initialize(_diContainer);
            _popupsManager.Initialize(_diContainer);
            ConfigLoaded();
        }

        private void ConfigLoaded()
        {
            _screensManager.OnConfigLoaded(_windowsConfig);
            _popupsManager.OnConfigLoaded(_windowsConfig);
        }
    }
}