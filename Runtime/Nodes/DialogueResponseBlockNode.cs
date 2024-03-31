using System;
using System.Collections.Generic;
using IronMountain.DialogueSystem.Nodes.Conditions;
using IronMountain.DialogueSystem.Nodes.ResponseGenerators;
using IronMountain.DialogueSystem.Responses;
using IronMountain.DialogueSystem.UI;
using UnityEngine;
using XNode;

namespace IronMountain.DialogueSystem.Nodes
{
    [NodeWidth(235)]
    [NodeTint("#0061CC")]
    public class DialogueResponseBlockNode : DialogueNode
    {
        public static event Action<DialogueResponseBlockNode, ConversationPlayer> OnDialogueResponseBlockEntered;
        public static event Action<DialogueResponseBlockNode, ConversationPlayer> OnDialogueResponseBlockExited;
        
        [Input] public Connection input;
        [Output] public Connection responses;
        [Output] public Connection defaultResponse;

        [SerializeField] private bool isTimed;
        [SerializeField] private float seconds = 15;

        public override string Name => "Responses";
        
        public bool IsTimed => isTimed;
        public float Seconds => isTimed ? seconds : Mathf.Infinity;

        public override DialogueNode GetNextNode(ConversationPlayer conversationUI)
        {
            return null;
        }
        
        public DialogueNode GetDefaultResponseNode()
        {
            return GetOutputPort("defaultResponse")?.Connection?.node as DialogueNode;
        }
        
        public List<BasicResponse> GetResponses(ConversationPlayer conversationUI)
        {
            List<BasicResponse> responsePaths = new List<BasicResponse>();
            NodePort outputPort = GetOutputPort("responses");
            List<NodePort> connectionPorts = outputPort?.GetConnections();
            if (connectionPorts == null || connectionPorts.Count == 0) return responsePaths;
            foreach (NodePort connectionPort in connectionPorts)
            {
                if (connectionPort is {node: DialogueResponseNode responseGenerator})
                {
                    responsePaths.AddRange(responseGenerator.GetDialogueResponses(conversationUI));
                }
            }
            return responsePaths;
        }
        
        public override void OnNodeEnter(ConversationPlayer conversationUI)
        {
            base.OnNodeEnter(conversationUI);
            conversationUI.EnterDialogueResponseBlockNode(this);
        }

        public override void OnNodeExit(ConversationPlayer conversationUI)
        {
            base.OnNodeExit(conversationUI);
            conversationUI.ExitDialogueResponseBlockNode(this);
        }
        
        public override void OnCreateConnection(NodePort @from, NodePort to)
        {
            NodePort myInputPort = GetInputPort("input");
            NodePort myOutputPort = GetOutputPort("responses");
            if (from == myOutputPort && !(to.node is DialogueResponseNode || to.node is Condition))
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
                   || GetOutputPort("responses").ConnectionCount < 1;
        }
		
#endif
    }
}