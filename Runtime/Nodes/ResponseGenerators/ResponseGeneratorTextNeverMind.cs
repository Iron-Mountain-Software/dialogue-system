using System.Collections.Generic;
using SpellBoundAR.DialogueSystem.Responses;
using SpellBoundAR.DialogueSystem.UI;
using UnityEngine;

namespace SpellBoundAR.DialogueSystem.Nodes.ResponseGenerators
{
    [NodeWidth(250)]
    [NodeTint("#EF604F")]
    public class ResponseGeneratorTextNeverMind : ResponseGeneratorText
    {
        public override string Name
        {
            get
            {
#if UNITY_EDITOR
                if (text.IsEmpty) return "NEVER MIND: EMPTY!";
                var collection = UnityEditor.Localization.LocalizationEditorSettings.GetStringTableCollection(text.TableReference);
                var entry = collection.SharedData.GetEntryFromReference(text.TableEntryReference);
                return "NEVER MIND: " + entry.Key;
#else
                return "NEVER MIND";
#endif
            }
        }
        
        public override List<BasicResponse> GetDialogueResponses(ConversationPlayer conversationPlayer)
        {
            IResponseStyle style = ScriptedResponseStyle
                ? ScriptedResponseStyle
                : new ResponseStyle(
                    .08f,
                    new Color(0.94f, 0.38f, 0.31f),
                    new Color(0.68f, 0.2f, 0.17f),
                    Color.white
                );
            List<BasicResponse> dialogueResponses = new List<BasicResponse>
            {
                new (conversationPlayer, this, Text, null, row, column, style)
            };
            return dialogueResponses;
        }
    }
}