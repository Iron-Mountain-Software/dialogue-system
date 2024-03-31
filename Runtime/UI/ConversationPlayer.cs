using System;
using System.Collections;
using IronMountain.DialogueSystem.Nodes;
using IronMountain.DialogueSystem.Nodes.ResponseGenerators;
using IronMountain.DialogueSystem.Speakers;
using IronMountain.DialogueSystem.UI.Responses;
using UnityEngine;

namespace IronMountain.DialogueSystem.UI
{
    public class ConversationPlayer : MonoBehaviour
    {
        public static event Action<Conversation> OnAnyConversationStarted;
        public static event Action<Conversation> OnAnyConversationEnded;
        public static event Action<Conversation, DialogueNode> OnAnyDialogueNodeChanged;
        public static event Action<Conversation, DialogueLine> OnAnyDialogueLinePlayed;
        
        public event Action OnDefaultSpeakerChanged;
        public event Action OnConversationChanged;
        public event Action<Conversation, DialogueNode> OnDialogueNodeChanged;
        public event Action<Conversation, DialogueLine> OnDialogueLinePlayed;

        public event Action OnClosed;
        
        [SerializeField] private Conversation conversation;
        [SerializeField] private bool playOnStart = true;
        [SerializeField] private bool autoAdvance = false;
        [SerializeField] private float autoAdvanceSeconds = 3f;
        [Space]
        [SerializeField] private bool selfDestruct = true;
        [SerializeField] private float destructionDelay = .5f;
        [Space]
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
                    OnAnyDialogueLinePlayed?.Invoke(conversation, _currentDialogueLine);
                }
            }
        }
        
        public DialogueResponseBlockNode CurrentDialogueResponseBlockNode => CurrentNode as DialogueResponseBlockNode;
        public float TotalSecondsToRespond => CurrentDialogueResponseBlockNode ? CurrentDialogueResponseBlockNode.Seconds : Mathf.Infinity;
        public float SecondsRemainingToRespond { get; set; }

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

        private void Update()
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

        private IEnumerator WaitToContinue(float seconds, Action onComplete)
        {
            yield return new WaitForSeconds(seconds);
            onComplete?.Invoke();
        }

        public void PlayNextNode()
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