using System;
using ARISE.DialogueSystem.Nodes;
using UnityEngine;

namespace ARISE.DialogueSystem.Responses
{
    [Serializable]
    public class BasicResponse
    {
        public DialogueNode Node { get; }
        public string Text { get; }
        public Sprite Icon { get; protected set; }
    
        public int Row { get; }
        public int Column { get; }

        public float Height { get; protected set; }
        public Color ButtonColorPrimary { get; protected set; }
        public Color ButtonColorSecondary { get; protected set; }
        public Color TextColor { get; protected set; }

        public virtual void ExecuteResponse()
        {
        }

        public BasicResponse(DialogueNode node, string text, Sprite sprite, int row, int column)
        {
            Node = node;
            Text = text;
            Icon = sprite;
            Row = row;
            Column = column;
            Height = .11f;
            ButtonColorPrimary = new Color(0.94f, 0.82f, 0.55f);
            ButtonColorSecondary = new Color(0.58f, 0.5f, 0.35f);
            TextColor = new Color(0.24f, 0.1f, 0.04f);
        }
    }
}