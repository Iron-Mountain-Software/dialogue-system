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
        [Header("References")]
        private AudioSource _audioSource;
    
        private void Awake() => InitializeAudioSource();

        private void OnEnable()
        {
            RefreshRequirements();
            ConversationUI.OnDialogueLinePlayed += OnDialogueLinePlayed;
        }

        private void OnDisable()
        {
            ConversationUI.OnDialogueLinePlayed -= OnDialogueLinePlayed;
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
            _audioSource = GetComponent<AudioSource>();
            if (!_audioSource) _audioSource = gameObject.AddComponent<AudioSource>();
            _audioSource.clip = null;
            _audioSource.mute = false;
            _audioSource.loop = false;
            _audioSource.playOnAwake = false;
        }

        private void OnDialogueLinePlayed(Conversation conversation, DialogueLine dialogueLine)
        {
            if (!_audioSource) InitializeAudioSource();
            if (_audioSource.isPlaying) _audioSource.Stop();
            if (dialogueLine.AudioClip)
            {
                _audioSource.clip = dialogueLine.AudioClip;
                _audioSource.Play();
            }
        }
    }
}