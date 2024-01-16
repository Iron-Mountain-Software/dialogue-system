using System;
using System.Collections;
using IronMountain.DialogueSystem.Nodes;
using IronMountain.DialogueSystem.Speakers;
using UnityEngine;

namespace IronMountain.DialogueSystem.UI
{
    public class ConversationPlayer : MonoBehaviour
    {
        public static event Action<Conversation> OnAnyConversationStarted;
        public static event Action<Conversation> OnAnyConversationEnded;
        public static event Action<Conversation, DialogueLine> OnAnyDialogueLinePlayed;

        public event Action OnDefaultSpeakerChanged;
        public event Action OnConversationChanged;
        public event Action<Conversation, DialogueLine> OnDialogueLinePlayed;

        public event Action OnClosed;
        
        [SerializeField] private Conversation conversation;
        [SerializeField] private bool playOnStart = true;
        [SerializeField] private bool continueAfterNarration = false;
        [SerializeField] private bool selfDestruct = true;
        [SerializeField] private float destructionDelay = .5f;
        [SerializeField] private Transform responseBlockParent;
        [SerializeField] private DialogueResponseBlock responseBlockPrefab;
        
        [Header("Cache")]
        private ISpeaker _defaultSpeaker;
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
        
        public Conversation Conversation
        {
            get => conversation;
            private set
            {
                if (conversation == value) return;
                if (conversation) OnAnyConversationEnded?.Invoke(conversation);
                conversation = value;
                if (conversation) OnAnyConversationStarted?.Invoke(conversation);
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
                    OnDialogueLinePlayed?.Invoke(conversation, _currentDialogueLine);
                    OnAnyDialogueLinePlayed?.Invoke(conversation, _currentDialogueLine);
                }
            }
        }

        private void OnEnable() => ConversationPlayersManager.Register(this);
        private void OnDisable() => ConversationPlayersManager.Unregister(this);
        
        public ConversationPlayer Initialize(ISpeaker speaker, Conversation conversation)
        {
            _defaultSpeaker = null;
            this.conversation = null;
            _currentNode = null;
            _currentDialogueLine = null;
            DefaultSpeaker = speaker;
            Conversation = conversation;
            Play();
            return this;
        }

        private void Start()
        {
            if (playOnStart && !CurrentNode) Play();
        }

        private void Play()
        {
            CurrentNode = Conversation && Conversation.nodes != null 
                ? (DialogueBeginningNode) conversation.nodes.Find(node => node is DialogueBeginningNode) 
                : null;
            if (!Conversation || !CurrentNode) return;
            Conversation.OnConversationStarted();
        }

        public void Close()
        {
            OnClosed?.Invoke();
            if (selfDestruct) Destroy(gameObject, destructionDelay);
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
            if (Conversation) Conversation.Playthroughs++;
            CurrentNode = null;
            Conversation = null;
            CurrentDialogueLine = null;
        }

#if UNITY_EDITOR
        
        private void OnValidate()
        {
            if (!responseBlockParent) responseBlockParent = transform;
            if (destructionDelay < 0) destructionDelay = 0;
        }
        
#endif
        
    }
}