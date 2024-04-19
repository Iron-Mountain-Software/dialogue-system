using System;
using IronMountain.DialogueSystem.Speakers;
using UnityEngine;
using Object = UnityEngine.Object;

namespace IronMountain.DialogueSystem.Narration
{
    [ExecuteAlways]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(AudioSource))]
    public class DialogueNarrator : MonoBehaviour
    {
        public event Action OnIsPlayingChanged;
    
        public enum Type
        {
            Global,
            Speaker
        }
        
        [SerializeField] private Type type;
        [SerializeField] private Object speaker;
        [SerializeField] private AudioSource audioSource;
        
        private bool _isPlaying;
    
        public ISpeaker Speaker => speaker as ISpeaker;
        public AudioSource AudioSource => audioSource;
        public ConversationPlayer CurrentConversationPlayer { get; private set; }

        public bool IsPlaying
        {
            get => _isPlaying;
            set
            {
                if (_isPlaying == value) return;
                _isPlaying = value;
                OnIsPlayingChanged?.Invoke();
            }
        }

        private void Awake()
        {
            InitializeAudioSource();
        }

        private void OnEnable()
        {
            RefreshRequirements();
            DialogueNarrationManager.DialogueNarrators.Add(this);
            ConversationPlayer.OnAnyDialogueLinePlayed += OnAnyDialogueLinePlayed;
        }

        private void OnDisable()
        {
            DialogueNarrationManager.DialogueNarrators.Remove(this);
            ConversationPlayer.OnAnyDialogueLinePlayed -= OnAnyDialogueLinePlayed;
        }

        public void RefreshRequirements()
        {
            DialogueNarrationRequirement[] requirements = GetComponents<DialogueNarrationRequirement>();
            foreach (DialogueNarrationRequirement requirement in requirements)
            {
                if (!requirement.enabled) continue;
                if (requirement.IsSatisfied()) continue;
                enabled = false;
                return;
            }
            enabled = true;
        }

        private void InitializeAudioSource()
        {
            if (!audioSource) audioSource = GetComponent<AudioSource>();
            if (!audioSource) audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = null;
            audioSource.mute = false;
            audioSource.loop = false;
            audioSource.playOnAwake = false;
        }

        private void OnAnyDialogueLinePlayed(ConversationPlayer conversationPlayer, Conversation conversation, DialogueLine dialogueLine)
        {
            if (dialogueLine == null) return;
            switch (type)
            {
                case Type.Global:
                    Play(conversationPlayer, dialogueLine.AudioClip);
                    break;
                case Type.Speaker:
                    if (Speaker == null || dialogueLine.Speaker == null) return;
                    if (!string.Equals(Speaker.ID, dialogueLine.Speaker.ID)) return;
                    Play(conversationPlayer, dialogueLine.AudioClip);
                    break;
            }
        }

        private void Play(ConversationPlayer conversationPlayer, AudioClip audioClip)
        {
            if (!audioClip) return;
            if (!audioSource) InitializeAudioSource();
            if (!audioSource) return;
            if (audioSource.isPlaying) audioSource.Stop();
            audioSource.clip = audioClip;
            if (CurrentConversationPlayer)
            {
                CurrentConversationPlayer.OnEnabledChanged -= RefreshPausedState;
                CurrentConversationPlayer.OnIsMutedChanged -= RefreshPausedState;
            }
            CurrentConversationPlayer = conversationPlayer;
            if (CurrentConversationPlayer)
            {
                CurrentConversationPlayer.OnEnabledChanged += RefreshPausedState;
                CurrentConversationPlayer.OnIsMutedChanged += RefreshPausedState;
            }
            if (CurrentConversationPlayer && CurrentConversationPlayer.enabled) audioSource.Play();
            RefreshMutedState();
        }

        private void RefreshPausedState()
        {
            if (!audioSource) return;
            if (CurrentConversationPlayer && CurrentConversationPlayer.enabled)
                audioSource.UnPause();
            else audioSource.Pause();
        }
        
        private void RefreshMutedState()
        {
            if (!audioSource) return;
            audioSource.mute = CurrentConversationPlayer && CurrentConversationPlayer.IsMuted;
        }

        private void Update()
        {
            IsPlaying = audioSource && audioSource.isPlaying;
        }

#if UNITY_EDITOR
        
        private void OnValidate()
        {
            InitializeAudioSource();
            ValidateSpeaker();
        }

        private void ValidateSpeaker()
        {
            switch (type)
            {
                case Type.Global:
                    speaker = null;
                    break;
                case Type.Speaker:
                    if (speaker is GameObject speakerObject) speaker = speakerObject.GetComponent<ISpeaker>() as Object;
                    if (speaker is not ISpeaker) speaker = gameObject.GetComponent<ISpeaker>() as Object;
                    if (!speaker) Debug.LogWarning("Warning: SpeakerController is missing a Speaker!", this);
                    break;
            }
        }

#endif
        
    }
}