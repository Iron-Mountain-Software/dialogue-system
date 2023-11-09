using System;
using System.Collections.Generic;
using IronMountain.DialogueSystem.Nodes.Conditions;
using IronMountain.DialogueSystem.Nodes.ResponseGenerators;
using IronMountain.DialogueSystem.UI;
using UnityEngine;
using XNode;

namespace IronMountain.DialogueSystem.Nodes
{
    [NodeWidth(160)]
    [NodeTint("#0061CC")]
    public class DialogueResponseBlockNode : DialogueNode
    {
        public static event Action<DialogueResponseBlockNode, ConversationPlayer> OnDialogueResponseBlockEntered;
        public static event Action<DialogueResponseBlockNode, ConversationPlayer> OnDialogueResponseBlockExited;
	
        [Input] public Connection input;
        [Output] public Connection output;

        public override string Name => "Response Generators";

        public override DialogueNode GetNextNode(ConversationPlayer conversationUI)
        {
            return null;
        }
        
        public List<ResponseGenerator> GetResponseGenerators()
        {
            NodePort outputPort = GetOutputPort("output");
            List<NodePort> connectionPorts = outputPort?.GetConnections();
            List<ResponseGenerator> responseGenerators = new List<ResponseGenerator>();
            foreach (NodePort connectionPort in connectionPorts)
            {
                if (connectionPort.node is ResponseGenerator responseGenerator)
                {
                    responseGenerators.Add(responseGenerator);
                }
            }
            return responseGenerators;
        }

        public override void OnNodeEnter(ConversationPlayer conversationUI)
        {
            base.OnNodeEnter(conversationUI);
            conversationUI.GenerateResponseBlock(this);
        }

        public override void OnNodeExit(ConversationPlayer conversationUI)
        {
            base.OnNodeExit(conversationUI);
            conversationUI.DestroyResponseBlock();
        }
        
        public override void OnCreateConnection(NodePort @from, NodePort to)
        {
            NodePort myInputPort = GetInputPort("input");
            NodePort myOutputPort = GetOutputPort("output");
            if (from == myOutputPort && !(to.node is ResponseGenerator || to.node is Condition))
            {
                from.Disconnect(to);
                Debug.LogError("Node_DialogueResponseBlock can only output to ResponseGenerator.");
            }
        }
        
#if UNITY_EDITOR

        protected override bool ExtensionHasWarnings()
        {
            return false;
        }

        protected override bool ExtensionHasErrors()
        {
            return GetInputPort("input").ConnectionCount == 0
                   || GetOutputPort("output").ConnectionCount < 1;
        }
		
#endif
    }
}