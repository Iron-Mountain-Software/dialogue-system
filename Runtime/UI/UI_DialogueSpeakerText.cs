using UnityEngine;
using UnityEngine.UI;

namespace SpellBoundAR.DialogueSystem.UI
{
    [RequireComponent(typeof(Text))]
    public class UI_DialogueSpeakerText : MonoBehaviour
    {
        private Text _text;

        private void Awake()
        {
            _text = GetComponent<Text>();
            if (_text) _text.text = string.Empty;
            ConversationUI.OnDialogueInteractionStarted += OnDialogueInteractionStarted;
            ConversationUI.OnDialogueInteractionEnded += OnDialogueInteractionEnded;
        }

        private void OnDestroy()
        {
            ConversationUI.OnDialogueInteractionStarted -= OnDialogueInteractionStarted;
            ConversationUI.OnDialogueInteractionEnded -= OnDialogueInteractionEnded;
        }

        private void OnDialogueInteractionStarted(Conversation conversation)
        {
            if (!conversation || !conversation.Speaker) return;
            if (_text) _text.text = conversation.Speaker.Name;
        }
    
        private void OnDialogueInteractionEnded(Conversation conversation)
        {
            if (_text) _text.text = string.Empty;
        }
    }
}
