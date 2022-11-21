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
        public static event Action OnCurrentConversationChanged;

        private static readonly Queue<Conversation> Queue = new ();

        [Header("References")]
        private static ConversationUI _conversationUI;
        private static Conversation _currentConversation;

        public static Conversation CurrentConversation
        {
            get => _currentConversation;
            set
            {
                if (_currentConversation == value) return;
                _currentConversation = value;
                OnCurrentConversationChanged?.Invoke();
            }
        }

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

        public static void PlayConversation(Conversation conversation)
        {
            CurrentConversation = conversation;
            _conversationUI = Resources.Load<ConversationUI>(Path);
            if (!_conversationUI) throw new Exception("Resources: Could not find: " + Path);
            _conversationUI = Object.Instantiate(_conversationUI).Initialize(CurrentConversation);
        }

        public static void StopConversation()
        {
            if (_conversationUI) _conversationUI.Close();
            CurrentConversation = null;
            Resources.UnloadUnusedAssets();
        }
    }
}
