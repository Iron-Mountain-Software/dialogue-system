using System;
using System.Collections.Generic;
using IronMountain.DialogueSystem.Speakers;
using IronMountain.DialogueSystem.UI;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace IronMountain.DialogueSystem.Selection
{
    public abstract class ConversationSelector : MonoBehaviour
    {
        public event Action<Conversation> OnNextConversationChanged;

        [SerializeField] private Conversation nextConversation;
        [SerializeField] private Object speaker;
        [SerializeField] private List<Conversation> additionalConversations = new ();

        protected readonly List<Conversation> AllConversations = new();
        
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
            RefreshAllConversations();
            RefreshNextConversation();
        }

        protected virtual void OnDisable()
        {
            if (Speaker != null) Speaker.OnActiveConversationsChanged -= RefreshNextConversation;
            ConversationPlayersManager.OnConversationPlayersChanged -= RefreshNextConversation;
        }

        private void RefreshAllConversations()
        {
            AllConversations.Clear();
            if (Speaker is {Conversations: { }})
            {
                foreach (Conversation conversation in Speaker.Conversations)
                {
                    if (!conversation || AllConversations.Contains(conversation)) continue;
                    AllConversations.Add(conversation);
                }
            }
            if (additionalConversations != null)
            {
                foreach (Conversation conversation in additionalConversations)
                {
                    if (!conversation || AllConversations.Contains(conversation)) continue;
                    AllConversations.Add(conversation);
                }
            }
        }

#if UNITY_EDITOR
        
        private void OnValidate()
        {
            ValidateSpeaker();
        }

        public void ValidateSpeaker()
        {
            if (speaker is GameObject speakerObject)
            {
                speaker = speakerObject.GetComponent<ISpeaker>() as Object;
                EditorUtility.SetDirty(this);
            }
            if (speaker is not ISpeaker)
            {
                speaker = gameObject.GetComponent<ISpeaker>() as Object;
                EditorUtility.SetDirty(this);
            }
            if (!speaker) Debug.LogWarning("Warning: ConversationSelector is missing a Speaker!", this);
            AssetDatabase.SaveAssetIfDirty(this);
        }

#endif
        
    }
}