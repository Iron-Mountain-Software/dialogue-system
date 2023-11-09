using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace IronMountain.DialogueSystem.UI
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Text))]
    public class DialogueLineTyperText : DialogueLineTyper
    {
        [SerializeField] private Text text;

        [Header("Cache")]
        private string _speaker;
        private string _dialogueLine;
        private int _currentIndex;

        public override bool IsAnimating => _currentIndex <= _dialogueLine.Length;

        private Text Text
        {
            get
            {
                if (!text) text = GetComponent<Text>();
                if (!text) text = gameObject.AddComponent<Text>();
                return text;
            }
        }

        protected override void Reset()
        {
            StopAllCoroutines();
            Text.text = string.Empty;
            _dialogueLine = string.Empty;
            _currentIndex = 0;
        }

        protected override void OnDialogueLinePlayed(Conversation conversation, DialogueLine dialogueLine)
        {
            StopAllCoroutines();
            _dialogueLine = dialogueLine != null ? dialogueLine.Text : string.Empty;
            if (PrependSpeakerName && dialogueLine is {Speaker: { }})
            {
                if (UseSpeakerColor)
                {
                    _speaker = "<color=#" + ColorUtility.ToHtmlStringRGBA(dialogueLine.Speaker.Color) + ">"
                               + dialogueLine.Speaker.SpeakerName
                               + SpeakerNameSeparator
                               + "</color>";
                }
                else _speaker = dialogueLine.Speaker.SpeakerName
                                + SpeakerNameSeparator;
            }
            else _speaker = string.Empty;
            Text.text = _dialogueLine;
            _currentIndex = _dialogueLine.Length + 1;
            AnimateByLetterRate(DefaultLetterRate);
        }

        protected override IEnumerator AnimateRunner(float letterRate) 
        {
            Text.text = string.Empty;
            _currentIndex = 1;
            while (IsAnimating) {
                Text.text = _speaker + _dialogueLine[.._currentIndex];
                if (letterRate > 0) { yield return new WaitForSeconds(letterRate); }
                _currentIndex++;
            }
        }

        public override void ForceFinishAnimating() 
        {
            StopAllCoroutines();
            Text.text = _dialogueLine;
            _currentIndex = _dialogueLine.Length + 1;
        }
    }
}
