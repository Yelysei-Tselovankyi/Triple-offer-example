using CodeBase.Core.UI;
using Cysharp.Threading.Tasks;

namespace CodeBase.Services.UiManager
{
    public interface IUiManager
    {
        UniTask<T> ShowWindow<T>(string id = null) where T : BaseWindow;
        UniTask CloseWindow<T>(string id = null) where T : BaseWindow;
        UniTask<T> ShowEventWindow<T>(string eventId) where T : BaseWindow;
        UniTask CloseEventWindow(string eventId);
    }
}