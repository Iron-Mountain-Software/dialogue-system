using System;
using ARISE.DialogueSystem.Nodes;
using UnityEngine;

namespace ARISE.DialogueSystem.Responses
{
    [Serializable]
    public class NeverMindResponse : BasicResponse
    {
        public NeverMindResponse(DialogueNode node, string text, int row, int column) : base(node, text, null, row, column)
        {
            Height = .08f;
            ButtonColorPrimary = new Color(0.94f, 0.38f, 0.31f);
            ButtonColorSecondary = new Color(0.68f, 0.2f, 0.17f);
            TextColor = Color.white;
        }
    }
}