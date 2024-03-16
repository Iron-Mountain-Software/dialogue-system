using UnityEngine;
using UnityEngine.UI;

namespace IronMountain.DialogueSystem.UI.Responses
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
            if (_dialogueResponseButton) _dialogueResponseButton.OnResponseChanged += Refresh;
            Refresh();
        }

        private void OnDisable()
        {
            if (_dialogueResponseButton) _dialogueResponseButton.OnResponseChanged -= Refresh;
        }

        private void Refresh()
        {
            if (!_outline) return;
            _outline.effectColor = _dialogueResponseButton && _dialogueResponseButton.Response is {Style: { }}
                ? _dialogueResponseButton.Response.Style.ButtonColorSecondary
                : Color.clear;
        }
    }
}