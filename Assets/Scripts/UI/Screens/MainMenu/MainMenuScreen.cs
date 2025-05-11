using System.Collections.Generic;
using Services.Scenes;
using UI.Screens.Base;
using UI.Screens.MainMenu.Data;
using UnityEngine;
using Zenject;

namespace UI.Screens.MainMenu
{
    public class MainMenuScreen : BaseScreen
    {
        [SerializeField]
        private LevelsPanel _levelsPanel;

        [SerializeField]
        private Transform _transform;

        [SerializeField]
        private LevelsConfig _config;

        private ISceneLoader _sceneLoader;
        private readonly List<LevelsPanel> _panels = new();

        [Inject]
        private void Construct(ISceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
        }

        private void OnDestroy()
        {
            foreach (var panel in _panels)
            {
                panel.OnLevelButtonClicked -= LevelButtonClicked;
            }
        }

        public override void Initialize()
        {
            base.Initialize();
            foreach (var level in _config.LevelData)
            {
                var panel = Instantiate(_levelsPanel, _transform);
                panel.Initialize(level);
                _panels.Add(panel);
                panel.OnLevelButtonClicked += LevelButtonClicked;
            }
        }

        private void LevelButtonClicked()
        {
            _sceneLoader.LoadScene(SceneType.GamePlay, ScreenTypes.Gameplay);
        }
    }
}