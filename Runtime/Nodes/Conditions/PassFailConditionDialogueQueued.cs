using UnityEngine;

namespace SpellBoundAR.DialogueSystem.Nodes.Conditions
{
    public class PassFailConditionDialogueQueued : PassFailCondition
    {
        public override string Name => "DIALOGUE QUEUED CHECK";

        protected override bool TestCondition(ConversationUI conversationUI)
        {
            Debug.Log("RUNNING DIALOGUE QUEUED CHECK!");
            Conversation thisConversation = conversationUI.CurrentConversation;
            Debug.Log("thisConversation: " + thisConversation.Name);
            foreach (Conversation testConversation in thisConversation.Speaker.Conversations)
            {
                Debug.Log("Testing: " + testConversation);
                Debug.Log("is active: " + testConversation.IsActive);
                Debug.Log("is not this: " + (testConversation != thisConversation));
                Debug.Log("is not prioritized: " + !testConversation.PrioritizeOverDefault);
                if (testConversation
                    && testConversation.IsActive
                    && testConversation != thisConversation
                    && !testConversation.PrioritizeOverDefault)
                {
                    Debug.Log("RETIRNING TRUE");
                    return true;
                }
            }
            Debug.Log("RETIRNING FALSE");
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