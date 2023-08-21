namespace SpellBoundAR.DialogueSystem.Selection
{
    public class DefaultConversationSelector : ConversationSelector
    {
        public override void RefreshNextConversation()
        {
            NextConversation = Speaker ? Speaker.DefaultConversation : null;
        }
    }
}