using System.Collections;
using SpellBoundAR.DialogueSystem.Speakers;
using UnityEngine;

namespace SpellBoundAR.DialogueSystem.UI
{
    public abstract class DialogueLineTyper : MonoBehaviour
    {
        [SerializeField] private bool prependSpeakerName;
        [SerializeField] private bool useSpeakerColor;
        [SerializeField] private string speakerNameSeparator = ": ";
        [SerializeField] private float defaultLetterRate = 0.04f;

        protected bool PrependSpeakerName => prependSpeakerName;
        protected bool UseSpeakerColor => useSpeakerColor;
        protected string SpeakerNameSeparator => speakerNameSeparator;
        protected float DefaultLetterRate => defaultLetterRate;
        
        public abstract bool IsAnimating { get; }
        
        protected virtual void Awake()
        {
            Reset();
            ConversationUI.OnDialogueLinePlayed += OnDialogueLinePlayed;
        }

        protected virtual void OnDestroy()
        {
            ConversationUI.OnDialogueLinePlayed -= OnDialogueLinePlayed;
        }

        protected abstract void Reset();
        
        protected abstract void OnDialogueLinePlayed(Conversation conversation, DialogueLine dialogueLine);

        protected abstract IEnumerator AnimateRunner(float letterRate);

        public abstract void ForceFinishAnimating();
        
        public void Animate() 
        {
            StopAllCoroutines();
            StartCoroutine(AnimateRunner(defaultLetterRate));
        }

        public void AnimateByLetterRate(float letterRate) 
        {
            StopAllCoroutines();
            StartCoroutine(AnimateRunner(letterRate));
        }

        public void AnimateByTotalTime(float totalTime, int charactersLength) 
        {
            float letterRate = 0;
            if (totalTime > 0 && charactersLength > 0) {
                letterRate = totalTime / charactersLength;
            }
            StopAllCoroutines();
            StartCoroutine(AnimateRunner(letterRate));
        }
    }
}
