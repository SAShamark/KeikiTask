using Audio;
using Audio.Data;
using UI.Managers;
using UI.Managers.Components;
using UnityEngine;
using Zenject;

namespace UI
{
    public abstract class BaseWindow : MonoBehaviour
    {
        [SerializeField]
        protected Canvas _canvas;

        [SerializeField]
        protected SafeAreaFitter _safeAreaFitter;

        protected IUIManager UIManager { get; private set; }
        protected IAudioManager AudioManager { get; private set; }

        [Inject]
        private void Construct(IUIManager uiManager, IAudioManager audioManager)
        {
            UIManager = uiManager;
            AudioManager = audioManager;
        }

        protected void ButtonClickedSound()
        {
            AudioManager.Play(AudioGroupType.UiSounds, "Button");
        }
        protected void CloseButtonClickedSound()
        {
            AudioManager.Play(AudioGroupType.UiSounds, "CloseButton");
        }
    }
}