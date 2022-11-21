﻿using System.Collections.Generic;
using ARISE.DialogueSystem.Responses;
using SpellBoundAR.ResourceUtilities;
using UnityEngine;
using UnityEngine.Localization;

namespace ARISE.DialogueSystem.Nodes.ResponseGenerators
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
            List<BasicResponse> dialogueResponses = new List<BasicResponse>
            {
                new BasicResponse(this, Text, Sprite, row, column)
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