using System;
using CodeBase.Core.UI;
using UnityEngine;

namespace CodeBase.Events.Infrastructure
{
    public class BaseOfferCard : MonoBehaviour, IOfferCard
    {
        [SerializeField] private string _offerId;
        [SerializeField] private BaseButton _offerButton;
        [SerializeField] private BaseText _offerDescription;
        [SerializeField] private BaseText _offerPrice;

        public event Action<string> OfferPurchaseButtonPressed;
        public string ID => _offerId;

        public void LockButton()
        {
            _offerButton.Lock();    
        }

        public void UnlockButton()
        {
            _offerButton.Unlock();
        }
        
        public void SetUpDescription(string offerDescription)
        {
            _offerDescription.SetText(offerDescription);
        }

        public void SetUpPrice(float offerPrice)
        {
            _offerPrice.SetText($"{offerPrice}");
        }
        private void Awake()
        {
            _offerButton.Pressed += OfferPurchaseInvoker;
        }

        private void OfferPurchaseInvoker()
        {
            OfferPurchaseButtonPressed?.Invoke(_offerId);
        }

        public void MakeOfferPurchased()
        {
            _offerButton.gameObject.SetActive(false);
        }
    }
}