using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Screens.MainMenu
{
    public class LevelButton : MonoBehaviour
    {
        [SerializeField]
        private Button _button;

        [SerializeField]
        private Image _image;

        private int _groupIndex;
        private int _level;

        public event Action<int, int> OnButtonClicked;

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(ButtonClicked);
        }

        public void Initialize(Sprite sprite, Color color, int groupIndex, int level)
        {
            _groupIndex = groupIndex;
            _level = level;
            _image.sprite = sprite;
            _image.color = color;
            _button.onClick.AddListener(ButtonClicked);
        }

        private void ButtonClicked()
        {
            OnButtonClicked?.Invoke(_groupIndex, _level);
        }
    }
}