using Audio;
using Services.Coroutines;
using Services.Scenes;
using Services.Storage;
using UI.Managers;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class CommonInstaller : MonoInstaller
    {
        [SerializeField]
        private ProjectAudio _projectAudio;

        [SerializeField]
        private ProjectCanvas _projectCanvas;

        [SerializeField]
        private CoroutineServices _coroutineServices;

        public override void InstallBindings()
        {
            Container.Bind<IStorageService>().To<StorageService>().AsSingle();
            Container.Bind<ISceneLoader>().To<SceneLoader>().AsSingle();
            CoroutineInstaller();
            AudioInstaller();
            UIInstaller();
        }

        private void CoroutineInstaller()
        {
            Container.Bind<ICoroutineServices>().FromInstance(_coroutineServices).AsSingle().NonLazy();
        }

        private void AudioInstaller()
        {
            var projectAudio = Container.InstantiatePrefabForComponent<ProjectAudio>(_projectAudio);
            projectAudio.gameObject.transform.SetParent(null);
            Container.BindInterfacesAndSelfTo<ProjectAudio>().FromInstance(projectAudio).AsSingle();
        }

        private void UIInstaller()
        {
            var projectCanvas = Container.InstantiatePrefabForComponent<ProjectCanvas>(_projectCanvas);
            projectCanvas.gameObject.transform.SetParent(null);
            Container.BindInterfacesAndSelfTo<ProjectCanvas>().FromInstance(projectCanvas).AsSingle();
        }
    }
}