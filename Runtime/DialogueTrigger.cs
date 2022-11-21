using System;
using UnityEngine;

namespace ARISE.DialogueSystem
{
    public class DialogueTrigger : MonoBehaviour
    {
        public event Action OnEnabled;
        public event Action OnDisabled;

        [SerializeField] private ConversationSelector conversationSelector;

        public IConversationEntity ConversationEntity { get; protected set; }
        
        public ConversationSelector ConversationSelector
        {
            get => conversationSelector;
            protected set => conversationSelector = value;
        }

        public virtual void TriggerConversation()
        {
            if (ConversationSelector == null) return;
            Conversation conversation = ConversationSelector.NextConversation;
            if (conversation == null) return;
            ConversationManager.PlayConversation(conversation);
        }

        protected void FireOnEnable() => OnEnabled?.Invoke();
        protected void FireOnDisable() => OnDisabled?.Invoke();

        protected virtual void OnEnable()
        {
            ConversationSelector = new ConversationSelector(ConversationEntity);
            FireOnEnable();
        }

        protected virtual void OnDisable()
        {
            ConversationSelector = null;
            FireOnDisable();
        }
    }
}
