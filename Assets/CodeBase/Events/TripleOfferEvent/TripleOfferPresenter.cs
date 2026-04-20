using System;
using System.Collections.Generic;
using CodeBase.Core.UI;
using CodeBase.Services.EventManager;
using CodeBase.Services.OffersService;
using CodeBase.Services.ProgressService;
using CodeBase.Services.PurchaseService;
using CodeBase.Services.UiManager;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace CodeBase.Events.TripleOfferEvent
{
    public class TripleOfferPresenter : IEventPresenter
    {
        public event Action AllOffersBought;
        private BaseTripleOfferConfig _config;

        private const string FIRST_OFFER_CARD_ID = "OfferCard1";
        private const string SECOND_OFFER_CARD_ID = "OfferCard2";
        private const string THIRD_OFFER_CARD_ID = "OfferCard3";

        private readonly IUiManager _uiManager;
        private readonly IOfferService _offerService;
        private readonly IPurchaseService _purchaseService;
        private readonly IProgressService _progressService;
        private BaseButton _openButton;
        private TripleOfferWindow _window;
        private TripleOfferProgress _progress;

        private Dictionary<string, string> _cardToOfferId;
        private string _eventId;

        [Inject]
        public TripleOfferPresenter(IUiManager uiManager, IOfferService offerService, IPurchaseService purchaseService, IProgressService progressService)
        {
            _offerService = offerService;
            _purchaseService = purchaseService;
            _uiManager = uiManager;
            _progressService = progressService;
        }

        public void Initialize(BaseTripleOfferConfig tripleOfferConfig, TripleOfferProgress progress, string eventId)
        {
            _config = tripleOfferConfig;
            _progress = progress;
            _eventId = eventId;
            
            _cardToOfferId = new ()
            {
                {FIRST_OFFER_CARD_ID, _config.FirstOfferId},
                {SECOND_OFFER_CARD_ID, _config.SecondOfferId},
                {THIRD_OFFER_CARD_ID, _config.ThirdOfferId}
            };
        }

        public void BindOpenButton(BaseButton button)
        {
            _openButton = button;
            SubscribeToOpenButtonEvents();
        }

        public async UniTask AsyncEnd()
        {
            if (_openButton != null)
            {
                Object.Destroy(_openButton.gameObject);
            }
            if (_window != null)
            {
                await CloseWindow();
            }
        }

        private void OnOpenButtonPressed()
        {
            OpenWindow().Forget();
        }

        private async UniTask OpenWindow()
        {
            _openButton.Lock();
            _window = await _uiManager.ShowEventWindow<TripleOfferWindow>(_eventId);
            SubscribeToWindowEvents();
            SetUpWindowValues();
            SetUpPurchasedCards();
            _openButton.Unlock();
        }
            
        private void OnOfferPurchaseButtonPressed(string id)
        {
            OfferPurchasedLogic(id).Forget();
        }

        private async UniTask OfferPurchasedLogic(string id)
        {
            _window.LockCloseButton();
            _window.LockOffersButtons();

            if (! await TryToMakePurchase(id))
            {
                HandleOfferPurchaseFailed();
                return;
            }

            await UpdateOfferProgress(id);

            _window.MakeOfferPurchased(id);
            _window.UnlockOffersButtons();
            _window.UnlockCloseButton();
            
            if (_progress.AreAllBought())
            {
                AllOffersBought?.Invoke();
            }
        }

        private async UniTask UpdateOfferProgress(string id)
        {
            switch (id)
            {
                case FIRST_OFFER_CARD_ID:
                    _progress.IsFirstOfferBought = true;
                    break;
                case SECOND_OFFER_CARD_ID:
                    _progress.IsSecondOfferBought = true;
                    break;
                case THIRD_OFFER_CARD_ID:
                    _progress.IsThirdOfferBought = true;
                    break;
            }

            await _progressService.SaveProgress();
        }

        private async UniTask<bool> TryToMakePurchase(string id)
        {
            bool isBought = id switch
            {
                FIRST_OFFER_CARD_ID => await _purchaseService.TryBuyOffer(_config.FirstOfferId),
                SECOND_OFFER_CARD_ID => await _purchaseService.TryBuyOffer(_config.SecondOfferId),
                THIRD_OFFER_CARD_ID => await _purchaseService.TryBuyOffer(_config.ThirdOfferId),
                _ => false
            };

            return isBought;
        }

        private void SetUpWindowValues()
        {
            foreach ((string cardId, string offerId) in _cardToOfferId)
            {
                OfferData offer = _offerService.GetOfferWithID(offerId);
                _window.SetUpCardValues(cardId, offer.OfferDescription, offer.OfferPrice);
            }
        }

        private void SetUpPurchasedCards()
        {
            if (_progress.IsFirstOfferBought)
            {
                _window.MakeOfferPurchased(FIRST_OFFER_CARD_ID);
            }

            if (_progress.IsSecondOfferBought)
            {
                _window.MakeOfferPurchased(SECOND_OFFER_CARD_ID);
            }

            if (_progress.IsThirdOfferBought)
            {
                _window.MakeOfferPurchased(THIRD_OFFER_CARD_ID);
            }
        }

        private void HandleOfferPurchaseFailed()
        {
            Debug.Log("Some error with buying offer!");
        }

        private void OnOpenButtonDestroy()
        {
            UnsubscribeFromOpenButtonEvents();
        }

        private void OnCloseButtonPressed()
        {
            CloseWindow().Forget();
        }

        private async UniTask CloseWindow()
        {
            UnsubscribeFromWindowEvents();
            await _uiManager.CloseEventWindow(_eventId);
        }

        private void SubscribeToOpenButtonEvents()
        {
            _openButton.Pressed += OnOpenButtonPressed;
            _openButton.Destroyed += OnOpenButtonDestroy;
        }

        private void UnsubscribeFromOpenButtonEvents()
        {
            _openButton.Pressed -= OnOpenButtonPressed;
            _openButton.Destroyed -= OnOpenButtonDestroy;
        }

        private void SubscribeToWindowEvents()
        {
            _window.CloseButtonPressed += OnCloseButtonPressed;
            _window.OfferPurchaseButtonPressed += OnOfferPurchaseButtonPressed;
        }

        private void UnsubscribeFromWindowEvents()
        {
            _window.CloseButtonPressed -= OnCloseButtonPressed;
            _window.OfferPurchaseButtonPressed -= OnOfferPurchaseButtonPressed;
        }
    }
}