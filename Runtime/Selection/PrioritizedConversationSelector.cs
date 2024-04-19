namespace IronMountain.DialogueSystem.Selection
{
    public class PrioritizedConversationSelector : ConversationSelector
    {
        public override void RefreshNextConversation()
        {
            Conversation conversation = null;
            if (Speaker != null && AllConversations != null)
            {
                foreach (Conversation testConversation in AllConversations)
                {
                    if (!testConversation || !testConversation.IsActive
                        || !testConversation.PrioritizeOverDefault) continue;
                    if (conversation && testConversation.Priority >= conversation.Priority) continue;
                    conversation = testConversation;
                }
                if (!conversation) conversation = Speaker.DefaultConversation;
            }
            NextConversation = conversation;
        }
    }
}