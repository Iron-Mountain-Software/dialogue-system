using IronMountain.DialogueSystem.UI;
using UnityEngine;

namespace IronMountain.DialogueSystem.Nodes.Actions
{
    [NodeWidth(150)]
    [NodeTint("#FF9229")]
    public abstract class DialogueAction : DialogueNode
    {
        [Input] public Connection input;
        [Output] public Connection output;

        protected abstract void HandleAction(ConversationPlayer conversationUI);

        public override DialogueNode GetNextNode(ConversationPlayer conversationUI)
        {
            return GetOutputPort("output")?.Connection?.node as DialogueNode;
        }
        
        public override void OnNodeEnter(ConversationPlayer conversationUI)
        {
            base.OnNodeEnter(conversationUI);
            HandleAction(conversationUI);
            conversationUI.CurrentNode = GetNextNode(conversationUI);
        }

#if UNITY_EDITOR

        public override void RefreshErrors()
        {
            base.RefreshErrors();
            if (GetInputPort("input").ConnectionCount == 0) Errors.Add("Bad input.");
            if (GetOutputPort("output").ConnectionCount != 1) Errors.Add("Bad output.");
        }
		
#endif
    }
}