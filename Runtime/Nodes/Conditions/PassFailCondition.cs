namespace IronMountain.DialogueSystem.Nodes.Conditions
{
	public abstract class PassFailCondition : Condition
	{
		[Output] public Connection pass;
		[Output] public Connection fail;

		protected abstract bool TestCondition(ConversationPlayer conversationPlayer);

		public override string Name => "Pass fail";

        public override DialogueNode GetNextNode(ConversationPlayer conversationPlayer)
        {
	        return TestCondition(conversationPlayer) ?
		        GetOutputPort("pass")?.Connection?.node as DialogueNode :
		        GetOutputPort("fail")?.Connection?.node as DialogueNode;
        }

        public override void OnNodeEnter(ConversationPlayer conversationPlayer)
        {
	        conversationPlayer.CurrentNode = GetNextNode(conversationPlayer);
        }

        public override void OnNodeUpdate(ConversationPlayer conversationPlayer) { }
        
        public override void OnNodeExit(ConversationPlayer conversationPlayer) { }

#if UNITY_EDITOR

		public override void RefreshErrors()
		{
			base.RefreshErrors();
			if (GetOutputPort("pass").ConnectionCount != 0) Errors.Add("Bad pass output.");
			if (GetOutputPort("fail").ConnectionCount != 1) Errors.Add("Bad fail output.");
		}
		
#endif
    }
}