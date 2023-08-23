namespace SpellBoundAR.DialogueSystem.Nodes.Conditions
{
    public class PassFailConditionDialogueQueued : PassFailCondition
    {
        public override string Name => "DIALOGUE QUEUED CHECK";

        protected override bool TestCondition(ConversationUI conversationUI)
        {
            Conversation thisConversation = conversationUI.CurrentConversation;
            foreach (Conversation testConversation in thisConversation.Speaker.Conversations)
            {
                if (testConversation
                    && testConversation.IsActive
                    && testConversation != thisConversation
                    && !testConversation.PrioritizeOverDefault)
                {
                    return true;
                }
            }
            return false;
        }
        
#if UNITY_EDITOR

        protected override bool ExtensionHasWarnings()
        {
            return false;
        }

        protected override bool ExtensionHasErrors()
        {
            return false;
        }
		
#endif
    }
}