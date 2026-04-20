using CodeBase.Core.ScriptableObjects.Events;
using Zenject;

namespace CodeBase.Zenject.ProjectContextInstallers
{
    public class SOInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<EventUiRegistry>().FromScriptableObjectResource("Configs/EventUiRegistry").AsSingle();
        }
    }
}
