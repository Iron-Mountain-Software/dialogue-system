namespace SpellBoundAR.DialogueSystem.Selection
{
    public class PrioritizedConversationSelector : ConversationSelector
    {
        public override void RefreshNextConversation()
        {
            Conversation conversation = null;
            if (Speaker)
            {
                foreach (Conversation testConversation in Speaker.Conversations)
                {
                    if (!testConversation) continue;
                    if (!testConversation.PrioritizeOverDefault) continue;
                    if (!testConversation.Looping && testConversation.Playthroughs > 0) continue;
                    if (!testConversation.Condition || !testConversation.Condition.Evaluate()) continue;
                    if (conversation && testConversation.Priority >= conversation.Priority) continue;
                    conversation = testConversation;
                }
                if (!conversation) conversation = Speaker.DefaultConversation;
            }
            NextConversation = conversation;
        }
    }
}