using SpellBoundAR.DialogueSystem.Nodes;
using SpellBoundAR.DialogueSystem.Speakers;
using SpellBoundAR.DialogueSystem.UI;
using UnityEngine;

namespace SpellBoundAR.DialogueSystem.Responses
{
    public class PlayConversationResponse : BasicResponse
    {
        private readonly ConversationPlayer _conversationPlayer;
        private readonly ISpeaker _speaker;
        private readonly Conversation _conversation;
    
        public PlayConversationResponse(ConversationPlayer conversationPlayer, DialogueNode node, string text, Sprite icon, int row, int column, IResponseStyle style, ISpeaker speaker, Conversation conversation) : base(node, text, icon, row, column, style)
        {
            _conversationPlayer = conversationPlayer;
            _speaker = speaker;
            _conversation = conversation;
        }

        public override void ExecuteResponse()
        {
            if (!_conversationPlayer || _speaker == null || !_conversation) return;
            _conversationPlayer.CompleteConversation();
            _conversationPlayer.Initialize(_speaker, _conversation);
        }
    }
}