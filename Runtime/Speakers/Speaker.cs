using System;
using System.Collections.Generic;
using SpellBoundAR.AssetManagement;
using UnityEngine;

namespace SpellBoundAR.DialogueSystem.Speakers
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Dialogue/Speaker")]
    public class Speaker : IdentifiableScriptableObject, ISpeaker
    {
        public event Action OnActiveConversationsChanged;

        [SerializeField] public Conversation defaultConversation;
        [SerializeField] private List<Conversation> conversations = new ();
        [SerializeField] private SpeakerPortraitCollection portraits;
        
        public Conversation DefaultConversation => defaultConversation;
        public List<Conversation> Conversations => conversations;
        public SpeakerPortraitCollection Portraits => portraits;

        private void OnEnable()
        {
            foreach (Conversation conversation in conversations)
            {
                if (conversation) conversation.OnIsActiveChanged += InvokeOnActiveConversationsChanged;
            }
        }

        private void OnDisable()
        {
            foreach (Conversation conversation in conversations)
            {
                if (conversation) conversation.OnIsActiveChanged -= InvokeOnActiveConversationsChanged;
            }
        }

        private void InvokeOnActiveConversationsChanged()
        {
            OnActiveConversationsChanged?.Invoke();
        }
    }
}