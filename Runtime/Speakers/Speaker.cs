using System;
using System.Collections.Generic;
using UnityEngine;

namespace IronMountain.DialogueSystem.Speakers
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Dialogue/Speaker")]
    public class Speaker : ScriptableObject, ISpeaker
    {
        public event Action OnActiveConversationsChanged;

        [SerializeField] private string id;
        [SerializeField] private string speakerName;
        [SerializeField] private List<string> aliases;
        [SerializeField] private Color color;
        [SerializeField] private Conversation defaultConversation;
        [SerializeField] private List<Conversation> conversations = new ();
        [SerializeField] private SpeakerPortraitCollection portraits;
        [SerializeField] private SpeakerPortraitCollection fullBodyPortraits;
        
        public string ID => id;
        public string SpeakerName => speakerName;
        public List<string> Aliases => aliases;
        public Color Color => color;
        public Conversation DefaultConversation => defaultConversation;
        public List<Conversation> Conversations => conversations;
        public SpeakerPortraitCollection Portraits => portraits;
        public SpeakerPortraitCollection FullBodyPortraits => fullBodyPortraits;

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
        
#if UNITY_EDITOR

        public virtual void Reset()
        {
            GenerateNewID();
        }
        
        [ContextMenu("Generate New ID")]
        private void GenerateNewID()
        {
            id = UnityEditor.GUID.Generate().ToString();
        }

#endif
    }
}