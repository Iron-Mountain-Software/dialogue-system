using System;
using UnityEngine;
using UnityEngine.UI;

namespace SpellBoundAR.DialogueSystem.UI
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Text))]
    public class SpeakerNameText : MonoBehaviour
    {
        [SerializeField] private ConversationPlayer conversationPlayer;
        [SerializeField] private Text text;

        private void Awake()
        {
            if (!conversationPlayer) conversationPlayer = GetComponentInParent<ConversationPlayer>();
            if (!text) text = GetComponent<Text>();
        }

        private void OnValidate()
        {
            if (!conversationPlayer) conversationPlayer = GetComponentInParent<ConversationPlayer>();
            if (!text) text = GetComponent<Text>();
        }

        private void OnEnable()
        {
            Refresh();
            if (conversationPlayer) conversationPlayer.OnDialogueLinePlayed += OnDialogueLinePlayed; 
        }

        private void OnDisable()
        {
            if (conversationPlayer) conversationPlayer.OnDialogueLinePlayed -= OnDialogueLinePlayed; 
        }

        private void Refresh()
        {
            if (!text) return;
            text.text = conversationPlayer && conversationPlayer.DefaultSpeaker != null
                ? conversationPlayer.DefaultSpeaker.SpeakerName 
                : string.Empty;
        }

        private void OnDialogueLinePlayed(Conversation conversation, DialogueLine dialogueLine)
        {
            if (!text) return;
            text.text = dialogueLine is {Speaker: { }}
                ? dialogueLine.Speaker.SpeakerName
                : string.Empty;
        }
    }
}