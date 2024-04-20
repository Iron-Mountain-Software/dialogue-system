using System.Collections.Generic;
using IronMountain.DialogueSystem.UI;
using XNode;
using Random = UnityEngine.Random;

namespace IronMountain.DialogueSystem.Nodes
{
    [NodeWidth(120)]
    [NodeTint("#656565")]
    public class DialogueRandomizerNode : DialogueNode
    {
        [Input] public Connection input;
        [Output] public Connection output;

        public override string Name => "Randomizer";

        public override DialogueNode GetNextNode(ConversationPlayer conversationPlayer)
        {
            NodePort outputPort = GetOutputPort("output");
            List<NodePort> connectionPorts = outputPort?.GetConnections();
            if (connectionPorts == null || connectionPorts.Count == 0) return null;
            int randomIndex = Random.Range(0, connectionPorts.Count);
            return connectionPorts[randomIndex].node as DialogueNode;
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
            if (GetInputPort("input").ConnectionCount == 0) Errors.Add("Bad input.");
            if (GetOutputPort("output").ConnectionCount < 1) Errors.Add("Bad output.");
        }

#endif
    }
}