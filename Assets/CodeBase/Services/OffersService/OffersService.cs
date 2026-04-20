using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Core;
using CodeBase.Services.ConfigService;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace CodeBase.Services.OffersService
{
    public class OffersService : IOfferService
    {
        public bool IsInitialized { get; private set; }

        private readonly IConfigService _configService;
        private Dictionary<string, OfferData> _offers;


        [Inject]
        public OffersService(IConfigService configService)
        {
            _configService = configService;
        }

        public void Initialize()
        {
            AsyncLoad().Forget();
        }

        private async UniTaskVoid AsyncLoad()
        {
           await LoadOffersAsync(); 
           IsInitialized = true; 
        }

        public OfferData GetOfferWithID(string id)
        {
            if (!IsInitialized)
            {
                Debug.LogWarning("Offers still downloading, try later");
                return null;
            }
            return _offers[id];
        }

        private async UniTask LoadOffersAsync()
        {
            AllOffersConfig allOffersConfig = await _configService.LoadAsync<AllOffersConfig>();
            _offers = allOffersConfig.Offers.ToDictionary(o => o.Id, o => o.OfferData);
        }
    }
}