using System;
using SpellBoundAR.DialogueSystem.Nodes;
using UnityEngine;

namespace SpellBoundAR.DialogueSystem.Responses
{
    [Serializable]
    public class BasicResponse
    {
        public DialogueNode Node { get; }
        public string Text { get; }
        public Sprite Icon { get; protected set; }
    
        public int Row { get; }
        public int Column { get; }

        public IResponseStyle Style { get; protected set; }

        public virtual void ExecuteResponse()
        {
        }

        public BasicResponse(DialogueNode node, string text, Sprite sprite, int row, int column, IResponseStyle style)
        {
            Node = node;
            Text = text;
            Icon = sprite;
            Row = row;
            Column = column;
            Style = style;
        }
    }
}