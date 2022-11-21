using ARISE.DialogueSystem.Nodes;
using UnityEngine;

namespace ARISE.DialogueSystem.Responses
{
    public class PlayQueuedDialogueResponse : BasicResponse
    {
        private readonly Conversation _conversation;
    
        public PlayQueuedDialogueResponse(DialogueNode node, string text, Sprite icon, int row, int column, Conversation conversation) : base(node, text, icon, row, column)
        {
            _conversation = conversation;
        }

        public override void ExecuteResponse()
        {
            if (_conversation) ConversationManager.EnqueueConversation(_conversation);
        }
    }
}