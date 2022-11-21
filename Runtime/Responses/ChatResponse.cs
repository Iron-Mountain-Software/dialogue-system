using System;
using ARISE.DialogueSystem.Nodes;

namespace ARISE.DialogueSystem.Responses
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