using CodeBase.Events.TripleOfferEvent;
using CodeBase.Services.EventManager;
using Zenject;

namespace CodeBase.Zenject.ProjectContextInstallers
{
    public class EventManagerInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IEvent>().To<TripleOfferEvent>().AsSingle();
            
            Container.Bind<IEventManager>().To<EventManager>().AsSingle();
        }
    }
}