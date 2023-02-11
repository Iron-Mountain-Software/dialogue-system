using System;
using SpellBoundAR.DialogueSystem.Entities;
using UnityEngine;

namespace SpellBoundAR.DialogueSystem
{
    [RequireComponent(typeof(IConversationEntity))]
    public class ConversationSelector : MonoBehaviour
    {
        public event Action<Conversation> OnNextConversationChanged;

        [Header("References")]
        [SerializeField] private Conversation nextConversation;

        [Header("Cache")]
        private IConversationEntity _conversationEntity;

        public IConversationEntity ConversationEntity => _conversationEntity ??= GetComponent<IConversationEntity>();

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

        private void OnEnable()
        {
            Entities.ConversationEntity.OnAnyDialogueListChanged += OnAnyDialogueListChanged;
            ConversationUI.OnDialogueInteractionEnded += OnDialogueInteractionEnded;
            RefreshNextDialogueInteraction();
        }

        private void OnDisable()
        {
            Entities.ConversationEntity.OnAnyDialogueListChanged -= OnAnyDialogueListChanged;
            ConversationUI.OnDialogueInteractionEnded -= OnDialogueInteractionEnded;
        }
        
        private void OnAnyDialogueListChanged(ConversationEntity conversationEntity)
        {
            if (ConversationEntity != null && ConversationEntity.ID == conversationEntity.ID) RefreshNextDialogueInteraction();
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
