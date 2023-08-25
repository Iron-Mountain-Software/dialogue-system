using System;
using SpellBoundAR.DialogueSystem.UI;

namespace SpellBoundAR.DialogueSystem.Nodes
{
    [NodeWidth(175)]
    [NodeTint("#D7263D")]
    public class DialogueEndingNode : DialogueNode
    {
        public static Action<DialogueEndingNode> OnDialogueEndingExited;
    
        [Input] public Connection input;
        public override string Name => graph ? "[out] " + graph.name : "[out]";

        public override DialogueNode GetNextNode(ConversationUI conversationUI)
        {
            return null;
        }

        public override void OnNodeEnter(ConversationUI conversationUI)
        {
            base.OnNodeEnter(conversationUI);
            conversationUI.CompleteDialogueInteraction();
        }

        public override void OnNodeExit(ConversationUI conversationUI)
        {
            base.OnNodeExit(conversationUI);
            OnDialogueEndingExited?.Invoke(this);
        }
        
#if UNITY_EDITOR

        protected override bool ExtensionHasWarnings()
        {
            return false;
        }

        protected override bool ExtensionHasErrors()
        {
            return GetInputPort("input").ConnectionCount == 0;
        }
		
#endif
        
    }
}