using TMPro;
using UnityEngine;

namespace UI.Screens.MainMenu
{
    public class LevelsPanel : MonoBehaviour
    {
        [SerializeField]
        private LevelButton _levelButton;

        [SerializeField]
        private Transform _transform;

        [SerializeField]
        private TMP_Text _text;

        public void Initialize(LevelData level)
        {
            _text.text = level.GroupName;
            foreach (var color in level.Colors)
            {
                var button = Instantiate(_levelButton, _transform);
                button.Initialize(level.Sprite, color);
            }
        }
    }
}