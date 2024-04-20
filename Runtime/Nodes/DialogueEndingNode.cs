using IronMountain.DialogueSystem.UI;

namespace IronMountain.DialogueSystem.Nodes
{
    [NodeWidth(175)]
    [NodeTint("#D7263D")]
    public class DialogueEndingNode : DialogueNode
    {
        [Input] public Connection input;
        public override string Name => graph ? "[out] " + graph.name : "[out]";

        public override DialogueNode GetNextNode(ConversationPlayer conversationPlayer)
        {
            return null;
        }

        public override void OnNodeEnter(ConversationPlayer conversationPlayer)
        {
            conversationPlayer.CompleteConversation();
            conversationPlayer.Close();
        }

        public override void OnNodeUpdate(ConversationPlayer conversationPlayer) { }

        public override void OnNodeExit(ConversationPlayer conversationPlayer) { }

#if UNITY_EDITOR
        
        public override void RefreshErrors()
        {
            base.RefreshErrors();
            if (GetInputPort("input").ConnectionCount == 0) Errors.Add("Bad input.");
        }

#endif
        
    }
}