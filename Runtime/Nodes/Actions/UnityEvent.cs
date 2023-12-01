using IronMountain.DialogueSystem.UI;
using UnityEngine;

namespace IronMountain.DialogueSystem.Nodes.Actions
{
    [NodeWidth(300)]
    public class UnityEvent : DialogueAction
    {
        [SerializeField] private UnityEngine.Events.UnityEvent action;

        protected override void HandleAction(ConversationPlayer conversationUI)
        {
            action?.Invoke();
        }

        public override string Name => "Unity Event";
    }
}