using System;
using System.Collections.Generic;
using SpellBoundAR.DialogueSystem.Nodes.Conditions;
using SpellBoundAR.DialogueSystem.Nodes.ResponseGenerators;
using UnityEngine;
using XNode;

namespace SpellBoundAR.DialogueSystem.Nodes
{
    [NodeWidth(160)]
    [NodeTint("#0061CC")]
    public class DialogueResponseBlockNode : DialogueNode
    {
        public static event Action<DialogueResponseBlockNode, ConversationUI> OnDialogueResponseBlockEntered;
        public static event Action<DialogueResponseBlockNode, ConversationUI> OnDialogueResponseBlockExited;
	
        [Input] public Connection input;
        [Output] public Connection output;

        public override string Name => "Response Generators";

        public override DialogueNode GetNextNode(ConversationUI conversationUI)
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

        public override void OnNodeEnter(ConversationUI conversationUI)
        {
            base.OnNodeEnter(conversationUI);
            OnDialogueResponseBlockEntered?.Invoke(this, conversationUI);
        }

        public override void OnNodeExit(ConversationUI conversationUI)
        {
            base.OnNodeExit(conversationUI);
            OnDialogueResponseBlockExited?.Invoke(this, conversationUI);
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