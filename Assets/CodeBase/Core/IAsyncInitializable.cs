using System;

namespace CodeBase.Core
{
    public interface IAsyncInitializable
    {
        bool IsInitialized { get; }
    }
}