using System;
using CodeBase.Services.ProgressService.SerializableTypes;

namespace CodeBase.Events.TripleOfferEvent
{
    [Serializable]
    public class TripleOfferProgress : BaseEventProgress
    {
        public bool IsFirstOfferBought;
        public bool IsSecondOfferBought;
        public bool IsThirdOfferBought;

        public bool AreAllBought()
        {
            return IsFirstOfferBought && IsSecondOfferBought && IsThirdOfferBought;
        }
    }
}