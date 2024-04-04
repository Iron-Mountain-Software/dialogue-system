using System.Collections.Generic;
using IronMountain.DialogueSystem.Responses;
using IronMountain.DialogueSystem.UI;
using IronMountain.ResourceUtilities;
using UnityEngine;
using UnityEngine.Localization;

namespace IronMountain.DialogueSystem.Nodes.ResponseGenerators
{
    public class BasicDialogueResponseNode : DialogueResponseNode
    {
        [SerializeField] protected string stringText;
        [SerializeField] protected LocalizedString text;
        [SerializeField] protected ResourceSprite sprite;

        public override string Name
        {
            get
            {
#if UNITY_EDITOR
                if (text.IsEmpty) return "EMPTY RESPONSE!";
                var collection = UnityEditor.Localization.LocalizationEditorSettings.GetStringTableCollection(text.TableReference);
                var entry = collection.SharedData.GetEntryFromReference(text.TableEntryReference);
                if (entry == null) return "EMPTY RESPONSE!";
                return entry.Key.Length > 44 ? entry.Key.Substring(0, 44) + "..." : entry.Key;
#else
                return "RESPONSE";
#endif
            }
        }

        public string Text 
        {
            get
            {
                if (Application.isPlaying)
                {
                    return text.IsEmpty ? stringText : text.GetLocalizedString();
                }
#if UNITY_EDITOR
                if (text.IsEmpty || string.IsNullOrEmpty(text.TableReference)) return stringText;
                var collection = UnityEditor.Localization.LocalizationEditorSettings.GetStringTableCollection(text.TableReference);
                var entry = collection.SharedData.GetEntryFromReference(text.TableEntryReference);
                return entry != null ? entry.Key : stringText;
#else
				return string.Empty;
#endif
            }
        }
        
        public LocalizedString LocalizedText => text;

        private Sprite Sprite => sprite ? sprite.Asset : null;
    
        public override List<BasicResponse> GetDialogueResponses(ConversationPlayer conversationPlayer)
        {
            IResponseStyle style = ScriptedResponseStyle
                ? ScriptedResponseStyle
                : new ResponseStyle(
                    .11f,
                    new Color(0.94f, 0.82f, 0.55f),
                    new Color(0.58f, 0.5f, 0.35f),
                    new Color(0.24f, 0.1f, 0.04f)
                );
            List<BasicResponse> dialogueResponses = new List<BasicResponse>
            {
                new BranchConversationResponse(conversationPlayer, this, Text, Sprite, row, column, style)
            };
            return dialogueResponses;
        }

#if UNITY_EDITOR

        private bool MissingText => string.IsNullOrWhiteSpace(stringText) && (text.IsEmpty || string.IsNullOrEmpty(text.TableReference));

        public override void RefreshWarnings()
        {
            base.RefreshWarnings();
            if (MissingText) Warnings.Add("No text.");
        }
        
#endif
        
    }
}