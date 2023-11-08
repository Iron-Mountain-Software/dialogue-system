using SpellBoundAR.DialogueSystem.Nodes;
using SpellBoundAR.DialogueSystem.Speakers;
using SpellBoundAR.DialogueSystem.UI;
using UnityEngine;

namespace SpellBoundAR.DialogueSystem.Responses
{
    public class PlayConversationResponse : BasicResponse
    {
        private readonly ISpeaker _speaker;
        private readonly Conversation _conversation;
        
        public override void ExecuteResponse()
        {
            if (ConversationPlayer && _speaker != null && _conversation)
            {
                ConversationPlayer.CompleteConversation();
                ConversationPlayer.Initialize(_speaker, _conversation);
            }
            else base.ExecuteResponse();
        }
        
        public PlayConversationResponse(ConversationPlayer conversationPlayer, DialogueNode sourceNode, string text, Sprite icon, int row, int column, IResponseStyle style, ISpeaker speaker, Conversation conversation)
            : base(conversationPlayer, sourceNode, text, icon, row, column, style)
        {
            _speaker = speaker;
            _conversation = conversation;
        }
    }
}