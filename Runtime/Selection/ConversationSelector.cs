using System;
using SpellBoundAR.DialogueSystem.Speakers;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SpellBoundAR.DialogueSystem.Selection
{
    public abstract class ConversationSelector : MonoBehaviour
    {
        public event Action<Conversation> OnNextConversationChanged;

        [Header("References")]
        [SerializeField] private Object speaker;
        [SerializeField] private Conversation nextConversation;

        public ISpeaker Speaker
        {
            get => speaker as ISpeaker;
            set => speaker = value as Object;
        }

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

        public abstract void RefreshNextConversation();

        protected virtual void OnEnable()
        {
            if (Speaker != null) Speaker.OnActiveConversationsChanged += RefreshNextConversation;
            ConversationUI.OnDialogueInteractionEnded += OnDialogueInteractionEnded;
            RefreshNextConversation();
        }

        protected virtual void OnDisable()
        {
            if (Speaker != null) Speaker.OnActiveConversationsChanged -= RefreshNextConversation;
            ConversationUI.OnDialogueInteractionEnded -= OnDialogueInteractionEnded;
        }

        private void OnDialogueInteractionEnded(Conversation conversation)
        {
            if (Speaker != null && Speaker.ID == conversation.Speaker.ID) RefreshNextConversation();
        }
        
#if UNITY_EDITOR
        
        private void OnValidate()
        {
            ValidateSpeaker();
        }

        private void ValidateSpeaker()
        {
            if (speaker is GameObject speakerObject) speaker = speakerObject.GetComponent<ISpeaker>() as Object;
            if (speaker is not ISpeaker) speaker = gameObject.GetComponent<ISpeaker>() as Object;
            if (!speaker) Debug.LogWarning("Warning: ConversationSelector is missing a Speaker!", this);
        }

#endif
        
    }
}