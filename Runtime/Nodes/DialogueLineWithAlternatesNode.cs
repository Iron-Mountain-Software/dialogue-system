using System.Collections.Generic;
using UnityEngine;

namespace SpellBoundAR.DialogueSystem.Nodes
{
    public class DialogueLineWithAlternatesNode : DialogueLineNode
    {
        [SerializeField] private List<DialogueLineMainContent> alternateContent;

        public List<DialogueLineMainContent> AlternateContent => alternateContent;
        
        protected override DialogueLine GetDialogueLine()
        {
            int random = Random.Range(-1, alternateContent.Count);
            if (random == -1)
            {
                return new DialogueLine(
                    text.IsEmpty ? string.Empty : text.GetLocalizedString(),
                    audioClip,
                    portrait,
                    animation,
                    sprite ? sprite.Asset : null,
                    virtualCameraReference
                );
            }
            return new DialogueLine(
                alternateContent[random].Text,
                alternateContent[random].AudioClip,
                portrait,
                animation,
                sprite ? sprite.Asset : null,
                virtualCameraReference
            );
        }
        
#if UNITY_EDITOR

        protected override bool ExtensionHasWarnings()
        {
            foreach (DialogueLineMainContent content in alternateContent)
            {
                if (!content.AudioClip) return true;
            }
            return base.ExtensionHasWarnings();
        }

        protected override bool ExtensionHasErrors()
        {
            foreach (DialogueLineMainContent content in alternateContent)
            {
                if (content.TextData.IsEmpty || string.IsNullOrEmpty(content.TextData.TableReference)) return true;
            }
            return base.ExtensionHasErrors();
        }
		
#endif
        
    }
}
