using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CodeBase.Core.UI;

namespace CodeBase.Services.EventManager
{
    public class EventManager : IEventManager 
    {
        private readonly List<IEvent> _events;
        private readonly List<string> _deployableEventIDs = new ();

        public EventManager(List<IEvent> events)
        {
            _events = events;
        }

        public void Tick()
        {
            UpdateEventStatus();
        }

        public ReadOnlyCollection<string> GetDeployableEventsIDs()
        {
            UpdateEventStatus();
            _deployableEventIDs.Clear();
            foreach (IEvent ev in _events.Where(ev => ev.CanDeploy))
            {
                _deployableEventIDs.Add(ev.EventID);
            }
            return _deployableEventIDs.AsReadOnly();
        }

        private void UpdateEventStatus()
        {
            foreach (IEvent ev in _events.Where(ev => ev.CanStart()))
            {
                ev.Start();
            }
        }

        public void BindButtonToEvent(BaseButton instantiatedButton, string eventID)
        {
            foreach (IEvent ev in _events.Where(ev => ev.EventID == eventID))
            {
                ev.BindOpenerButton(instantiatedButton);
            }
        }
    }
}