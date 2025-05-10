using UI.Managers.Components;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Screens
{
    public class IntroScreen : MonoBehaviour
    {
        [SerializeField]
        protected SafeAreaFitter _safeAreaFitter;

        [SerializeField]
        private Image _image;

        public void Init()
        {
            if (_safeAreaFitter != null)
            {
                _safeAreaFitter.FitToSafeArea();
            }

            UpdateProgress(0);
        }

        public void UpdateProgress(float progress)
        {
            /*_image.DOKill();
            _image.DOFillAmount(progress, 0.5f).SetEase(Ease.InOutQuad);*/
        }
    }
}