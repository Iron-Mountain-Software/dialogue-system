namespace IronMountain.DialogueSystem.Selection
{
    public class DefaultConversationSelector : ConversationSelector
    {
        public override void RefreshNextConversation()
        {
            NextConversation = Speaker?.DefaultConversation;
        }
    }
}