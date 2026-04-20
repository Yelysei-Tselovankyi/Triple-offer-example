using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CodeBase.Services.Timer
{
    public class Timer : ITimer
    {
        public bool IsRunning { get; private set; }
        public event Action TimerEnded;
        private CancellationTokenSource _cts;

        public async UniTask StartUntil(DateTime endTime)
        {
            if (IsRunning)
            {
                Debug.LogWarning("[Timer] Timer is already running.");
                return;
            }

            IsRunning = true;
            _cts = new CancellationTokenSource();

            try
            {
                DateTime now = DateTime.UtcNow;
                TimeSpan remaining = endTime.ToUniversalTime() - now;

                if (remaining.TotalSeconds <= 0)
                {
                    Debug.Log("[Timer] End time has already passed.");
                    TimerEnded?.Invoke();
                    IsRunning = false;
                    return;
                }

                var secondsRemaining = remaining.TotalSeconds;
                Debug.Log(secondsRemaining);
                await UniTask.Delay(TimeSpan.FromSeconds(secondsRemaining), cancellationToken: _cts.Token);

                IsRunning = false;
                TimerEnded?.Invoke();
            }
            catch (OperationCanceledException)
            {
                Debug.Log("[Timer] Timer was cancelled.");
            }
        }

        public void Stop()
        {
            if (IsRunning)
            {
                IsRunning = false;
                _cts?.Cancel();
                _cts?.Dispose();
                _cts = null;
            }
        }
    }
}
