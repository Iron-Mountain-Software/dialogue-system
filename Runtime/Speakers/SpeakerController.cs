using System;
using SpellBoundAR.DialogueSystem.Selection;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SpellBoundAR.DialogueSystem.Speakers
{
    public class SpeakerController : MonoBehaviour
    {
        public event Action OnEnabled;
        public event Action OnDisabled;

        [SerializeField] private Object speaker;
        [SerializeField] private ConversationSelector conversationSelector;

        public ISpeaker Speaker
        {
            get => speaker as ISpeaker;
            set => speaker = value as Object;
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

#if UNITY_EDITOR
        
        private void OnValidate()
        {
            ValidateSpeaker();
            ValidateConversationSelector();
        }

        private void ValidateSpeaker()
        {
            if (speaker is GameObject speakerObject) speaker = speakerObject.GetComponent<ISpeaker>() as Object;
            if (speaker is not ISpeaker) speaker = null;
            if (!speaker) Debug.LogWarning("Warning: Missing a Speaker!", this);
        }

        private void ValidateConversationSelector()
        {
            if (!conversationSelector) conversationSelector = GetComponent<ConversationSelector>();
            if (!conversationSelector) Debug.LogWarning("Warning: Missing a ConversationSelector!", this);
        }

#endif
        

    }
}