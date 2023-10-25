using System;
using System.Collections;
using System.Collections.Generic;
using IronMountain.StandardAnimations.RectTransforms;
using SpellBoundAR.DialogueSystem.Nodes;
using SpellBoundAR.DialogueSystem.Speakers;
using UnityEngine;

namespace SpellBoundAR.DialogueSystem.UI
{
    public class ConversationUI : MonoBehaviour
    {
        public static event Action<ISpeaker, Conversation> OnDialogueInteractionStarted;
        public static event Action<Conversation, DialogueLine> OnDialogueLinePlayed;
        public static event Action<ISpeaker, Conversation> OnDialogueInteractionEnded;

        public event Action OnSpeakerChanged;
        public event Action OnConversationChanged;
        
        public event Action OnOpened;
        public event Action OnClosed;

        [Header("Static Settings")]
        private const float DestructionDelay = .5f;

        [Header("Settings")]
        [SerializeField] private bool continueAfterNarration = false;

        [Header("References")]
        [SerializeField] private Drawer drawer;
        
        [Header("Cache")]
        private ISpeaker _currentSpeaker;
        private Conversation _currentConversation;
        private DialogueNode _currentNode;

        public int FrameOfLastProgression { get; private set; }
        public float TimeOfLastProgression { get; private set; }

        public ISpeaker CurrentSpeaker
        {
            get => _currentSpeaker;
            private set
            {
                if (_currentSpeaker == value) return;
                _currentSpeaker = value;
                OnSpeakerChanged?.Invoke();
            }
        }
        
        public Conversation CurrentConversation
        {
            get => _currentConversation;
            private set
            {
                if (_currentConversation == value) return;
                if (_currentConversation) OnDialogueInteractionEnded?.Invoke(_currentSpeaker, _currentConversation);
                _currentConversation = value;
                if (_currentConversation) OnDialogueInteractionStarted?.Invoke(_currentSpeaker, _currentConversation);
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
                TimeOfLastProgression = Time.time;
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

        public ConversationUI Initialize(ISpeaker speaker, Conversation conversation)
        {
            _currentSpeaker = null;
            _currentConversation = null;
            _currentNode = null;
            CurrentSpeaker = speaker;
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
            StopAllCoroutines();
            StartCoroutine(PlayDialogueLineRunner(dialogueLine));
        }

        private IEnumerator PlayDialogueLineRunner(DialogueLine dialogueLine)
        {
            OnDialogueLinePlayed?.Invoke(_currentConversation, dialogueLine);
            if (!continueAfterNarration) yield break;
            float narrationLength = dialogueLine != null && dialogueLine.AudioClip ? dialogueLine.AudioClip.length : 1f;
            yield return new WaitForSeconds(narrationLength);
            PlayNextDialogueNode();
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
                Tuple<ISpeaker, Conversation> entry = ConversationManager.DequeueConversation();
                Initialize(entry.Item1, entry.Item2);
            }
            else Close();
        }
    }
}