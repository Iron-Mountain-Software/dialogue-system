using IronMountain.DialogueSystem.UI;

namespace IronMountain.DialogueSystem.Nodes
{
    [NodeWidth(175)]
    [NodeTint("#D7263D")]
    public class DialogueEndingNode : DialogueNode
    {
        [Input] public Connection input;
        public override string Name => graph ? "[out] " + graph.name : "[out]";

        public override DialogueNode GetNextNode(ConversationPlayer conversationUI)
        {
            return null;
        }

        public override void OnNodeEnter(ConversationPlayer conversationUI)
        {
            base.OnNodeEnter(conversationUI);
            conversationUI.CompleteConversation();
            conversationUI.Close();
        }

#if UNITY_EDITOR

        protected override bool ExtensionHasWarnings()
        {
            return false;
        }

        protected override bool ExtensionHasErrors()
        {
            return GetInputPort("input").ConnectionCount == 0;
        }
		
#endif
        
    }
}