using IronMountain.DialogueSystem.UI;
using UnityEngine;

namespace IronMountain.DialogueSystem.Nodes.Conditions
{
	public abstract class PassFailCondition : Condition
	{
		[Output] public Connection pass;
		[Output] public Connection fail;

		[Header("Cache")]
		protected ConversationPlayer ConversationUI;

		protected abstract bool TestCondition(ConversationPlayer conversationUI);

		public override string Name => "Pass fail";

        public override DialogueNode GetNextNode(ConversationPlayer conversationUI)
        {
	        return TestCondition(conversationUI) ?
		        GetOutputPort("pass")?.Connection?.node as DialogueNode :
		        GetOutputPort("fail")?.Connection?.node as DialogueNode;
        }

        public override void OnNodeEnter(ConversationPlayer conversationUI)
        {
	        conversationUI.CurrentNode = GetNextNode(conversationUI);
        }

        public override void OnNodeExit(ConversationPlayer conversationUI) { }

#if UNITY_EDITOR

		public override void RefreshErrors()
		{
			base.RefreshErrors();
			if (GetInputPort("pass").ConnectionCount != 0) Errors.Add("Bad input.");
			if (GetOutputPort("fail").ConnectionCount != 1) Errors.Add("Bad output.");
		}
		
#endif
    }
}
