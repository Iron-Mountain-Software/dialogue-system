using UnityEngine;

namespace ARISE.DialogueSystem.Nodes.Conditions
{
	public abstract class PassFailCondition : Condition
	{
		[Output] public Connection pass;
		[Output] public Connection fail;

		[Header("Cache")]
		protected ConversationUI ConversationUI;

		protected abstract bool TestCondition(ConversationUI conversationUI);

		public override string Name => "Pass fail";

        public override DialogueNode GetNextNode(ConversationUI conversationUI)
        {
	        return TestCondition(conversationUI) ?
		        GetOutputPort("pass")?.Connection?.node as DialogueNode :
		        GetOutputPort("fail")?.Connection?.node as DialogueNode;
        }

        public override void OnNodeEnter(ConversationUI conversationUI)
        {
	        conversationUI.CurrentNode = GetNextNode(conversationUI);
        }

        public override void OnNodeExit(ConversationUI conversationUI) { }

#if UNITY_EDITOR

		protected override bool ExtensionHasErrors()
        {
	        return false;
        }
		
#endif
    }
}
