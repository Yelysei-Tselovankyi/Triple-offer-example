using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CodeBase.Core.UI
{
    public class BaseWindow : MonoBehaviour, IWindow
    {
        public virtual void PlayOpenAnimation()
        {
        }

        public virtual async UniTask PlayCloseAnimation()
        {
        }
    }
}