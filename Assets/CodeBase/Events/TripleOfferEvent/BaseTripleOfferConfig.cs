using System;
using CodeBase.Services.ConfigService;

namespace CodeBase.Events.TripleOfferEvent
{
    [Serializable]
    public class BaseTripleOfferConfig : IConfig
    {
        public string EventStartTime;
        public string EventEndTime;
        public string FirstOfferId;
        public string SecondOfferId;
        public string ThirdOfferId;
    }
}