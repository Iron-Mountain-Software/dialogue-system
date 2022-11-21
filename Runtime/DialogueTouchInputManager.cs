using SpellBoundAR.DialogueSystem.UI;
using UnityEngine;

namespace SpellBoundAR.DialogueSystem
{
    public class DialogueTouchInputManager : MonoBehaviour
    {
        [SerializeField] private DialogueLineTyper dialogueLineTyper;

        [Header("Cache")]
        private ConversationUI _conversationUI;
        
        private void Awake()
        {
            _conversationUI = GetComponentInParent<ConversationUI>();
        }

        private void Update()
        {
            if (_conversationUI
                && _conversationUI.CurrentConversation
                && ConversationUI.FrameOfLastProgression != Time.frameCount
                && Input.GetMouseButtonUp(0))
            {
                if (dialogueLineTyper && dialogueLineTyper.IsAnimating)
                {
                    dialogueLineTyper.ForceFinishAnimating();
                }
                else _conversationUI.PlayNextDialogueNode();
            }
        }
    }
}