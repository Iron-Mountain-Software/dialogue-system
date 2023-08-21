using System;
using SpellBoundAR.DialogueSystem.Speakers;
using UnityEngine;

namespace SpellBoundAR.DialogueSystem.Selection
{
    public abstract class ConversationSelector : MonoBehaviour
    {
        public event Action<Conversation> OnNextConversationChanged;

        [Header("References")]
        [SerializeField] private Speaker speaker;
        [SerializeField] private Conversation nextConversation;

        public Speaker Speaker
        {
            get => speaker;
            set => speaker = value;
        }

        public Conversation NextConversation
        {
            get => nextConversation;
            protected set
            {
                if (nextConversation == value) return;
                nextConversation = value;
                OnNextConversationChanged?.Invoke(value);
            }
        }

        public abstract void RefreshNextConversation();

        protected virtual void OnEnable()
        {
            if (speaker) speaker.OnActiveConversationsChanged += RefreshNextConversation;
            ConversationUI.OnDialogueInteractionEnded += OnDialogueInteractionEnded;
            RefreshNextConversation();
        }

        protected virtual void OnDisable()
        {
            if (speaker) speaker.OnActiveConversationsChanged -= RefreshNextConversation;
            ConversationUI.OnDialogueInteractionEnded -= OnDialogueInteractionEnded;
        }

        private void OnDialogueInteractionEnded(Conversation conversation)
        {
            if (Speaker && Speaker.ID == conversation.Speaker.ID) RefreshNextConversation();
        }
    }
}