using SpellBoundAR.DialogueSystem.UI;
using UnityEngine;

namespace SpellBoundAR.DialogueSystem.Nodes.Actions
{
    [NodeWidth(300)]
    [NodeTint("#FFCA3A")]
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