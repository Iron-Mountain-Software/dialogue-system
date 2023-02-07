using System;
using System.Collections.Generic;
using SpellBoundAR.Characters;
using UnityEngine;

namespace SpellBoundAR.DialogueSystem.Entities
{
    public class ScriptedConversationEntityBehaviour : MonoBehaviour, IConversationEntity
    {
        [Header("References")]
        [SerializeField] private ScriptableObject conversationEntity;

        private IConversationEntity ConversationEntity => conversationEntity as IConversationEntity;

        public string ID
        {
            get => ConversationEntity?.ID;
            set
            {
                if (ConversationEntity != null) ConversationEntity.ID = value;
            }
        }

        public string Name => ConversationEntity?.Name;
        public Sprite Depiction => ConversationEntity?.Depiction;

        public Conversation DefaultConversation => ConversationEntity?.DefaultConversation;
        public List<Conversation> Conversations => ConversationEntity?.Conversations;
        public List<Conversation> GetActiveDialogue() => ConversationEntity?.GetActiveDialogue();
        public Conversation GetNextPrioritizedConversation() => ConversationEntity?.GetNextPrioritizedConversation();
        public Conversation GetNextConversation() => ConversationEntity?.GetNextConversation();
        public Sprite GetPortrait(PortraitType type) => ConversationEntity?.GetPortrait(type);

#if UNITY_EDITOR

        private void OnValidate()
        {
            if (conversationEntity is not IConversationEntity) conversationEntity = null;
        }

#endif
    }
}
