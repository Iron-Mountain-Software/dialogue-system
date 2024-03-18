using IronMountain.DialogueSystem.Speakers;
using IronMountain.DialogueSystem.UI;
using UnityEngine;

namespace IronMountain.DialogueSystem.Responses
{
    public class SwapConversationResponse : BasicResponse
    {
        private readonly ConversationPlayer _conversationPlayer;
        private readonly ISpeaker _speaker;
        private readonly Conversation _conversation;
        
        public override void Execute()
        {
            if (!_conversationPlayer) return;
            _conversationPlayer.CompleteConversation();
            if (_speaker == null || !_conversation) return;
            _conversationPlayer.Initialize(_speaker, _conversation);
        }
        
        public SwapConversationResponse(ConversationPlayer conversationPlayer, ISpeaker speaker, Conversation conversation, string text, Sprite icon, int row, int column, IResponseStyle style)
            : base(text, icon, row, column, style)
        {
            _conversationPlayer = conversationPlayer;
            _speaker = speaker;
            _conversation = conversation;
        }
    }
}