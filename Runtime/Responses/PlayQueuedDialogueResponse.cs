using SpellBoundAR.DialogueSystem.Nodes;
using UnityEngine;

namespace SpellBoundAR.DialogueSystem.Responses
{
    public class PlayQueuedDialogueResponse : BasicResponse
    {
        private readonly Conversation _conversation;
    
        public PlayQueuedDialogueResponse(DialogueNode node, string text, Sprite icon, int row, int column, IResponseStyle style, Conversation conversation) : base(node, text, icon, row, column, style)
        {
            _conversation = conversation;
        }

        public override void ExecuteResponse()
        {
            if (_conversation) ConversationManager.EnqueueConversation(_conversation);
        }
    }
}