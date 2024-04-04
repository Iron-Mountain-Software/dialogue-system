using System.Collections.Generic;
using IronMountain.DialogueSystem.UI;
using UnityEngine;

namespace IronMountain.DialogueSystem.Nodes
{
    public class DialogueLineWithAlternatesNode : DialogueLineNode
    {
        [SerializeField] private List<DialogueLineMainContent> alternateContent;

        public List<DialogueLineMainContent> AlternateContent => alternateContent;
        
        protected override DialogueLine GetDialogueLine(ConversationPlayer conversationUI)
        {
            int random = Random.Range(-1, alternateContent.Count);
            if (random == -1)
            {
                return new DialogueLine(
                    CustomSpeaker ? CustomSpeaker : conversationUI.DefaultSpeaker,
                    Text,
                    AudioClip,
                    portrait,
                    animation,
                    sprite ? sprite.Asset : null
                );
            }
            return new DialogueLine(
                CustomSpeaker ? CustomSpeaker : conversationUI.DefaultSpeaker,
                alternateContent[random].Text,
                alternateContent[random].AudioClip,
                portrait,
                animation,
                sprite ? sprite.Asset : null
            );
        }
        
#if UNITY_EDITOR

        public override void RefreshWarnings()
        {
            base.RefreshWarnings();
            foreach (DialogueLineMainContent content in alternateContent)
            {
                if (!content.AudioClip) Warnings.Add("No audio.");
                if (content.TextData.IsEmpty || string.IsNullOrEmpty(content.TextData.TableReference)) Warnings.Add("No text.");
            }
        }

#endif
        
    }
}
