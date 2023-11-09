using System.Collections;
using TMPro;
using UnityEngine;

namespace IronMountain.DialogueSystem.UI
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class DialogueLineTyperTMPro : DialogueLineTyper
    {
        [Header("Cache")]
        private TextMeshProUGUI _text;

        public override bool IsAnimating => _text.maxVisibleCharacters < _text.text.Length;

        private TextMeshProUGUI Text
        {
            get
            {
                if (!_text) _text = GetComponent<TextMeshProUGUI>();
                return _text;
            }
        }
        
        protected override void Reset()
        {
            StopAllCoroutines();
            Text.text = string.Empty;
            Text.maxVisibleCharacters = Text.text.Length;
        }

        protected override void OnDialogueLinePlayed(Conversation conversation, DialogueLine dialogueLine)
        {
            StopAllCoroutines();
            string text = dialogueLine != null ? dialogueLine.Text : string.Empty;

            if (PrependSpeakerName && dialogueLine is {Speaker: { }})
            {
                if (UseSpeakerColor)
                {
                    text = "<#" + ColorUtility.ToHtmlStringRGBA(dialogueLine.Speaker.Color) + ">"
                           + dialogueLine.Speaker.SpeakerName 
                           + SpeakerNameSeparator
                           + "</color>" 
                           + text;
                }
                else text = dialogueLine.Speaker.SpeakerName 
                            + SpeakerNameSeparator 
                            + text;
            }
            
            Text.text = text;
            Text.maxVisibleCharacters = Text.text.Length;
            AnimateByLetterRate(DefaultLetterRate);
        }

        protected override IEnumerator AnimateRunner(float letterRate) 
        {
            Text.maxVisibleCharacters = 0;
            while (IsAnimating) {
                Text.maxVisibleCharacters++;
                if (letterRate > 0) { yield return new WaitForSeconds(letterRate); }
            }
        }

        public override void ForceFinishAnimating() 
        {
            StopAllCoroutines();
            Text.maxVisibleCharacters = Text.text.Length;
        }
    }
}