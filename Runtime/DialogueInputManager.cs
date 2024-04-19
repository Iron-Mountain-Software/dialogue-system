using IronMountain.DialogueSystem.UI.TextAnimation;
using UnityEngine;

namespace IronMountain.DialogueSystem
{
    public class DialogueInputManager : MonoBehaviour
    {
        [SerializeField] private KeyCode keyCode = KeyCode.Mouse0;
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
                && conversationUI.enabled
                && conversationUI.Conversation
                && conversationUI.FrameOfLastProgression != Time.frameCount
                && Input.GetKeyUp(keyCode))
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