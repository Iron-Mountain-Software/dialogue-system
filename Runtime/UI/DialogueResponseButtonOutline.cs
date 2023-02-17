using UnityEngine;
using UnityEngine.UI;

namespace SpellBoundAR.DialogueSystem.UI
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Outline))]
    public class DialogueResponseButtonOutline : MonoBehaviour
    {
        [Header("Cache")]
        private Outline _outline;
        private DialogueResponseButton _dialogueResponseButton;

        private void Awake()
        {
            _outline = GetComponent<Outline>();
            _dialogueResponseButton = GetComponentInParent<DialogueResponseButton>();
        }

        private void OnEnable()
        {
            if (_dialogueResponseButton) _dialogueResponseButton.OnBasicResponseChanged += Refresh;
            Refresh();
        }

        private void OnDisable()
        {
            if (_dialogueResponseButton) _dialogueResponseButton.OnBasicResponseChanged -= Refresh;
        }

        private void Refresh()
        {
            if (!_outline) return;
            _outline.effectColor = _dialogueResponseButton && _dialogueResponseButton.BasicResponse is {Style: { }}
                ? _dialogueResponseButton.BasicResponse.Style.ButtonColorSecondary
                : Color.clear;
        }
    }
}