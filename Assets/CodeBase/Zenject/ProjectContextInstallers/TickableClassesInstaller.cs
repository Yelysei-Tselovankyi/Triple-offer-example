using CodeBase.Services.EventManager;
using Zenject;

namespace CodeBase.Zenject.ProjectContextInstallers
{
    public class TickableClassesInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<ITickable>().To<IEventManager>().FromResolve();
        }
    }
}