using System;
using System.Collections;
using Audio;
using Audio.Data;
using Services.Coroutines;
using UI.Managers;
using UI.Popups;
//using UI.Popups.Variables.Loading;
using UI.Screens;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Services.Scenes
{
    public class SceneLoader : ISceneLoader, IDisposable
    {
        public SceneType SceneType { get; private set; }

        private readonly IAudioManager _audioManager;
        private readonly IUIManager _uiManager;
        private readonly ICoroutineServices _coroutineServices;

        //private LoadingPopup _loadingPopup;
        private Coroutine _loadSceneCoroutine;

        public SceneLoader(IAudioManager audioManager, IUIManager uiManager, ICoroutineServices coroutineServices)
        {
            _audioManager = audioManager;
            _uiManager = uiManager;
            _coroutineServices = coroutineServices;
        }

        public void LoadScene(SceneType sceneType, ScreenTypes nextScreen = 0, bool isLoading = true)
        {
            if (isLoading)
            {
                _uiManager.PopupsManager.ShowPopup(PopupTypes.Loading);
                /*if (_uiManager.PopupsManager.GetPopup(PopupTypes.Loading) is LoadingPopup popup)
                {
                    popup.Init();
                    _loadingPopup = popup;
                }*/
            }

            _loadSceneCoroutine = _coroutineServices.StartRoutine(LoadSceneCoroutine(sceneType, nextScreen));
            SceneType = sceneType;

            switch (sceneType)
            {
                case SceneType.MainMenu:
                    _audioManager.Play(AudioGroupType.Music, "Menu1");
                    break;
                case SceneType.GamePlay:
                    _audioManager.Play(AudioGroupType.Music, "GamePlay1");
                    break;
            }

            Debug.Log($"{sceneType} scene loaded");
        }

        private IEnumerator LoadSceneCoroutine(SceneType sceneType, ScreenTypes nextScreen)
        {
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneType.ToString());
            if (asyncOperation != null)
            {
                asyncOperation.allowSceneActivation = false;
                float progress = 0;

                while (!asyncOperation.isDone)
                {
                    progress = Mathf.MoveTowards(progress, asyncOperation.progress, Time.deltaTime);
                    // _loadingPopup?.UpdateProgress(progress);

                    if (progress >= 0.9f)
                    {
                        //_loadingPopup?.UpdateProgress(1);
                        if (nextScreen != 0)
                        {
                            _uiManager.ScreensManager.ShowScreen(nextScreen);
                        }

                        asyncOperation.allowSceneActivation = true;
                    }

                    yield return null;
                }
            }
        }

        public void Dispose()
        {
            _coroutineServices.StopRoutine(_loadSceneCoroutine);
        }
    }
    
}