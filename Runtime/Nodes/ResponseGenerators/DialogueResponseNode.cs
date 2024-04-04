using System.Collections.Generic;
using IronMountain.DialogueSystem.Nodes.Conditions;
using IronMountain.DialogueSystem.Responses;
using IronMountain.DialogueSystem.UI;
using UnityEngine;
using XNode;

namespace IronMountain.DialogueSystem.Nodes.ResponseGenerators
{
    [NodeWidth(300)]
    [NodeTint("#996515")]
    public abstract class DialogueResponseNode : DialogueNode
    {
        [Input] public Connection input;
        [Output] public Connection output;
    
        [SerializeField] protected int row;
        [SerializeField] protected int column;
        [SerializeField] private ScriptedResponseStyle style;

        public ScriptedResponseStyle ScriptedResponseStyle => style;
        
        public abstract List<BasicResponse> GetDialogueResponses(ConversationPlayer conversationPlayer);

        public override DialogueNode GetNextNode(ConversationPlayer conversationUI)
        {
            return GetOutputPort("output")?.Connection?.node as DialogueNode;
        }
        
        public override void OnNodeEnter(ConversationPlayer conversationUI)
        {
            conversationUI.CurrentNode = GetNextNode(conversationUI);
        }
    
        public override void OnNodeExit(ConversationPlayer conversationUI) { }

        public override void OnCreateConnection(NodePort @from, NodePort to)
        {
            base.OnCreateConnection(from, to);
            NodePort myInputPort = GetInputPort("input");
            if (to == myInputPort && @from.node is not (DialogueResponseBlockNode or Condition))
            {
                from.Disconnect(to);
                Debug.LogError("ResponseGenerator can only have inputs from Node_DialogueResponseBlock.");
            }
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