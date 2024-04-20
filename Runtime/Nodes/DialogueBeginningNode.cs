using System;

namespace IronMountain.DialogueSystem.Nodes
{
    [NodeWidth(175)]
    [NodeTint("#00A676")]
    public class DialogueBeginningNode : DialogueNode
    {
        public static Action<DialogueBeginningNode> OnDialogueBeginningEntered;

        [Output] public Connection output;

        public override string Name => graph ? "[in] " + graph.name : "[in]";

        public override DialogueNode GetNextNode(ConversationPlayer conversationPlayer)
        {
            return GetOutputPort("output")?.Connection?.node as DialogueNode;
        }

        public override void OnNodeEnter(ConversationPlayer conversationPlayer)
        {
            OnDialogueBeginningEntered?.Invoke(this);
            conversationPlayer.CurrentNode = GetNextNode(conversationPlayer);
        }

        public override void OnNodeUpdate(ConversationPlayer conversationPlayer) { }

        public override void OnNodeExit(ConversationPlayer conversationPlayer) { }

#if UNITY_EDITOR

        public override void RefreshErrors()
        {
            base.RefreshErrors();
            if (GetOutputPort("output").ConnectionCount != 1) Errors.Add("Bad output.");
        }

#endif
        
    }
}