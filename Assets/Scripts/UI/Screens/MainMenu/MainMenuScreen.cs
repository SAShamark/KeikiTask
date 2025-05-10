using UI.Screens.Base;
using UnityEngine;

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

        public override void Initialize()
        {
            base.Initialize();
            foreach (var level in _config.LevelData)
            {
                var panel = Instantiate(_levelsPanel, _transform);
                panel.Initialize(level);
            }
        }
    }
}
