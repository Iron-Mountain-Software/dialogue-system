using System.Collections;
using IronMountain.DialogueSystem.Speakers;
using UnityEngine;
using UnityEngine.UI;

namespace IronMountain.DialogueSystem.UI.TextAnimation
{
    [RequireComponent(typeof(Text))]
    public class DialogueLineTyperText : DialogueLineTyper
    {
        [SerializeField] private Text text;
        
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
            base.Reset();
            Text.text = string.Empty;
        }

        protected override string FormatSpeakerWithColor(ISpeaker speaker)
        {
            if (speaker == null) return SpeakerNameSeparator;
            return "<color=#" + ColorUtility.ToHtmlStringRGBA(speaker.Color) + ">"
                   + speaker.SpeakerName
                   + SpeakerNameSeparator
                   + "</color>";
        }

        protected override IEnumerator Animate(float seconds) 
        {
            IsAnimating = true;
            for (float timer = 0; timer < seconds; timer += Time.deltaTime)
            {
                float progress = timer / seconds;
                int index = Mathf.RoundToInt(progress * DialogueLineContent.Length);
                Text.text = SpeakerContent 
                            + DialogueLineContent[..index] 
                            + "<color=#00000000>"
                            + DialogueLineContent[index..]
                            + "</color>";
                yield return null;
            }
            Text.text = SpeakerContent + DialogueLineContent;
            IsAnimating = false;
        }

        public override void ForceFinishAnimating() 
        {
            StopAllCoroutines();
            Text.text = SpeakerContent + DialogueLineContent;
            IsAnimating = false;
        }
    }
}
