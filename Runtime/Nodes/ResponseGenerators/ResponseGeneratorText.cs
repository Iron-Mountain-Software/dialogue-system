using System.Collections.Generic;
using SpellBoundAR.DialogueSystem.Responses;
using IronMountain.ResourceUtilities;
using UnityEngine;
using UnityEngine.Localization;

namespace SpellBoundAR.DialogueSystem.Nodes.ResponseGenerators
{
    public class ResponseGeneratorText : ResponseGenerator
    {
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
                    return text.IsEmpty ? string.Empty : text.GetLocalizedString();
                }
#if UNITY_EDITOR
                if (text.IsEmpty || string.IsNullOrEmpty(text.TableReference)) return string.Empty;
                var collection = UnityEditor.Localization.LocalizationEditorSettings.GetStringTableCollection(text.TableReference);
                var entry = collection.SharedData.GetEntryFromReference(text.TableEntryReference);
                return entry != null ? entry.Key : string.Empty;
#else
				return string.Empty;
#endif
            }
        }

        private Sprite Sprite => sprite ? sprite.Asset : null;
    
        public override List<BasicResponse> GetDialogueResponses(ConversationUI conversationUI)
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
                new (this, Text, Sprite, row, column, style)
            };
            return dialogueResponses;
        }

#if UNITY_EDITOR

        protected override bool ExtensionHasErrors()
        {
            return text.IsEmpty || string.IsNullOrEmpty(text.TableReference) || base.ExtensionHasErrors();
        }
        
#endif
        
    }
}