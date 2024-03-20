using System;
using System.Collections.Generic;
using UnityEngine;

namespace IronMountain.DialogueSystem
{
    public class ConversationsManager : MonoBehaviour
    {
        public static event Action OnConversationsChanged; 
        
        public static readonly List<Conversation> AllConversations = new ();

        public static void Register(Conversation conversation)
        {
            if (!conversation || AllConversations.Contains(conversation)) return;
            AllConversations.Add(conversation);
            OnConversationsChanged?.Invoke();
        }
        
        public static void Unregister(Conversation conversation)
        {
            if (!conversation || !AllConversations.Contains(conversation)) return;
            AllConversations.Remove(conversation);
            OnConversationsChanged?.Invoke();
        }
    }
}