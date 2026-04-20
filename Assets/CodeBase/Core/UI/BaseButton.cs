using System;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.Core.UI
{
    public class BaseButton : MonoBehaviour, IButton
    {
        [SerializeField] private Button _button;
        public event Action Pressed;
        public event Action Destroyed;

        public void Lock()
        {
            _button.interactable = false;
        }

        public void Unlock()
        {
            _button.interactable = true;
        }
        
        protected virtual void Awake()
        {
            _button.onClick.AddListener(OnButtonClicked);
        }

        protected virtual void OnButtonClicked()
        {
            Pressed?.Invoke();
        }

        protected virtual void OnDestroy()
        {
            Destroyed?.Invoke();
            _button.onClick.RemoveListener(OnButtonClicked);
        }
    }
}