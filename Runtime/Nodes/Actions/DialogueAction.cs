using SpellBoundAR.DialogueSystem.UI;
using UnityEngine;

namespace SpellBoundAR.DialogueSystem.Nodes.Actions
{
    [NodeWidth(150)]
    [NodeTint("#FFCA3A")]
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

        public void LogErrors()
        {
            if (GetInputPort("input").ConnectionCount == 0) Debug.LogError("Dialogue Action Node Error: Invalid Input: " + Name, this);
            if (GetOutputPort("output").ConnectionCount != 1) Debug.LogError("Dialogue Action Node Error: Invalid Output: " + Name, this);
        }
        
#if UNITY_EDITOR
		
        protected override bool ExtensionHasWarnings()
        {
            return false;
        }
        
        protected override bool ExtensionHasErrors()
        {
            return GetInputPort("input").ConnectionCount == 0
                   || GetOutputPort("output").ConnectionCount != 1;
        }
		
#endif
    }
}