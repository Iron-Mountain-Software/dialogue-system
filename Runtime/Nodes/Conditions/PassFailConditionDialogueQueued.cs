using IronMountain.DialogueSystem.Speakers;

namespace IronMountain.DialogueSystem.Nodes.Conditions
{
    public class PassFailConditionDialogueQueued : PassFailCondition
    {
        public override string Name => "DIALOGUE QUEUED CHECK";

        protected override bool TestCondition(ConversationPlayer conversationPlayer)
        {
            ISpeaker thisSpeaker = conversationPlayer.DefaultSpeaker;
            Conversation thisConversation = conversationPlayer.Conversation;
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