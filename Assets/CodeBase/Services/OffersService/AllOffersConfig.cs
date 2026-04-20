using System;
using System.Collections.Generic;
using CodeBase.Services.ConfigService;

namespace CodeBase.Services.OffersService
{
    [Serializable]
    public class AllOffersConfig : IConfig
    {
        public List<Offer> Offers;
    }
}