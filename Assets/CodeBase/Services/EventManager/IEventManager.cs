using System.Collections.ObjectModel;
using CodeBase.Core.UI;
using Zenject;

namespace CodeBase.Services.EventManager
{
    public interface IEventManager : ITickable
    {
        ReadOnlyCollection<string> GetDeployableEventsIDs();
        void BindButtonToEvent(BaseButton instantiatedButton, string eventID);
    }
}