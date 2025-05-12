using System.Collections;
using System.Collections.Generic;
using Services.Scenes;
using UI.Screens;
using UnityEngine;
using Zenject;

namespace Services
{
    public class ApplicationStartManager : MonoBehaviour
    {
        [SerializeField]
        private SceneContext _sceneContext;

        private List<ILoadingInitialization> _initList;
        private ISceneLoader _sceneLoader;
        private Coroutine _initializeCoroutine;

        [Inject]
        private void Construct(List<ILoadingInitialization> initList, ISceneLoader sceneLoader)
        {
            _initList = initList;
            _sceneLoader = sceneLoader;
        }

        private void Start()
        {
            _initializeCoroutine = StartCoroutine(Initialize());
        }

        private void OnDestroy()
        {
            if (_initializeCoroutine != null)
            {
                StopCoroutine(_initializeCoroutine);
            }
        }

        private IEnumerator Initialize()
        {
            RunSceneContext();
            yield return InitializeLoadingComponents();
            ShowNextScene();
        }

        private void RunSceneContext() => _sceneContext.Run();


        private IEnumerator InitializeLoadingComponents()
        {
            foreach (ILoadingInitialization loadingInitialization in _initList)
            {
                loadingInitialization.Init();
                yield return null;
            }
        }

        private void ShowNextScene() => _sceneLoader.LoadScene(SceneType.Menu, ScreenTypes.MainMenu);
    }
}