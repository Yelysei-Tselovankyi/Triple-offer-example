using System.Collections.Generic;
using System.Collections.ObjectModel;
using CodeBase.Core.ScriptableObjects.Events;
using CodeBase.Core.UI;
using CodeBase.Services.EventManager;
using CodeBase.Services.ResourcesProvider;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace CodeBase.Scenes.Hub
{
    public class HubUi : MonoBehaviour
    {
        [SerializeField] private Transform _eventButtonsContainer;
        private IEventManager _eventManager;
        private IAddressableAssetProvider _addressableAssetProvider;
        private EventUiRegistry _eventUiRegistry;

        [Inject]
        private void Construct(IEventManager eventManager, IAddressableAssetProvider addressableAssetProvider,
            EventUiRegistry eventUiRegistry)
        {
            _eventManager = eventManager;
            _addressableAssetProvider = addressableAssetProvider;
            _eventUiRegistry = eventUiRegistry;
        }

        private void Start()
        {
            GetAndInstantiateEventButtons().Forget();
        }

        private async UniTaskVoid GetAndInstantiateEventButtons()
        {
            ReadOnlyCollection<string> eventsIDs = _eventManager.GetDeployableEventsIDs();
            foreach (string eventID in eventsIDs)
            {
                AssetReferenceGameObject eventButtonRef = _eventUiRegistry.GetButton(eventID);
                GameObject instantiatedButton =
                    await _addressableAssetProvider.InstantiateAsync(eventButtonRef, _eventButtonsContainer);
                BaseButton eventButton = instantiatedButton.GetComponent<BaseButton>();
                _eventManager.BindButtonToEvent(eventButton, eventID);
            }
        }
    }
}