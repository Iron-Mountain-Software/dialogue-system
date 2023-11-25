using System.Collections;
using IronMountain.DialogueSystem.Speakers;
using TMPro;
using UnityEngine;

namespace IronMountain.DialogueSystem.UI
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class DialogueLineTyperTMPro : DialogueLineTyper
    {
        [SerializeField] private TextMeshProUGUI text;
        
        private TextMeshProUGUI Text
        {
            get
            {
                if (!text) text = GetComponent<TextMeshProUGUI>();
                return text;
            }
        }
        
        protected override void Reset()
        {
            base.Reset();
            Text.text = string.Empty;
            Text.maxVisibleCharacters = Text.text.Length;
        }
        
        protected override string FormatSpeakerWithColor(ISpeaker speaker)
        {
            if (speaker == null) return SpeakerNameSeparator;
            return "<#" + ColorUtility.ToHtmlStringRGBA(speaker.Color) + ">"
                   + speaker.SpeakerName
                   + SpeakerNameSeparator
                   + "</color>";
        }

        protected override IEnumerator Animate(float seconds) 
        {
            IsAnimating = true;
            Text.text = SpeakerContent + DialogueLineContent;
            Text.maxVisibleCharacters = SpeakerContent.Length;
            for (float timer = 0; timer < seconds; timer += Time.deltaTime)
            {
                float progress = timer / seconds;
                int index = Mathf.RoundToInt(progress * DialogueLineContent.Length);
                Text.maxVisibleCharacters = SpeakerContent.Length + index;
                yield return null;
            }
            Text.maxVisibleCharacters = Text.text.Length;
            IsAnimating = false;
        }

        public override void ForceFinishAnimating() 
        {
            StopAllCoroutines();
            Text.maxVisibleCharacters = Text.text.Length;
            IsAnimating = false;
        }
    }
}