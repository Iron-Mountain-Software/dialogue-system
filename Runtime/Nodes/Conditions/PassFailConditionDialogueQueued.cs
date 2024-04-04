using IronMountain.DialogueSystem.Speakers;
using IronMountain.DialogueSystem.UI;

namespace IronMountain.DialogueSystem.Nodes.Conditions
{
    public class PassFailConditionDialogueQueued : PassFailCondition
    {
        public override string Name => "DIALOGUE QUEUED CHECK";

        protected override bool TestCondition(ConversationPlayer conversationUI)
        {
            ISpeaker thisSpeaker = conversationUI.DefaultSpeaker;
            Conversation thisConversation = conversationUI.Conversation;
            foreach (Conversation testConversation in thisSpeaker.Conversations)
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
    }
}