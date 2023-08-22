using UnityEngine;
using UnityEngine.UI;

namespace SpellBoundAR.DialogueSystem.UI
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Text))]
    public class ConversationUISpeakerNameText : MonoBehaviour
    {
        [Header("Cache")]
        private Text _text;
        private ConversationUI _conversationUI;

        private void Awake()
        {
            _text = GetComponent<Text>();
            _conversationUI = GetComponentInParent<ConversationUI>();
        }

        private void OnEnable()
        {
            if (_conversationUI) _conversationUI.OnConversationChanged += Refresh; 
            Refresh();
        }

        private void OnDisable()
        {
            if (_conversationUI) _conversationUI.OnConversationChanged -= Refresh; 
        }

        private void Refresh()
        {
            if (!_text) return;
            _text.text = _conversationUI
                         && _conversationUI.CurrentConversation
                         && _conversationUI.CurrentConversation.Speaker != null
                ? _conversationUI.CurrentConversation.Speaker.SpeakerName
                : string.Empty;
        }
    }
}