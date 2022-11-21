namespace ARISE.DialogueSystem.Nodes.Actions
{
    [NodeWidth(200)]
    [NodeTint("#FFCA3A")]
    public class DialoguePlayPrioritized : DialogueAction
    {
        protected override void HandleAction()
        {
            if (graph is Conversation conversation)
            {
                Conversation nextConversation = null;
                foreach (Conversation testConversation in conversation.Entity.GetActiveDialogue())
                {
                    if (!testConversation || testConversation == conversation || !testConversation.PrioritizeOverDefault) continue;
                    if (!nextConversation || testConversation.Priority < nextConversation.Priority) nextConversation = testConversation;
                }
                if (nextConversation) ConversationManager.EnqueueConversation(nextConversation);
            }
        }

        public override string Name => "Play Next Interaction";
    }
}