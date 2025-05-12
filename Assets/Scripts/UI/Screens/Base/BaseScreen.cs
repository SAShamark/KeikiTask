using UnityEngine;
using UnityEngine.UI;

namespace UI.Screens.Base
{
    public class BaseScreen : BaseWindow
    {
        [SerializeField] protected Button _backButton;

        public ScreenModelData ScreenData { get; set; }
        
        public virtual void Initialize()
        {
            if (_backButton != null)
            {
                _backButton.onClick.RemoveAllListeners();

                _backButton.onClick.AddListener(UIManager.ScreensManager.ShowPreviousScreen);
            }
        }

        public virtual void Show()
        {
            if (_safeAreaFitter != null)
            {
                _safeAreaFitter.FitToSafeArea();
            }
            
            _canvas.enabled = true;
        }

        public virtual void Hide()
        {
            _canvas.enabled = false;
        }
    }
}