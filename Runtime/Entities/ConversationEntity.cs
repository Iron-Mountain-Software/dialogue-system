using System;
using System.Collections.Generic;
using SpellBoundAR.AssetManagement;
using SpellBoundAR.Characters;
using UnityEngine;

namespace SpellBoundAR.DialogueSystem.Entities
{
    public class ConversationEntity : IConversationEntity
    {
        public static event Action<ConversationEntity> OnAnyDialogueListChanged;

        public string ID { get; set; }
        public string Name { get; }
        public Sprite Depiction { get; }
        public Conversation DefaultConversation { get; }
        public List<Conversation> Conversations { get; }

        public ConversationEntity(
            IIdentifiable identifiable,
            IDepictable depictable,
            Conversation defaultConversation,
            List<Conversation> conversations)
        {
            ID = identifiable.ID;
            Name = identifiable.Name;
            Depiction = depictable.Depiction;
            DefaultConversation = defaultConversation;
            Conversations = conversations;
            foreach (Conversation conversation in Conversations)
            {
                if (!conversation || !conversation.Condition) continue;
                conversation.Condition.OnConditionStateChanged += OnAnyConversationConditionStateChanged;
            }
        }
        
        public ConversationEntity(
            IIdentifiable identifiable,
            IDepictable depictable,
            ConversationEntityConfiguration configuration)
        {
            ID = identifiable.ID;
            Name = identifiable.Name;
            Depiction = depictable.Depiction;
            DefaultConversation = configuration.DefaultConversation;
            Conversations = configuration.Conversations;
            foreach (Conversation conversation in Conversations)
            {
                if (!conversation || !conversation.Condition) continue;
                conversation.Condition.OnConditionStateChanged += OnAnyConversationConditionStateChanged;
            }
        }

        ~ConversationEntity()
        {
            foreach (Conversation conversation in Conversations)
            {
                if (!conversation || !conversation.Condition) continue;
                conversation.Condition.OnConditionStateChanged -= OnAnyConversationConditionStateChanged;
            }
        }
        
        private void OnAnyConversationConditionStateChanged()
        {
            OnAnyDialogueListChanged?.Invoke(this);
        }

        public List<Conversation> GetActiveDialogue()
        {
            List<Conversation> conversations = new List<Conversation>();
            foreach (Conversation conversation in Conversations)
            {
                if (!conversation) continue;
                if (!conversation.Looping && conversation.Playthroughs > 0) continue;
                if (!conversation.Condition || !conversation.Condition.Evaluate()) continue;
                conversations.Add(conversation);
            }
            return conversations;
        }

        public Conversation GetNextPrioritizedConversation()
        {
            Conversation conversation = null;
            foreach (Conversation testConversation in Conversations)
            {
                if (!testConversation) continue;
                if (!testConversation.PrioritizeOverDefault) continue;
                if (!testConversation.Looping && testConversation.Playthroughs > 0) continue;
                if (!testConversation.Condition || !testConversation.Condition.Evaluate()) continue;
                if (conversation && testConversation.Priority >= conversation.Priority) continue;
                conversation = testConversation;
            }
            return conversation;
        }

        public virtual Conversation GetNextConversation()
        {
            Conversation prioritizedConversation = GetNextPrioritizedConversation();
            return prioritizedConversation ? prioritizedConversation : DefaultConversation;
        }

        public virtual Sprite GetPortrait(PortraitType type)
        {
            return Depiction;
        }
    }
}
