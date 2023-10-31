using System.Collections.Generic;
using SpellBoundAR.DialogueSystem.Responses;
using SpellBoundAR.DialogueSystem.Speakers;
using SpellBoundAR.DialogueSystem.UI;
using UnityEngine;

namespace SpellBoundAR.DialogueSystem.Nodes.ResponseGenerators
{
    [NodeWidth(200)]
    [NodeTint("#80461B")]
    public class ResponseGeneratorActiveDialogue : ResponseGenerator
    {
        public override string Name => "ACTIVE CONVERSATIONS";
    
        public override List<BasicResponse> GetDialogueResponses(ConversationPlayer conversationPlayer)
        {
            List<BasicResponse> dialogueResponses = new List<BasicResponse>();
            ISpeaker thisSpeaker = conversationPlayer.DefaultSpeaker;
            Conversation thisConversation = conversationPlayer.CurrentConversation;
            IResponseStyle style = ScriptedResponseStyle
                ? ScriptedResponseStyle
                : new ResponseStyle(
                    .11f,
                    new Color(0.94f, 0.82f, 0.55f),
                    new Color(0.58f, 0.5f, 0.35f),
                    new Color(0.24f, 0.1f, 0.04f)
                );
            var rowOffset = 0;
            foreach (Conversation conversation in thisSpeaker.Conversations)
            {
                if (conversation
                    && conversation.IsActive
                    && conversation != thisConversation
                    && !conversation.PrioritizeOverDefault)
                {
                    dialogueResponses.Add(new PlayConversationResponse(
                        conversationPlayer,
                        this,
                        conversation.InvokingLine,
                        conversation.InvokingIcon,
                        row + rowOffset,
                        column,
                        style,
                        thisSpeaker,
                        conversation));
                    rowOffset++;
                }
            }
            return dialogueResponses;
        }
    }
}