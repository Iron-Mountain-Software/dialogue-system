using System;
using SpellBoundAR.DialogueSystem.Speakers;
using SpellBoundAR.DialogueSystem.UI;
using UnityEngine;

namespace SpellBoundAR.DialogueSystem.Starters
{
    public class ConversationStarterFromResource : ConversationStarter
    {
        [SerializeField] private string path = "Prefabs/UI/Conversation UI";
        
        public override ConversationPlayer StartConversation(ISpeaker speaker, Conversation conversation)
        {
            if (!enabled || speaker == null || !conversation) return null;
            ConversationPlayer conversationPlayer = Resources.Load<ConversationPlayer>(path);
            if (!conversationPlayer) throw new Exception("Resources: Could not find: " + path);
            return Instantiate(conversationPlayer).Initialize(speaker, conversation);
        }
    }
}
