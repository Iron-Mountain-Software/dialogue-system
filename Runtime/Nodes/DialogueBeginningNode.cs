﻿using System;
using IronMountain.DialogueSystem.UI;

namespace IronMountain.DialogueSystem.Nodes
{
    [NodeWidth(175)]
    [NodeTint("#00A676")]
    public class DialogueBeginningNode : DialogueNode
    {
        public static Action<DialogueBeginningNode> OnDialogueBeginningEntered;

        [Output] public Connection output;

        public override string Name => graph ? "[in] " + graph.name : "[in]";

        public override DialogueNode GetNextNode(ConversationPlayer conversationUI)
        {
            return GetOutputPort("output")?.Connection?.node as DialogueNode;
        }

        public override void OnNodeEnter(ConversationPlayer conversationUI)
        {
            base.OnNodeEnter(conversationUI);
            OnDialogueBeginningEntered?.Invoke(this);
            conversationUI.CurrentNode = GetNextNode(conversationUI);
        }

#if UNITY_EDITOR

        public override void RefreshErrors()
        {
            base.RefreshErrors();
            if (GetOutputPort("output").ConnectionCount != 1) Errors.Add("Bad output.");
        }

#endif
        
    }
}