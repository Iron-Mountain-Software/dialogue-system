using System;
using System.Collections.Generic;
using SpellBoundAR.DialogueSystem.Speakers;
using SpellBoundAR.DialogueSystem.UI;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SpellBoundAR.DialogueSystem
{
    public static class ConversationManager
    {
        private const string Path = "Prefabs/UI/Conversation UI";

        public static event Action OnConversationQueueChanged;
        
        private static readonly Queue<Tuple<ISpeaker, Conversation>> Queue = new ();
        
        public static int ConversationQueueLength() => Queue.Count;
        
        public static Tuple<ISpeaker, Conversation> PeekConversationQueue() => Queue.Peek();

        public static void EnqueueConversation(ISpeaker speaker, Conversation conversation)
        {
            if (speaker == null || !conversation) return;
            foreach (var (queuedSpeaker, queuedConversation) in Queue)
            {
                if (queuedSpeaker == speaker && queuedConversation == conversation) return;
            }
            Queue.Enqueue(new Tuple<ISpeaker, Conversation>(speaker, conversation));
            OnConversationQueueChanged?.Invoke();
        }

        public static Tuple<ISpeaker, Conversation> DequeueConversation()
        {
            Tuple<ISpeaker, Conversation> entry = Queue.Dequeue();            
            OnConversationQueueChanged?.Invoke();
            return entry;
        }

        public static ConversationPlayer PlayConversation(ISpeaker speaker, Conversation conversation)
        {
            ConversationPlayer conversationUI = Resources.Load<ConversationPlayer>(Path);
            if (!conversationUI) throw new Exception("Resources: Could not find: " + Path);
            return Object.Instantiate(conversationUI).Initialize(speaker, conversation);
        }

        public static void StopConversation(ConversationPlayer conversationUI)
        {
            if (!conversationUI) return;
            conversationUI.Close();
            Resources.UnloadUnusedAssets();
        }
    }
}
