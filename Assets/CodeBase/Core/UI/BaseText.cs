using TMPro;
using UnityEngine;

namespace CodeBase.Core.UI
{
    public class BaseText : MonoBehaviour, IBaseText
    {
        [SerializeField] private TMP_Text _text;
        
        public void SetText(string text)
        {
            _text.text = text;
        }
    }
}