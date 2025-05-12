using Services.Scenes;
using UI.Screens.Base;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Screens.Battle
{
    public class GameplayScreen : BaseScreen
    {
        [SerializeField]
        private Button _menuButton;

        private ISceneLoader _sceneLoader;

        [Inject]
        private void Construct(ISceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
        }

        private void OnDestroy()
        {
            _menuButton.onClick.RemoveListener(MenuButtonClicked);
        }

        public override void Initialize()
        {
            base.Initialize();
            _menuButton.onClick.AddListener(MenuButtonClicked);
        }

        private void MenuButtonClicked()
        {
            _sceneLoader.LoadScene(SceneType.Menu, ScreenTypes.MainMenu);
        }
    }
}