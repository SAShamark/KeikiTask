using UI.Screens.MainMenu.Data;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class ScriptableObjectInstaller : MonoInstaller
    {
        [SerializeField]
        private LevelsConfig _levelsConfig;
        public override void InstallBindings()
        {
            InstallBindingAsSingle(_levelsConfig);
        }


        private void InstallBindingAsSingle<T>(T scriptableObject) where T : ScriptableObject
        {
            Container.BindInterfacesAndSelfTo<T>()
                .FromNewScriptableObject(scriptableObject)
                .AsSingle();
        }
    }
}