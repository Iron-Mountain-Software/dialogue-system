using System;
using SpellBoundAR.DialogueSystem.Selection;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SpellBoundAR.DialogueSystem.Speakers
{
    [DisallowMultipleComponent]
    public class SpeakerController : MonoBehaviour
    {
        public event Action OnSpeakerChanged;
        
        public event Action OnEnabled;
        public event Action OnDisabled;

        [SerializeField] private Object speaker;
        [SerializeField] private ConversationSelector conversationSelector;

        public ISpeaker Speaker
        {
            get => speaker as ISpeaker;
            set
            {
                if (speaker == value as Object) return;
                speaker = (Object) value;
                if (conversationSelector) conversationSelector.Speaker = value;
                OnSpeakerChanged?.Invoke();
            }
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
            if (conversation) ConversationManager.PlayConversation(Speaker, conversation);
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
            if (speaker is not ISpeaker) speaker = gameObject.GetComponent<ISpeaker>() as Object;
            if (!speaker) Debug.LogWarning("Warning: SpeakerController is missing a Speaker!", this);
            if (conversationSelector) conversationSelector.Speaker = Speaker;
        }

        private void ValidateConversationSelector()
        {
            if (!conversationSelector) conversationSelector = GetComponent<ConversationSelector>();
            if (!conversationSelector) Debug.LogWarning("Warning: SpeakerController is missing a ConversationSelector!", this);
        }

#endif
        

    }
}