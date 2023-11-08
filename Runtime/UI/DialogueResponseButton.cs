using System;
using SpellBoundAR.DialogueSystem.Responses;
using UnityEngine;
using UnityEngine.UI;

namespace SpellBoundAR.DialogueSystem.UI
{
    public class DialogueResponseButton : MonoBehaviour
    {
        public event Action OnBasicResponseChanged;

        [Header("References")]
        [SerializeField] private Image buttonImagePrimary;
        [SerializeField] private Image buttonImageSecondary;
        [SerializeField] private Text text;

        [Header("Cache")]
        private BasicResponse _basicResponse;
        private ConversationPlayer _conversationUI;

        public BasicResponse BasicResponse
        {
            get => _basicResponse;
            private set
            {
                if (_basicResponse == value) return;
                _basicResponse = value;
                OnBasicResponseChanged?.Invoke();
            }
        }

        public virtual void Initialize(BasicResponse basicResponse, ConversationPlayer conversationUI)
        {
            if (basicResponse == null) { Destroy(gameObject); return; }
            BasicResponse = basicResponse;
            _conversationUI = conversationUI;
            if (buttonImagePrimary) buttonImagePrimary.color = _basicResponse.Style.ButtonColorPrimary;
            if (buttonImageSecondary) buttonImageSecondary.color = _basicResponse.Style.ButtonColorSecondary;
            if (text)
            {
                text.color = _basicResponse.Style.TextColor;
                text.text = _basicResponse.Text;
            }
        }

        public void OnClick()
        {
            _basicResponse?.ExecuteResponse();
        }
    }
}