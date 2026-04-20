using CodeBase.Services.EventManager;
using CodeBase.Services.OffersService;
using CodeBase.Services.ProgressService;
using Zenject;

namespace CodeBase.Zenject.ProjectContextInstallers
{
    public class InitializableClassesInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IInitializable>().To<IOfferService>().FromResolve();
            Container.Bind<IInitializable>().To<IProgressService>().FromResolve();
            Container.Bind<IInitializable>().To<IEvent>().FromResolve();
        }
    }
}