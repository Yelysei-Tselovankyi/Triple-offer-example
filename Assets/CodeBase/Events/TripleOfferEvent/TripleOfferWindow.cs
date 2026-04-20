using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Core.UI;
using CodeBase.Events.Infrastructure;
using UnityEngine;

namespace CodeBase.Events.TripleOfferEvent
{
    public class TripleOfferWindow : BaseWindow
    {
        [SerializeField] private BaseButton _closeButton;
        [SerializeField] private List<BaseOfferCard> _offersCards;
        
        public Action CloseButtonPressed;
        public Action<string> OfferPurchaseButtonPressed;

        public void SetUpCardValues(string offerCardId, string description, float price )
        {
            BaseOfferCard card = GetOfferCardById(offerCardId);
            card.SetUpDescription(description);
            card.SetUpPrice(price);
        }

        public void MakeOfferPurchased(string id)
        {
            BaseOfferCard offerCard = GetOfferCardById(id);
            offerCard.MakeOfferPurchased();
        }

        public void LockOffersButtons()
        {
            foreach (BaseOfferCard offer in _offersCards)
            {
                offer.LockButton();
            }
        }

        public void UnlockOffersButtons()
        {
            foreach (BaseOfferCard offer in _offersCards)
            {
                offer.UnlockButton();
            }
        }

        public void LockCloseButton()
        {
            _closeButton.Lock();
        }

        public void UnlockCloseButton()
        {
            _closeButton.Unlock();
        }

        private void Awake()
        {
            SubscribeToEvents();
        }

        private void OnDestroy()
        {
            UnsubscribeFromEvents();
        }

        private void PurchaseButtonPressedInvoker(string cardId)
        {
            OfferPurchaseButtonPressed?.Invoke(cardId);
        }

        private void CloseButtonPressedInvoker()
        {
            CloseButtonPressed?.Invoke(); 
        }

        private BaseOfferCard GetOfferCardById(string id)
        {
            if (_offersCards.Count(o => o?.ID == id) > 1)
            {
                Debug.LogError($"Multiple OffersCard found with ID: {id}. Returning the first one.");
            }

            BaseOfferCard offerCard = _offersCards.FirstOrDefault(o => o?.ID == id);
            
            if (offerCard == null)
            {
                throw new KeyNotFoundException($"No OffersCard found with ID: {id}");
            } 
            
            return offerCard;
        }

        private void SubscribeToEvents()
        {
            _closeButton.Pressed += CloseButtonPressedInvoker;
            foreach (BaseOfferCard offer in _offersCards)
            {
                offer.OfferPurchaseButtonPressed += PurchaseButtonPressedInvoker;
            }
        }

        private void UnsubscribeFromEvents()
        {
            _closeButton.Pressed -= CloseButtonPressedInvoker;
            foreach (BaseOfferCard offer in _offersCards)
            {
                offer.OfferPurchaseButtonPressed -= PurchaseButtonPressedInvoker;
            }
        }
    }
}
