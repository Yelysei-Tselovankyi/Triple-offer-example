using CodeBase.Core.UI;
using Zenject;

namespace CodeBase.Services.EventManager
{
    public interface IEvent : IInitializable
    {
        string EventID { get; }
        bool CanDeploy { get; }
        bool CanStart();
        void Start();
        void StartEventEnd();
        void BindOpenerButton(BaseButton instantiatedButton);
    }
}