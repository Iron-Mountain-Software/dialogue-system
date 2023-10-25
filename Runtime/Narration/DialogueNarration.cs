using System;
using SpellBoundAR.DialogueSystem.Speakers;
using SpellBoundAR.DialogueSystem.UI;
using UnityEngine;

namespace SpellBoundAR.DialogueSystem.Narration
{
    [ExecuteAlways]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(AudioSource))]
    public class DialogueNarration : MonoBehaviour
    {
        [SerializeField] private ConversationPlayer conversationPlayer;
        [SerializeField] private AudioSource audioSource;
    
        private void Awake()
        {
            InitializeAudioSource();
            if (!conversationPlayer) conversationPlayer = GetComponentInParent<ConversationPlayer>();
        }

        private void OnValidate()
        {
            InitializeAudioSource();
            if (!conversationPlayer) conversationPlayer = GetComponentInParent<ConversationPlayer>();
        }

        private void OnEnable()
        {
            RefreshRequirements();
            if (conversationPlayer) conversationPlayer.OnDialogueLinePlayed += OnDialogueLinePlayed;
        }

        private void OnDisable()
        {
            if (conversationPlayer) conversationPlayer.OnDialogueLinePlayed -= OnDialogueLinePlayed;
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
            audioSource = GetComponent<AudioSource>();
            if (!audioSource) audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = null;
            audioSource.mute = false;
            audioSource.loop = false;
            audioSource.playOnAwake = false;
        }

        private void OnDialogueLinePlayed(Conversation conversation, DialogueLine dialogueLine)
        {
            if (!audioSource) InitializeAudioSource();
            if (audioSource.isPlaying) audioSource.Stop();
            if (dialogueLine.AudioClip)
            {
                audioSource.clip = dialogueLine.AudioClip;
                audioSource.Play();
            }
        }
    }
}