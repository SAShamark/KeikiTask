using System;
using System.Collections.Generic;
using TMPro;
using UI.Screens.MainMenu.Data;
using UnityEngine;
using UnityEngine.UI;

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
        
        private readonly List<LevelButton> _levelButtons = new();
        public event Action OnLevelButtonClicked;

        private void OnDestroy()
        {
            foreach (var button in _levelButtons)
            {
                button.OnButtonClicked -= LevelButtonClicked;
            }
        }

        public void Initialize(LevelData level)
        {
            _text.text = level.GroupName;
            foreach (var color in level.Colors)
            {
                var button = Instantiate(_levelButton, _transform);
                button.Initialize(level.Sprite, color);
                _levelButtons.Add(button);
                button.OnButtonClicked += LevelButtonClicked;
            }
        }

        private void LevelButtonClicked()
        {
            OnLevelButtonClicked?.Invoke();
        }
    }
}