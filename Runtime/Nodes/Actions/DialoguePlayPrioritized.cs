using IronMountain.DialogueSystem.Speakers;
using IronMountain.DialogueSystem.UI;

namespace IronMountain.DialogueSystem.Nodes.Actions
{
    [NodeWidth(200)]
    [NodeTint("#FFCA3A")]
    public class DialoguePlayPrioritized : DialogueAction
    {
        protected override void HandleAction(ConversationPlayer conversationUI)
        {
            if (graph is not Conversation conversation) return;
            ISpeaker currentSpeaker = conversationUI.DefaultSpeaker;
            Conversation nextConversation = null;
            foreach (Conversation testConversation in currentSpeaker.Conversations)
            {
                if (!testConversation
                    || !testConversation.IsActive
                    || testConversation == conversation
                    || !testConversation.PrioritizeOverDefault) continue;
                if (!nextConversation || testConversation.Priority < nextConversation.Priority) nextConversation = testConversation;
            }
            if (nextConversation)
            {
                conversationUI.CompleteConversation();
                conversationUI.Initialize(currentSpeaker, nextConversation);
            }
        }

        public override string Name => "Play Next Interaction";
    }
}