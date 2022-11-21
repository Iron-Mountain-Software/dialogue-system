using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace SpellBoundAR.DialogueSystem.UI
{
    [RequireComponent(typeof(Text))]
    public class DialogueLineTyperText : DialogueLineTyper
    {
        [Header("Cache")]
        private Text _text;
        private string _target;
        private int _currentIndex;

        public override bool IsAnimating => _currentIndex <= _target.Length;

        private Text Text
        {
            get
            {
                if (!_text) _text = GetComponent<Text>();
                return _text;
            }
        }

        protected override void SetText(string text) 
        {
            StopAllCoroutines();
            _target = text;
            Text.text = _target;
            _currentIndex = _target.Length + 1;
        }

        protected override IEnumerator AnimateRunner(float letterRate) 
        {
            Text.text = string.Empty;
            _currentIndex = 1;
            while (IsAnimating) {
                Text.text = _target[.._currentIndex];
                if (letterRate > 0) { yield return new WaitForSeconds(letterRate); }
                _currentIndex++;
            }
        }

        public override void ForceFinishAnimating() 
        {
            StopAllCoroutines();
            Text.text = _target;
            _currentIndex = _target.Length + 1;
        }
    }
}
