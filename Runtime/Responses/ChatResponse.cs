using System;
using SpellBoundAR.DialogueSystem.Nodes;

namespace SpellBoundAR.DialogueSystem.Responses
{
    [Serializable]
    public class ChatResponse : BasicResponse
    {
        public ChatResponse(DialogueNode node, string text, int row, int column) : base(node, text, null, row, column)
        {
            Height = .08f;
        }
    }
}