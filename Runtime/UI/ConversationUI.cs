using System;
using SpellBoundAR.DialogueSystem.Nodes;
using SpellBoundAR.Drawers;
using UnityEngine;

namespace SpellBoundAR.DialogueSystem
{
    public class ConversationUI : MonoBehaviour
    {
        public static event Action<Conversation> OnDialogueInteractionStarted;
        public static event Action<Conversation, DialogueLine> OnDialogueLinePlayed;
        public static event Action<Conversation> OnDialogueInteractionEnded;

        public event Action OnConversationChanged;
        
        public event Action OnOpened;
        public event Action OnClosed;

        [Header("Static Settings")]
        private const float DestructionDelay = .5f;
        
        [Header("References")]
        [SerializeField] private Drawer drawer;
        
        [Header("Cache")]
        public static int FrameOfLastProgression;
        private Conversation _currentConversation;
        private DialogueNode _currentNode;

        public Conversation CurrentConversation
        {
            get => _currentConversation;
            private set
            {
                if (_currentConversation == value) return;
                if (_currentConversation) OnDialogueInteractionEnded?.Invoke(_currentConversation);
                _currentConversation = value;
                if (_currentConversation) OnDialogueInteractionStarted?.Invoke(_currentConversation);
                OnConversationChanged?.Invoke();
            }
        }
    
        public DialogueNode CurrentNode
        {
            get => _currentNode;
            set
            {
                if (_currentNode == value) return;
                if (_currentNode) _currentNode.OnNodeExit(this);
                _currentNode = value;
                FrameOfLastProgression = Time.frameCount;
                if (_currentNode) _currentNode.OnNodeEnter(this);
            }
        }

        private void Awake()
        {
            if (drawer) drawer.CloseImmediate();
        }

        private void Start()
        {
            Open();
        }

        public ConversationUI Initialize(Conversation conversation)
        {
            _currentConversation = null;
            _currentNode = null;
            CurrentConversation = conversation;
            CurrentNode = (DialogueBeginningNode) conversation.nodes.Find(node => node is DialogueBeginningNode);
            CurrentConversation.OnConversationStarted();
            return this;
        }

        public void Open()
        {
            if (drawer) drawer.Open();
            OnOpened?.Invoke();
        }

        public void Close()
        {
            if (drawer) drawer.Close();
            Destroy(gameObject, DestructionDelay);
            OnClosed?.Invoke();
        }

        public void PlayDialogueLine(DialogueLine dialogueLine)
        {
            OnDialogueLinePlayed?.Invoke(_currentConversation, dialogueLine);
        }

        public void PlayNextDialogueNode()
        {
            if (!CurrentNode) return;
            DialogueNode nextNode = CurrentNode.GetNextNode(this);
            if (nextNode) CurrentNode = nextNode;
        }

        public void CompleteDialogueInteraction()
        {
            CurrentConversation.Playthroughs++;
            CurrentNode = null;
            CurrentConversation = null;
            PlayNextConversationOrClose();
        }

        protected virtual void PlayNextConversationOrClose()
        {
            if (ConversationManager.ConversationQueueLength() > 0)
            {
                Conversation conversation = ConversationManager.DequeueConversation();
                Initialize(conversation);
            }
            else Close();
        }
    }
}