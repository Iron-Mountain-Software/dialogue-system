using System.Collections.Generic;
using SpellBoundAR.DialogueSystem.Responses;

namespace SpellBoundAR.DialogueSystem.Nodes.ResponseGenerators
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
        
        public override List<BasicResponse> GetDialogueResponses(ConversationUI conversationUI)
        {
            List<BasicResponse> dialogueResponses = new List<BasicResponse>
            {
                new ChatResponse(this, Text, row, column)
            };
            return dialogueResponses;
        }
    }
}