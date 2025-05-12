using System;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Popups
{
    public class BasePopup : BaseWindow
    {
        [SerializeField]
        private Button _closeButton;

        [SerializeField]
        private Animator _animator;

        private readonly int _enabled = Animator.StringToHash("Enabled");
        private readonly int _disabled = Animator.StringToHash("Disabled");
        private string _defaultClipName = "PopupOn";
        protected PopupData _data;

        public PopupModelData PopupData { get; set; }


        public virtual void SetData(PopupData data = null)
        {
            if (data != null)
            {
                _data = data;
            }
        }

        public virtual void Show()
        {
            gameObject.SetActive(true);
            if (_animator != null)
            {
                _defaultClipName = _animator.runtimeAnimatorController.animationClips[0].name;
                _animator.SetTrigger(_enabled);
            }

            if (_safeAreaFitter != null)
            {
                _safeAreaFitter.FitToSafeArea();
            }

            if (_closeButton != null)
            {
                _closeButton.onClick.RemoveAllListeners();

                _closeButton.onClick.AddListener(CloseTrigger);
            }

            Debug.Log($"{gameObject.name} popup showed");
        }

        public virtual async void CloseTrigger()
        {
            if (_animator != null)
            {
                await HideAnimation();
            }

            UIManager.PopupsManager.HidePopup(PopupData.PopupType);
        }

        private async Task HideAnimation()
        {
            _animator.SetTrigger(_disabled);
            int clipTime = Mathf.FloorToInt(ClipDataProvider.ClipDuration(_animator, _defaultClipName) *
                                            ValueConstants.MILLISECONDS_IN_SECOND);
            await Task.Delay(clipTime);

            try
            {
                await Task.Delay(clipTime);
            }
            catch (TaskCanceledException exception)
            {
                Debug.Log("Popup hide exception: " + exception);
            }
        }
    }

    public class PopupData
    {
    }
    
    public static class ClipDataProvider
    {
        public static float ClipDuration(Animator animator, string clipName)
        {
            AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
            AnimationClip targetClip = clips.FirstOrDefault(animationClip => animationClip.name == clipName);
            if (targetClip != null)
            {
                return targetClip.length;
            }
            throw new ArgumentOutOfRangeException("Can't find clip of name " + clipName);
        }
    }
    public struct ValueConstants
    {
        public const float EPSILON = 0.001f;
        public const int MILLISECONDS_IN_SECOND = 1000;
    }
}