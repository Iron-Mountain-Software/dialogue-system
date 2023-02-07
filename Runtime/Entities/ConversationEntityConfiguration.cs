using System;
using System.Collections.Generic;
using UnityEngine;

namespace SpellBoundAR.DialogueSystem.Entities
{
    [Serializable]
    public class ConversationEntityConfiguration
    {
        [SerializeField] public Conversation defaultConversation;
        [SerializeField] private List<Conversation> conversations = new ();

        public Conversation DefaultConversation => defaultConversation;
        public List<Conversation> Conversations => conversations;
    }
}
