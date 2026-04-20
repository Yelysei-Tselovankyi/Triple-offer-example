using System;
using System.Collections.Generic;
using CodeBase.Core;
using CodeBase.Core.UI;
using CodeBase.Services.ConfigService;
using CodeBase.Services.EventManager;
using CodeBase.Services.ProgressService;
using CodeBase.Services.ProgressService.SerializableTypes;
using CodeBase.Services.Timer;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace CodeBase.Events.TripleOfferEvent
{
    public class TripleOfferEvent : IEvent
    {
        private TripleOfferPresenter _presenter;
        private readonly DiContainer _container;
        private readonly IConfigService _configService;
        private readonly IProgressService _progressService;
        private readonly ITimer _timer;

        private BaseTripleOfferConfig _tripleOfferConfig;
        private TripleOfferProgress _tripleOfferProgress;
        private bool _isAlreadyStarted;
        private bool _canDeploy;
        private bool _isInitialized;
        private const string EVENT_ID = "TripleOfferEvent";

        public bool CanDeploy => _canDeploy;
        public string EventID => EVENT_ID;

        [Inject]
        public TripleOfferEvent(DiContainer container, IConfigService configService, IProgressService progressService, ITimer timer)
        {
            _container = container;
            _configService = configService;
            _progressService = progressService;
            _timer = timer;
        }

        public bool CanStart()
        {
            if (!_isInitialized) return false;
            if (AllOffersBought()) return false;
            if (_isAlreadyStarted) return false;
            if (!IsEventTimeReached()) return false;

            return true;
        }

        private bool AllOffersBought()
        {
            return _tripleOfferProgress.AreAllBought();
        }

        public void Initialize()
        {
            LoadEventProgress();
            AsyncInitialize().Forget();
        }

        public void Start()
        {
            CreatePresenter(_tripleOfferConfig, _tripleOfferProgress);
            StartTimer();
            _timer.TimerEnded += StartEventEnd;
            _isAlreadyStarted = true;
            _canDeploy = true;
        }

        private void StartTimer()
        {
            DateTime eventEndTime = DateTime.Parse(_tripleOfferConfig.EventEndTime).ToUniversalTime();
            _timer.StartUntil(eventEndTime);
        }

        public void BindOpenerButton(BaseButton instantiatedButton)
        {
            _presenter.BindOpenButton(instantiatedButton);
        }

        public void StartEventEnd()
        {
            EndEvent().Forget();
        }

        private async UniTaskVoid EndEvent()
        {
            if (_timer.IsRunning) _timer.Stop();
            _timer.TimerEnded -= StartEventEnd;
            await _presenter.AsyncEnd();
            _canDeploy = false;
            Debug.Log("Event ended");
        }

        private async UniTask AsyncInitialize()
        {
            await LoadConfig();
            _isInitialized = true;
        }

        private void LoadEventProgress()
        {
            Dictionary<string, BaseEventProgress> eventsProgress = _progressService.PlayerProgress.EventsProgress;
            
            if (eventsProgress.TryGetValue(EventID, out BaseEventProgress baseProgress))
            {
                _tripleOfferProgress = baseProgress as TripleOfferProgress;
            }

            if (_tripleOfferProgress == null)
            {
                _tripleOfferProgress = new TripleOfferProgress();
                eventsProgress.Add(EventID, _tripleOfferProgress);
            }
        }

        private async UniTask LoadConfig()
        {
            _tripleOfferConfig = await _configService.LoadAsync<BaseTripleOfferConfig>();
        }

        private void CreatePresenter(BaseTripleOfferConfig config, TripleOfferProgress progress)
        {
            _presenter = _container.Instantiate<TripleOfferPresenter>();
            _presenter.Initialize(config,  progress, EventID);
            _presenter.AllOffersBought += StartEventEnd;
        }

        private bool IsEventTimeReached()
        {
            DateTime now = DateTime.UtcNow;
            DateTime eventEndTime = DateTime.Parse(_tripleOfferConfig.EventEndTime).ToUniversalTime();
            DateTime eventStartTime = DateTime.Parse(_tripleOfferConfig.EventStartTime).ToUniversalTime();

            if (eventStartTime < now && now < eventEndTime)
            {
                Debug.Log("[Triple Offer Event] Event time reached]");
                return true;
            }
            
            return false;
        }
    }
}
