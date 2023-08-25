using SpellBoundAR.DialogueSystem.Nodes;
using SpellBoundAR.DialogueSystem.Speakers;
using UnityEngine;

namespace SpellBoundAR.DialogueSystem.Responses
{
    public class PlayQueuedDialogueResponse : BasicResponse
    {
        private readonly ISpeaker _speaker;
        private readonly Conversation _conversation;
    
        public PlayQueuedDialogueResponse(DialogueNode node, string text, Sprite icon, int row, int column, IResponseStyle style, ISpeaker speaker, Conversation conversation) : base(node, text, icon, row, column, style)
        {
            _speaker = speaker;
            _conversation = conversation;
        }

        public override void ExecuteResponse()
        {
            if (_conversation) ConversationManager.EnqueueConversation(_speaker, _conversation);
        }
    }
}