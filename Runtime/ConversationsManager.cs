using System;
using System.Collections.Generic;
using UnityEngine;

namespace SpellBoundAR.DialogueSystem
{
    public class ConversationsManager : MonoBehaviour
    {
        public static event Action OnActiveInteractionsChanged;
    
        public static readonly List<Conversation> AllConversations = new ();
        public static readonly List<Conversation> ActiveConversations = new ();
        
        public static void RegisterActiveConversation(Conversation conversation)
        {
            if (!conversation || ActiveConversations.Contains(conversation)) return;
            ActiveConversations.Add(conversation);
            OnActiveInteractionsChanged?.Invoke();
        }
        
        public static void UnregisterActiveConversation(Conversation conversation)
        {
            if (!conversation || !ActiveConversations.Contains(conversation)) return;
            ActiveConversations.Remove(conversation);
            OnActiveInteractionsChanged?.Invoke();
        }
    }
}
