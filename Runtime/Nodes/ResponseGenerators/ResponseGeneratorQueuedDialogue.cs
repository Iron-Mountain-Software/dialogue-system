using System.Collections.Generic;
using ARISE.DialogueSystem.Responses;

namespace ARISE.DialogueSystem.Nodes.ResponseGenerators
{
    [NodeWidth(200)]
    [NodeTint("#80461B")]
    public class ResponseGeneratorQueuedDialogue : ResponseGenerator
    {
        public override string Name => "QUEUED RESPONSES";
    
        public override List<BasicResponse> GetDialogueResponses(ConversationUI conversationUI)
        {
            List<BasicResponse> dialogueResponses = new List<BasicResponse>();
            List<Conversation> conversations = conversationUI.CurrentConversation.Entity.GetActiveDialogue();
            int rowOffset = 0;
            foreach (Conversation conversation in conversations)
            {
                if (conversation != conversationUI.CurrentConversation && !conversation.PrioritizeOverDefault)
                {
                    dialogueResponses.Add(new PlayQueuedDialogueResponse(
                        this, 
                        conversation.InvokingLine,
                        conversation.InvokingIcon,
                        row + rowOffset,
                        column, 
                        conversation));
                    rowOffset++;
                }
            }
            return dialogueResponses;
        }
    }
}