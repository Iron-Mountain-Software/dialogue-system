using IronMountain.DialogueSystem.UI;
using IronMountain.DialogueSystem.UI.TextAnimation;
using UnityEngine;

namespace IronMountain.DialogueSystem
{
    public class DialogueTouchInputManager : MonoBehaviour
    {
        [SerializeField] private ConversationPlayer conversationUI;
        [SerializeField] private DialogueLineTyper dialogueLineTyper;

        private void Awake() => OnValidate();

        private void OnValidate()
        {
            if (!conversationUI) conversationUI = GetComponentInParent<ConversationPlayer>();
        }

        private void Update()
        {
            if (conversationUI
                && conversationUI.Conversation
                && conversationUI.FrameOfLastProgression != Time.frameCount
                && Input.GetMouseButtonUp(0))
            {
                if (dialogueLineTyper && dialogueLineTyper.IsAnimating)
                {
                    dialogueLineTyper.ForceFinishAnimating();
                }
                else conversationUI.PlayNextNode();
            }
        }
    }
}