using System.Collections;
using TMPro;
using UnityEngine;

namespace ARISE.DialogueSystem.UI
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

        protected override void SetText(string text) 
        {
            StopAllCoroutines();
            Text.text = text;
            Text.maxVisibleCharacters = Text.text.Length;
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