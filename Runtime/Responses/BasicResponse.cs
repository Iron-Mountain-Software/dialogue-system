using System;
using SpellBoundAR.DialogueSystem.Nodes;
using SpellBoundAR.DialogueSystem.UI;
using UnityEngine;

namespace SpellBoundAR.DialogueSystem.Responses
{
    [Serializable]
    public class BasicResponse
    {
        public ConversationPlayer ConversationPlayer { get; }
        public DialogueNode SourceNode { get; }
        public string Text { get; }
        public Sprite Icon { get; protected set; }
    
        public int Row { get; }
        public int Column { get; }

        public IResponseStyle Style { get; protected set; }

        public virtual void ExecuteResponse()
        {
            if (ConversationPlayer) ConversationPlayer.CurrentNode = SourceNode;
        }

        public BasicResponse(ConversationPlayer conversationPlayer, DialogueNode sourceNode, string text, Sprite sprite, int row, int column, IResponseStyle style)
        {
            ConversationPlayer = conversationPlayer;
            SourceNode = sourceNode;
            Text = text;
            Icon = sprite;
            Row = row;
            Column = column;
            Style = style;
        }
    }
}