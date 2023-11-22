using System;
using IronMountain.DialogueSystem.Selection;
using IronMountain.DialogueSystem.Starters;
using UnityEngine;
using Object = UnityEngine.Object;

namespace IronMountain.DialogueSystem.Speakers
{
    [DisallowMultipleComponent]
    public class SpeakerController : MonoBehaviour
    {
        public event Action OnSpeakerChanged;
        
        public event Action OnEnabled;
        public event Action OnDisabled;

        [SerializeField] private Object speaker;
        [SerializeField] private ConversationSelector conversationSelector;
        [SerializeField] private ConversationStarter conversationStarter;

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
        
        public virtual ConversationStarter ConversationStarter
        {
            get
            {
                if (!conversationStarter) conversationStarter = GetComponent<ConversationStarterFromResource>();
                return conversationStarter;
            }
        }

        public Conversation NextConversation => ConversationSelector 
            ? ConversationSelector.NextConversation
            : null;

        protected virtual void OnEnable()
        {
            SpeakerControllersManager.Register(this);
            OnEnabled?.Invoke();
        }

        protected virtual void OnDisable()
        {
            SpeakerControllersManager.Unregister(this);
            OnDisabled?.Invoke();
        }

        public virtual void StartConversation(Conversation conversation)
        {
            if (!enabled || !ConversationStarter || Speaker == null || !conversation) return;
            ConversationStarter.StartConversation(Speaker, conversation);
        }
        
        public virtual void StartConversation()
        {
            if (!enabled || !ConversationStarter || Speaker == null || !NextConversation) return;
            ConversationStarter.StartConversation(Speaker, NextConversation);
        }

#if UNITY_EDITOR
        
        private void OnValidate()
        {
            ValidateSpeaker();
            ValidateConversationSelector();
            ValidateConversationStarter();
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
        
        private void ValidateConversationStarter()
        {
            if (!conversationStarter) conversationStarter = GetComponent<ConversationStarter>();
            if (!conversationStarter) Debug.LogWarning("Warning: SpeakerController is missing a ConversationStarter!", this);
        }

#endif

    }
}