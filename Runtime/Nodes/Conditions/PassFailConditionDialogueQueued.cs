namespace SpellBoundAR.DialogueSystem.Nodes.Conditions
{
    public class PassFailConditionDialogueQueued : PassFailCondition
    {
        public override string Name => "DIALOGUE QUEUED CHECK";

        protected override bool TestCondition(ConversationUI conversationUI)
        {
            return conversationUI.CurrentConversation.Entity.GetActiveDialogue().Count > 0;
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