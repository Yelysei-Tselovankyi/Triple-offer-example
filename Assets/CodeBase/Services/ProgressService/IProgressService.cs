using System;
using CodeBase.Core;
using CodeBase.Services.ProgressService.SerializableTypes;
using Cysharp.Threading.Tasks;
using Zenject;

namespace CodeBase.Services.ProgressService
{
    public interface IProgressService : IInitializable, IAsyncInitializable
    {
        PlayerProgress PlayerProgress { get; }
        UniTask SaveProgress();
    }
}