using System;
using Cysharp.Threading.Tasks;

namespace CodeBase.Services.Timer
{
    public interface ITimer
    {
        event Action TimerEnded;
        bool IsRunning { get; }

        UniTask StartUntil(DateTime endTime);
        void Stop();
    }
}