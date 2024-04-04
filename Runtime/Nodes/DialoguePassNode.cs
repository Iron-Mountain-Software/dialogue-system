using IronMountain.DialogueSystem.UI;

namespace IronMountain.DialogueSystem.Nodes
{
    [NodeWidth(120)]
    [NodeTint("#656565")]
    public class DialoguePassNode : DialogueNode
    {
        [Input] public Connection input;
        [Output] public Connection output;

        public override string Name => "Pass";

        public override DialogueNode GetNextNode(ConversationPlayer conversationUI)
        {
            return GetOutputPort("output")?.Connection?.node as DialogueNode;
        }

        public override void OnNodeEnter(ConversationPlayer conversationUI)
        {
            base.OnNodeEnter(conversationUI);
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