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
        public event Action<int, int> OnLevelButtonClicked;

        private void OnDestroy()
        {
            foreach (var button in _levelButtons)
            {
                button.OnButtonClicked -= LevelButtonClicked;
            }
        }

        public void Initialize(LevelData data, int index)
        {
            _text.text = data.GroupName;
            for (var level = 0; level < data.Colors.Count; level++)
            {
                Color color = data.Colors[level];
                var button = Instantiate(_levelButton, _transform);
                button.Initialize(data.Sprite, color, index, level);
                _levelButtons.Add(button);
                button.OnButtonClicked += LevelButtonClicked;
            }
        }

        private void LevelButtonClicked(int groupIndex, int level)
        {
            OnLevelButtonClicked?.Invoke(groupIndex, level);
        }
    }
}