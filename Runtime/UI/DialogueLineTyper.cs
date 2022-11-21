using System.Collections;
using UnityEngine;

namespace SpellBoundAR.DialogueSystem.UI
{
    public abstract class DialogueLineTyper : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float defaultLetterRate = 0.04f;

        protected float DefaultLetterRate => defaultLetterRate;
        
        public abstract bool IsAnimating { get; }
        
        protected virtual void Awake()
        {
            SetText(string.Empty);
            ConversationUI.OnDialogueLinePlayed += TypeDialogueLine;
        }

        protected virtual void OnDestroy()
        {
            ConversationUI.OnDialogueLinePlayed -= TypeDialogueLine;
        }
        
        private void TypeDialogueLine(Conversation conversation, DialogueLine dialogueLine)
        {
            if (dialogueLine == null) return;
            SetText(dialogueLine.Text);
            AnimateByLetterRate(defaultLetterRate);
        }

        protected abstract void SetText(string text);
                
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

        protected abstract IEnumerator AnimateRunner(float letterRate);

        public abstract void ForceFinishAnimating();
    }
}
