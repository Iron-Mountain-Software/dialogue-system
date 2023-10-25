using SpellBoundAR.DialogueSystem.Responses;
using UnityEngine;
using UnityEngine.UI;

namespace SpellBoundAR.DialogueSystem.UI
{
    public class UI_DialogueResponseWithIcon : DialogueResponseButton
    {
        [SerializeField] private Image icon;

        public override void Initialize(BasicResponse basicResponse, ConversationPlayer conversationUI)
        {
            if (basicResponse == null) { Destroy(gameObject); return; }
            base.Initialize(basicResponse, conversationUI);
            if (!icon) return;
            if (basicResponse.Icon)
            {
                icon.sprite = basicResponse.Icon;
                icon.color = Color.white;
                icon.preserveAspect = true;
            }
            else
            {
                icon.color = Color.clear;
            }
        }
    }
}