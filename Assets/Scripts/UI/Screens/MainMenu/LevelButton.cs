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

        public void Initialize(Sprite sprite, Color color)
        {
            _image.sprite = sprite;
            _image.color = color;
        }
    }
}