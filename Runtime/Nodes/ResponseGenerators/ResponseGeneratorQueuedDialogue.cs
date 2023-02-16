using System.Collections.Generic;
using SpellBoundAR.DialogueSystem.Responses;
using UnityEngine;

namespace SpellBoundAR.DialogueSystem.Nodes.ResponseGenerators
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
            IResponseStyle style = ScriptedResponseStyle
                ? ScriptedResponseStyle
                : new ResponseStyle(
                    .11f,
                    new Color(0.94f, 0.82f, 0.55f),
                    new Color(0.58f, 0.5f, 0.35f),
                    new Color(0.24f, 0.1f, 0.04f)
                );
            var rowOffset = 0;
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
                        style,
                        conversation));
                    rowOffset++;
                }
            }
            return dialogueResponses;
        }
    }
}