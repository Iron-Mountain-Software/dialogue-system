using System;
using System.Collections;
using IronMountain.DialogueSystem.Nodes;
using IronMountain.DialogueSystem.Speakers;
using UnityEngine;

namespace IronMountain.DialogueSystem.UI
{
    public class ConversationPlayer : MonoBehaviour
    {
        public static event Action<Conversation, DialogueLine> OnAnyDialogueLinePlayed;

        public event Action OnDefaultSpeakerChanged;
        public event Action OnConversationChanged;
        public event Action<Conversation, DialogueLine> OnDialogueLinePlayed;

        public event Action OnClosed;
        
        [Header("Settings")]
        [SerializeField] private bool continueAfterNarration = false;
        [SerializeField] private float destructionDelay = .5f;
        [SerializeField] private Transform responseBlockParent;
        [SerializeField] private DialogueResponseBlock responseBlockPrefab;
        
        [Header("Cache")]
        private ISpeaker _defaultSpeaker;
        private Conversation _currentConversation;
        private DialogueNode _currentNode;
        private DialogueLine _currentDialogueLine;
        private DialogueResponseBlock _currentResponseBlock;

        public int FrameOfLastProgression { get; private set; }
        public float TimeOfLastProgression { get; private set; }

        public ISpeaker DefaultSpeaker
        {
            get => _defaultSpeaker;
            private set
            {
                if (_defaultSpeaker == value) return;
                _defaultSpeaker = value;
                OnDefaultSpeakerChanged?.Invoke();
            }
        }
        
        public Conversation CurrentConversation
        {
            get => _currentConversation;
            private set
            {
                if (_currentConversation == value) return;
                _currentConversation = value;
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
        
        public DialogueLine CurrentDialogueLine
        {
            get => _currentDialogueLine;
            set
            {
                if (_currentDialogueLine == value) return;
                _currentDialogueLine = value;
                if (_currentDialogueLine != null)
                {
                    OnDialogueLinePlayed?.Invoke(_currentConversation, _currentDialogueLine);
                    OnAnyDialogueLinePlayed?.Invoke(_currentConversation, _currentDialogueLine);
                }
            }
        }

        private void OnEnable() => ConversationPlayersManager.Register(this);
        private void OnDisable() => ConversationPlayersManager.Unregister(this);
        
        public ConversationPlayer Initialize(ISpeaker speaker, Conversation conversation)
        {
            _defaultSpeaker = null;
            _currentConversation = null;
            _currentNode = null;
            _currentDialogueLine = null;
            DefaultSpeaker = speaker;
            CurrentConversation = conversation;
            CurrentNode = (DialogueBeginningNode) conversation.nodes.Find(node => node is DialogueBeginningNode);
            CurrentConversation.OnConversationStarted();
            return this;
        }

        public void Close()
        {
            Destroy(gameObject, destructionDelay);
            OnClosed?.Invoke();
        }

        public void PlayDialogueLine(DialogueLine dialogueLine)
        {
            StopAllCoroutines();
            CurrentDialogueLine = dialogueLine;
            if (continueAfterNarration)
            {
                float seconds = dialogueLine != null && dialogueLine.AudioClip 
                    ? dialogueLine.AudioClip.length 
                    : 2f;
                StartCoroutine(WaitToContinue(seconds));
            }
        }

        private IEnumerator WaitToContinue(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            PlayNextDialogueNode();
        }

        public void GenerateResponseBlock(DialogueResponseBlockNode dialogueResponseBlockNode)
        {
            if (_currentResponseBlock || !responseBlockPrefab) return;
            Transform parent = responseBlockParent ? responseBlockParent : transform;
            _currentResponseBlock = Instantiate(responseBlockPrefab, parent);
            _currentResponseBlock.Initialize(dialogueResponseBlockNode, this);
        }

        public void DestroyResponseBlock()
        {
            if (_currentResponseBlock) _currentResponseBlock.Destroy();
            _currentResponseBlock = null;
        }

        public void PlayNextDialogueNode()
        {
            if (!CurrentNode) return;
            DialogueNode nextNode = CurrentNode.GetNextNode(this);
            if (nextNode) CurrentNode = nextNode;
        }

        public void CompleteConversation()
        {
            if (CurrentConversation) CurrentConversation.Playthroughs++;
            CurrentNode = null;
            CurrentConversation = null;
            CurrentDialogueLine = null;
        }

#if UNITY_EDITOR
        
        private void OnValidate()
        {
            if (!responseBlockParent) responseBlockParent = transform;
        }
        
#endif
        
    }
}