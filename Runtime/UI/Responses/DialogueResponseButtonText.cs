using System;
using UnityEngine;
using UnityEngine.UI;

namespace IronMountain.DialogueSystem.UI.Responses
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Text))]
    public class DialogueResponseButtonText : MonoBehaviour
    {
        [SerializeField] private DialogueResponseButton dialogueResponseButton;
        [SerializeField] private Text text;

        private void Awake() => OnValidate();

        private void OnValidate()
        {
            if (!text) text = GetComponent<Text>();
            if (!dialogueResponseButton) dialogueResponseButton = GetComponentInParent<DialogueResponseButton>();
        }

        private void OnEnable()
        {
            if (dialogueResponseButton) dialogueResponseButton.OnResponseChanged += Refresh;
            Refresh();
        }

        private void OnDisable()
        {
            if (dialogueResponseButton) dialogueResponseButton.OnResponseChanged -= Refresh;
        }

        private void Refresh()
        {
            if (!text) return;
            text.text = dialogueResponseButton && dialogueResponseButton.Response != null
                ? dialogueResponseButton.Response.Text
                : string.Empty;
        }
    }
}