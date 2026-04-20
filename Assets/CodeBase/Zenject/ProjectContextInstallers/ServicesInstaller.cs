using CodeBase.Services.ConfigService;
using CodeBase.Services.OffersService;
using CodeBase.Services.ProgressService;
using CodeBase.Services.PurchaseService;
using CodeBase.Services.ResourcesProvider;
using CodeBase.Services.Timer;
using CodeBase.Services.UiManager;
using Zenject;

namespace CodeBase.Zenject.ProjectContextInstallers
{
    public class ProjectContextInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IAddressableAssetProvider>().To<AddressableAssetProvider>().AsSingle();
            Container.Bind<IUiManager>().To<UiManager>().AsSingle();
            Container.Bind<IConfigService>().To<FirebaseConfigService>().AsSingle();
            Container.Bind<IOfferService>().To<OffersService>().AsSingle();
            Container.Bind<IProgressService>().To<LocalProgressService>().AsSingle();
            Container.Bind<IPurchaseService>().To<MockPurchaseService>().AsSingle();
            
            Container.Bind<ITimer>().To<Timer>().AsTransient();
        }
    }
}
