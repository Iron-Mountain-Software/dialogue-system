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
            if (_conversationUI) _conversationUI.OnSpeakerChanged += Refresh; 
            Refresh();
        }

        private void OnDisable()
        {
            if (_conversationUI) _conversationUI.OnSpeakerChanged -= Refresh; 
        }

        private void Refresh()
        {
            if (!_text) return;
            _text.text = _conversationUI
                         && _conversationUI.CurrentSpeaker != null
                ? _conversationUI.CurrentSpeaker.SpeakerName
                : string.Empty;
        }
    }
}