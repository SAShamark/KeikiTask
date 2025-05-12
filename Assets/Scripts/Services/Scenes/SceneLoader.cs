using System;
using System.Collections;
using Services.Coroutines;
using UI.Managers;
using UI.Screens;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Services.Scenes
{
    public class SceneLoader : ISceneLoader, IDisposable
    {
        public SceneType SceneType { get; private set; }

        private readonly IUIManager _uiManager;
        private readonly ICoroutineServices _coroutineServices;

        private Coroutine _loadSceneCoroutine;

        public SceneLoader(IUIManager uiManager, ICoroutineServices coroutineServices)
        {
            _uiManager = uiManager;
            _coroutineServices = coroutineServices;
        }

        public void LoadScene(SceneType sceneType, ScreenTypes nextScreen = 0)
        {
            _loadSceneCoroutine = _coroutineServices.StartRoutine(LoadSceneCoroutine(sceneType, nextScreen));
            SceneType = sceneType;

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

                    if (progress >= 0.9f)
                    {
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