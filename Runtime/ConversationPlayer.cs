using System;
using System.Collections;
using IronMountain.DialogueSystem.Nodes;
using IronMountain.DialogueSystem.Speakers;
using IronMountain.DialogueSystem.UI.Responses;
using UnityEngine;

namespace IronMountain.DialogueSystem
{
    public class ConversationPlayer : MonoBehaviour
    {
        public static event Action<Conversation> OnAnyConversationStarted;
        public static event Action<Conversation> OnAnyConversationEnded;
        public static event Action<Conversation, DialogueNode> OnAnyDialogueNodeChanged;
        public static event Action<ConversationPlayer, Conversation, DialogueLine> OnAnyDialogueLinePlayed;
        
        public event Action OnEnabledChanged;
        public event Action OnDefaultSpeakerChanged;
        public event Action OnConversationChanged;
        public event Action<Conversation, DialogueNode> OnDialogueNodeChanged;
        public event Action<Conversation, DialogueLine> OnDialogueLinePlayed;
        public event Action OnIsMutedChanged;
        public event Action OnClosed;
        
        [SerializeField] private Conversation conversation;
        [SerializeField] private bool playOnStart = true;
        [SerializeField] private bool isMuted;
        [SerializeField] private bool autoAdvance = false;
        [SerializeField] private float autoAdvanceSeconds = 3f;
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
                OnDialogueNodeChanged?.Invoke(conversation, _currentNode);
                OnAnyDialogueNodeChanged?.Invoke(conversation, _currentNode);
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
                    OnAnyDialogueLinePlayed?.Invoke(this, conversation, _currentDialogueLine);
                }
            }
        }
        
        public bool IsMuted
        {
            get => isMuted;
            set
            {
                if (isMuted == value) return;
                isMuted = value;
                OnIsMutedChanged?.Invoke();
            }
        }
        
        public DialogueResponseBlockNode CurrentDialogueResponseBlockNode => CurrentNode as DialogueResponseBlockNode;
        public float TotalSecondsToRespond => CurrentDialogueResponseBlockNode ? CurrentDialogueResponseBlockNode.Seconds : Mathf.Infinity;
        public float SecondsRemainingToRespond { get; set; }

        protected virtual void Awake() => ConversationPlayersManager.Register(this);
        protected virtual void OnDestroy() => ConversationPlayersManager.Unregister(this);

        protected virtual void OnEnable()
        {
            OnEnabledChanged?.Invoke();
        }

        protected virtual void OnDisable()
        {
            OnEnabledChanged?.Invoke();
        }

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

        protected virtual void Update()
        {
            if (CurrentDialogueResponseBlockNode && CurrentDialogueResponseBlockNode.IsTimed)
            {
                SecondsRemainingToRespond -= Time.deltaTime;
                if (SecondsRemainingToRespond > 0) return;
                DialogueNode nextNode = CurrentDialogueResponseBlockNode.GetDefaultResponseNode();
                if (nextNode) CurrentNode = nextNode;
            }
        }

        public void Close()
        {
            OnClosed?.Invoke();
            if (selfDestruct) Destroy(gameObject, destructionDelay);
        }

        public void HandleDialogueLine(DialogueLine dialogueLine)
        {
            StopAllCoroutines();
            CurrentDialogueLine = dialogueLine;
            if (autoAdvance)
            {
                float seconds = dialogueLine != null && dialogueLine.AudioClip 
                    ? dialogueLine.AudioClip.length 
                    : autoAdvanceSeconds;
                StartCoroutine(WaitToContinue(seconds, PlayNextNode));
            }
        }
        
        private IEnumerator WaitToContinue(float seconds, Action onComplete)
        {
            yield return new WaitForSeconds(seconds);
            onComplete?.Invoke();
        }
        
        public void EnterDialogueResponseBlockNode(DialogueResponseBlockNode dialogueResponseBlockNode)
        {
            StopAllCoroutines();
            CloseCurrentResponseBlock();
            SecondsRemainingToRespond = TotalSecondsToRespond;
            SpawnCurrentResponseBlock();
        }
        
        public void ExitDialogueResponseBlockNode(DialogueResponseBlockNode dialogueResponseBlockNode)
        {
            StopAllCoroutines();
            CloseCurrentResponseBlock();
            SecondsRemainingToRespond = Mathf.Infinity;
        }

        private void CloseCurrentResponseBlock()
        {
            if (!_currentResponseBlock) return;
            _currentResponseBlock.Destroy();
            _currentResponseBlock = null;
        }

        private void SpawnCurrentResponseBlock()
        {
            if (!CurrentDialogueResponseBlockNode || !responseBlockPrefab) return;
            Transform parent = responseBlockParent ? responseBlockParent : transform;
            _currentResponseBlock = Instantiate(responseBlockPrefab, parent);
            _currentResponseBlock.Initialize(CurrentDialogueResponseBlockNode, this);
        }

        public void PlayNextNode()
        {
            if (!enabled || !CurrentNode) return;
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
            OnIsMutedChanged?.Invoke();
        }
        
#endif
        
    }
}