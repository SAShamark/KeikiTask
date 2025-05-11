using System.Collections.Generic;
using Gameplay;
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
        private LevelsManager _levelsManager;
        private readonly List<LevelsPanel> _panels = new();

        [Inject]
        private void Construct(ISceneLoader sceneLoader, LevelsManager levelsManager)
        {
            _sceneLoader = sceneLoader;
            _levelsManager = levelsManager;
        }

        private void OnDestroy()
        {
            foreach (LevelsPanel panel in _panels)
            {
                panel.OnLevelButtonClicked -= LevelButtonClicked;
            }
        }

        public override void Initialize()
        {
            base.Initialize();
            for (var index = 0; index < _config.LevelData.Count; index++)
            {
                LevelData data = _config.LevelData[index];
                LevelsPanel panel = Instantiate(_levelsPanel, _transform);
                panel.Initialize(data, index);
                _panels.Add(panel);
                panel.OnLevelButtonClicked += LevelButtonClicked;
            }
        }

        private void LevelButtonClicked(int groupIndex, int level)
        {
            _levelsManager.SelectLevel(groupIndex, level);
            _sceneLoader.LoadScene(SceneType.GamePlay, ScreenTypes.Gameplay);
        }
    }
}