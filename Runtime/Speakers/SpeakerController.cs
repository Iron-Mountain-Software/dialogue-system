using System;
using SpellBoundAR.DialogueSystem.Selection;
using UnityEngine;

namespace SpellBoundAR.DialogueSystem.Speakers
{
    [RequireComponent(typeof(ConversationSelector))]
    public class SpeakerController : MonoBehaviour
    {
        public event Action OnEnabled;
        public event Action OnDisabled;

        [SerializeField] private Speaker speaker;
        [SerializeField] private ConversationSelector conversationSelector;

        public Speaker Speaker
        {
            get => speaker;
            set => speaker = value;
        }

        public virtual ConversationSelector ConversationSelector
        {
            get
            {
                if (!conversationSelector) conversationSelector = GetComponent<ConversationSelector>();
                return conversationSelector;
            }
        }
        
        protected virtual void OnEnable() => OnEnabled?.Invoke();
        protected virtual void OnDisable() => OnDisabled?.Invoke();

        public virtual void PlayConversation()
        {
            Conversation conversation = ConversationSelector 
                ? ConversationSelector.NextConversation
                : null;
            if (conversation) ConversationManager.PlayConversation(conversation);
        }
    }
}