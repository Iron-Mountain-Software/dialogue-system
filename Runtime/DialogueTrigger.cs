using System;
using UnityEngine;

namespace SpellBoundAR.DialogueSystem
{
    [RequireComponent(typeof(ConversationSelector))]
    public class DialogueTrigger : MonoBehaviour
    {
        public event Action OnEnabled;
        public event Action OnDisabled;

        [Header("Cache")]
        private ConversationSelector _conversationSelector;

        public ConversationSelector ConversationSelector
        {
            get
            {
                if (!_conversationSelector) _conversationSelector = GetComponent<ConversationSelector>();
                return _conversationSelector;
            }
        }

        [ContextMenu("Trigger Conversation")]
        public virtual void TriggerConversation()
        {
            if (ConversationSelector == null) return;
            Conversation conversation = ConversationSelector.NextConversation;
            if (conversation) ConversationManager.PlayConversation(conversation);
        }

        protected virtual void OnEnable() => OnEnabled?.Invoke();

        protected virtual void OnDisable() => OnDisabled?.Invoke();
    }
}