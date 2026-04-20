using System;

namespace CodeBase.Events.Infrastructure
{
    public interface IOfferCard
    {
        event Action<string> OfferPurchaseButtonPressed;
        string ID { get; }
        void SetUpDescription(string offerDescription);
        void SetUpPrice(float offerPrice);
    }
}