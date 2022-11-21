namespace SpellBoundAR.DialogueSystem.Nodes
{
    [NodeWidth(100)]
    [NodeTint("#656565")]
    public class DialoguePassNode : DialogueNode
    {
        [Input] public Connection input;
        [Output] public Connection output;

        public override string Name => "PASS";

        public override DialogueNode GetNextNode(ConversationUI conversationUI)
        {
            return GetOutputPort("output")?.Connection?.node as DialogueNode;
        }

        public override void OnNodeEnter(ConversationUI conversationUI)
        {
            base.OnNodeEnter(conversationUI);
            conversationUI.CurrentNode = GetNextNode(conversationUI);
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