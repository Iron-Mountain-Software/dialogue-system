using System.Collections;
using IronMountain.DialogueSystem.Speakers;
using UnityEngine;

namespace IronMountain.DialogueSystem.UI.TextAnimation
{
    [DisallowMultipleComponent]
    public abstract class DialogueLineTyper : MonoBehaviour
    {
        [SerializeField] private ConversationPlayer conversationPlayer;
        [SerializeField] private bool prependSpeakerName;
        [SerializeField] private bool useSpeakerColor;
        [SerializeField] private string speakerNameSeparator = ": ";

        [SerializeField] private bool matchAudioClipLength;
        [SerializeField] private float defaultLetterRate = 0.04f;

        protected string SpeakerContent;
        protected string DialogueLineContent;
        
        protected bool PrependSpeakerName => prependSpeakerName;
        protected bool UseSpeakerColor => useSpeakerColor;
        protected string SpeakerNameSeparator => speakerNameSeparator;
        
        public bool IsAnimating { get; protected set; }
        
        protected virtual void Awake()
        {
            if (!conversationPlayer) conversationPlayer = GetComponentInParent<ConversationPlayer>();
        }

        protected virtual void OnValidate()
        {
            if (!conversationPlayer) conversationPlayer = GetComponentInParent<ConversationPlayer>();
        }
        
        private void OnEnable()
        {
            Reset();
            if (conversationPlayer) conversationPlayer.OnDialogueLinePlayed += OnDialogueLinePlayed;
            IsAnimating = false;
        }
        
        protected virtual void OnDisable()
        {
            if (conversationPlayer) conversationPlayer.OnDialogueLinePlayed -= OnDialogueLinePlayed;
            IsAnimating = false;
        }

        protected virtual void Reset()
        {
            StopAllCoroutines();
            SpeakerContent = string.Empty;
            DialogueLineContent = string.Empty;
            IsAnimating = false;
        }

        protected abstract string FormatSpeakerWithColor(ISpeaker speaker);
        protected abstract IEnumerator Animate(float seconds);
        public abstract void ForceFinishAnimating();
        
        private void OnDialogueLinePlayed(Conversation conversation, DialogueLine dialogueLine)
        {
            StopAllCoroutines();
            DialogueLineContent = dialogueLine != null ? dialogueLine.Text : string.Empty;
            if (PrependSpeakerName && dialogueLine is {Speaker: { }})
            {
                if (UseSpeakerColor)
                {
                    SpeakerContent = FormatSpeakerWithColor(dialogueLine.Speaker);
                }
                else SpeakerContent = dialogueLine.Speaker.SpeakerName + SpeakerNameSeparator;
            }
            else SpeakerContent = string.Empty;
            
            AudioClip audioClip = dialogueLine?.AudioClip;
            float animationSeconds = matchAudioClipLength && audioClip
                ? audioClip.length
                : DialogueLineContent.Length * defaultLetterRate;

            StartCoroutine(Animate(animationSeconds));
        }
    }
}
