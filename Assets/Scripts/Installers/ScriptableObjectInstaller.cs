using UnityEngine;
using Zenject;

namespace Installers
{
    public class ScriptableObjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
        }


        private void InstallBindingAsSingle<T>(T scriptableObject) where T : ScriptableObject
        {
            Container.BindInterfacesAndSelfTo<T>()
                .FromNewScriptableObject(scriptableObject)
                .AsSingle();
        }
    }
}