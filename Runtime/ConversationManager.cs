using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SpellBoundAR.DialogueSystem
{
    public static class ConversationManager
    {
        private const string Path = "Prefabs/UI/Conversation UI";

        public static event Action OnConversationQueueChanged;
        
        private static readonly Queue<Conversation> Queue = new ();
        
        public static int ConversationQueueLength() => Queue.Count;
        
        public static Conversation PeekConversationQueue() => Queue.Peek();

        public static void EnqueueConversation(Conversation conversation)
        {
            if (!conversation || Queue.Contains(conversation)) return;
            Queue.Enqueue(conversation);
            OnConversationQueueChanged?.Invoke();
        }

        public static Conversation DequeueConversation()
        {
            Conversation conversation = Queue.Dequeue();            
            OnConversationQueueChanged?.Invoke();
            return conversation;
        }

        public static ConversationUI PlayConversation(Conversation conversation)
        {
            ConversationUI conversationUI = Resources.Load<ConversationUI>(Path);
            if (!conversationUI) throw new Exception("Resources: Could not find: " + Path);
            return Object.Instantiate(conversationUI).Initialize(conversation);
        }

        public static void StopConversation(ConversationUI conversationUI)
        {
            if (!conversationUI) return;
            conversationUI.Close();
            Resources.UnloadUnusedAssets();
        }
    }
}
