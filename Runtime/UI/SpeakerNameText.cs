using System;
using UnityEngine;
using UnityEngine.UI;

namespace SpellBoundAR.DialogueSystem.UI
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Text))]
    public class SpeakerNameText : MonoBehaviour
    {
        [SerializeField] private Text text;
        [SerializeField] private ConversationPlayer conversationPlayer;

        private void Awake()
        {
            if (!text) text = GetComponent<Text>();
            if (!conversationPlayer) conversationPlayer = GetComponentInParent<ConversationPlayer>();
        }

        private void OnValidate()
        {
            if (!text) text = GetComponent<Text>();
            if (!conversationPlayer) conversationPlayer = GetComponentInParent<ConversationPlayer>();
        }

        private void OnEnable()
        {
            if (conversationPlayer) conversationPlayer.OnDefaultSpeakerChanged += Refresh; 
            Refresh();
        }

        private void OnDisable()
        {
            if (conversationPlayer) conversationPlayer.OnDefaultSpeakerChanged -= Refresh; 
        }

        private void Refresh()
        {
            if (!text) return;
            text.text = conversationPlayer
                         && conversationPlayer.DefaultSpeaker != null
                ? conversationPlayer.DefaultSpeaker.SpeakerName
                : string.Empty;
        }
    }
}