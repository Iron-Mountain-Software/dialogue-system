using System;
using SpellBoundAR.DialogueSystem.UI;
using UnityEngine;

namespace SpellBoundAR.DialogueSystem
{
    public class DialogueTouchInputManager : MonoBehaviour
    {
        [SerializeField] private ConversationPlayer conversationUI;
        [SerializeField] private DialogueLineTyper dialogueLineTyper;

        private void Awake()
        {
            if (!conversationUI) conversationUI = GetComponentInParent<ConversationPlayer>();
        }

        private void OnValidate()
        {
            if (!conversationUI) conversationUI = GetComponentInParent<ConversationPlayer>();
        }

        private void Update()
        {
            if (conversationUI
                && conversationUI.CurrentConversation
                && conversationUI.FrameOfLastProgression != Time.frameCount
                && Input.GetMouseButtonUp(0))
            {
                if (dialogueLineTyper && dialogueLineTyper.IsAnimating)
                {
                    dialogueLineTyper.ForceFinishAnimating();
                }
                else conversationUI.PlayNextDialogueNode();
            }
        }
    }
}