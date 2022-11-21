using System;
using UnityEngine;

namespace SpellBoundAR.DialogueSystem
{
    [Serializable]
    public class ConversationSelector
    {
        public event Action<Conversation> OnNextConversationChanged;

        [SerializeField] private Conversation nextConversation;

        public IConversationEntity ConversationEntity { get; private set; }
        
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

        public ConversationSelector(IConversationEntity conversationEntity)
        {
            ConversationEntity = conversationEntity;
            DialogueSystem.ConversationEntity.OnAnyDialogueListChanged += OnAnyDialogueListChanged;
            ConversationUI.OnDialogueInteractionEnded += OnDialogueInteractionEnded;
            RefreshNextDialogueInteraction();
        }

        ~ConversationSelector()
        {
            DialogueSystem.ConversationEntity.OnAnyDialogueListChanged -= OnAnyDialogueListChanged;
            ConversationUI.OnDialogueInteractionEnded -= OnDialogueInteractionEnded;
        }
        
        private void OnAnyDialogueListChanged(ConversationEntity conversationEntity)
        {
            if (ConversationEntity != null && ConversationEntity == conversationEntity) RefreshNextDialogueInteraction();
        }

        private void OnDialogueInteractionEnded(Conversation conversation)
        {
            if (ConversationEntity != null && ConversationEntity.ID == conversation.Entity.ID) RefreshNextDialogueInteraction();
        }
    
        public void RefreshNextDialogueInteraction()
        {
            NextConversation = ConversationEntity.GetNextConversation();
        }
    }
}
