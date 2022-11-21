using SpellBoundAR.CanvasGroups;
using SpellBoundAR.DialogueSystem.Nodes;
using UnityEngine;

namespace SpellBoundAR.DialogueSystem.UI
{
    [RequireComponent(typeof(CanvasGroupAnimator))]
    public class DialogueTextBoxBackground : MonoBehaviour
    {
        [Header("Cache")]
        private CanvasGroupAnimator _animator;

        private void Awake()
        {
            _animator = GetComponent<CanvasGroupAnimator>();
            DialogueBeginningNode.OnDialogueBeginningEntered += OnDialogueStartEntered;
            DialogueEndingNode.OnDialogueEndingExited += OnDialogueEndExited;
        }

        private void Start()
        {
            if (_animator) _animator.FadeOutImmediate();
        }

        private void OnDestroy()
        {
            DialogueBeginningNode.OnDialogueBeginningEntered -= OnDialogueStartEntered;
            DialogueEndingNode.OnDialogueEndingExited -= OnDialogueEndExited;
        }

        private void OnDialogueStartEntered(DialogueBeginningNode dialogueBeginningNode)
        {
            if (_animator) _animator.FadeIn();
        }

        private void OnDialogueEndExited(DialogueEndingNode dialogueEndingNode)
        {
            if (_animator) _animator.FadeOut();
        }
    }
}