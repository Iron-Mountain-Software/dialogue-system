using System.Collections.Generic;
using ARISE.DialogueSystem.Responses;

namespace ARISE.DialogueSystem.Nodes.ResponseGenerators
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
        
        public override List<BasicResponse> GetDialogueResponses(ConversationUI conversationUI)
        {
            List<BasicResponse> dialogueResponses = new List<BasicResponse>
            {
                new NeverMindResponse(this, Text, row, column)
            };
            return dialogueResponses;
        }
    }
}