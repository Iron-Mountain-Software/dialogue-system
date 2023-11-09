using System;
using IronMountain.DialogueSystem.Speakers;
using IronMountain.DialogueSystem.UI;
using UnityEngine;
using Object = UnityEngine.Object;

namespace IronMountain.DialogueSystem.Selection
{
    public abstract class ConversationSelector : MonoBehaviour
    {
        public event Action<Conversation> OnNextConversationChanged;

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
            ConversationPlayersManager.OnConversationPlayersChanged += RefreshNextConversation;
            RefreshNextConversation();
        }

        protected virtual void OnDisable()
        {
            if (Speaker != null) Speaker.OnActiveConversationsChanged -= RefreshNextConversation;
            ConversationPlayersManager.OnConversationPlayersChanged -= RefreshNextConversation;
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