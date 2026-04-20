using System;

namespace CodeBase.Core.UI
{
    public interface IButton
    {
        event Action Pressed;
    }
}