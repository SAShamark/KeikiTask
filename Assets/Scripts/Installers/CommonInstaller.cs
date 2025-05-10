using UI.Managers;
using UnityEngine;
using Zenject;

namespace Audio
{
    public class CommonInstaller : MonoInstaller
    {
        [SerializeField]
        private ProjectAudio _projectAudio;

        [SerializeField]
        private ProjectCanvas _projectCanvas;

        public override void InstallBindings()
        {
            AudioInstaller();
            UIInstaller();
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