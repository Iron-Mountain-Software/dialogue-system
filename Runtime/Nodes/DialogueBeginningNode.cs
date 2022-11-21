using System;

namespace ARISE.DialogueSystem.Nodes
{
    [NodeWidth(175)]
    [NodeTint("#00A676")]
    public class DialogueBeginningNode : DialogueNode
    {
        public static Action<DialogueBeginningNode> OnDialogueBeginningEntered;

        [Output] public Connection output;

        public override string Name => graph ? "[in] " + graph.name : "[in]";

        public override DialogueNode GetNextNode(ConversationUI conversationUI)
        {
            return GetOutputPort("output")?.Connection?.node as DialogueNode;
        }

        public override void OnNodeEnter(ConversationUI conversationUI)
        {
            base.OnNodeEnter(conversationUI);
            OnDialogueBeginningEntered?.Invoke(this);
            conversationUI.CurrentNode = GetNextNode(conversationUI);
        }

#if UNITY_EDITOR

        protected override bool ExtensionHasWarnings()
        {
            return false;
        }

        protected override bool ExtensionHasErrors()
        {
            return GetOutputPort("output").ConnectionCount != 1;
        }
		
#endif
        
    }
}