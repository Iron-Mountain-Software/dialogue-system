using System;
using IronMountain.DialogueSystem.Nodes;
using IronMountain.DialogueSystem.UI;
using UnityEngine;

namespace IronMountain.DialogueSystem.Responses
{
    [Serializable]
    public class BranchConversationResponse : BasicResponse
    {
        private readonly ConversationPlayer _conversationPlayer;
        private readonly DialogueNode _node;

        public override void Execute()
        {
            if (_conversationPlayer) _conversationPlayer.CurrentNode = _node;
        }

        public BranchConversationResponse(ConversationPlayer conversationPlayer, DialogueNode node, string text, Sprite sprite, int row, int column, IResponseStyle style) 
            : base(text, sprite, row, column, style)
        {
            _conversationPlayer = conversationPlayer;
            _node = node;
        }
    }
}