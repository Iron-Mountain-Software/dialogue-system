using System.Collections.Generic;
using IronMountain.DialogueSystem.Responses;
using IronMountain.DialogueSystem.UI;
using UnityEngine;

namespace IronMountain.DialogueSystem.Nodes.ResponseGenerators
{
    [NodeWidth(250)]
    [NodeTint("#80461B")]
    public class ResponseGeneratorTextChat : ResponseGeneratorText
    {
        public override string Name
        {
            get
            {
#if UNITY_EDITOR
                if (text.IsEmpty) return "CHAT: EMPTY!";
                var collection = UnityEditor.Localization.LocalizationEditorSettings.GetStringTableCollection(text.TableReference);
                var entry = collection.SharedData.GetEntryFromReference(text.TableEntryReference);
                return "CHAT: " + entry.Key;
#else
                return "CHAT";
#endif
            }
        }
        
        public override List<BasicResponse> GetDialogueResponses(ConversationPlayer conversationPlayer)
        {
            IResponseStyle style = ScriptedResponseStyle
                ? ScriptedResponseStyle
                : new ResponseStyle(
                    .08f,
                    new Color(0.94f, 0.82f, 0.55f),
                    new Color(0.58f, 0.5f, 0.35f),
                    new Color(0.24f, 0.1f, 0.04f)
                );
            List<BasicResponse> dialogueResponses = new List<BasicResponse>
            {
                new (conversationPlayer, this, Text, null, row, column, style)
            };
            return dialogueResponses;
        }
    }
}