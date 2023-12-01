using System.Collections.Generic;
using IronMountain.DialogueSystem.UI;
using IronMountain.ScriptableActions;

namespace IronMountain.DialogueSystem.Nodes.Actions
{
    [NodeWidth(400)]
    public class ScriptableActionNode : DialogueAction
    {
        public List<ScriptableAction> actions = new ();

        protected override void HandleAction(ConversationPlayer conversationUI)
        {
            foreach (ScriptableAction action in actions)
            {
                if (action) action.Invoke();
            }
        }

        public override string Name => "Scriptable Actions";
    }
}