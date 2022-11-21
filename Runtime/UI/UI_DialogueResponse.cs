using SpellBoundAR.DialogueSystem.Responses;
using UnityEngine;
using UnityEngine.UI;

namespace SpellBoundAR.DialogueSystem.UI
{
    public class UI_DialogueResponse : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Image buttonImagePrimary;
        [SerializeField] private Image buttonImageSecondary;
        [SerializeField] private Text text;

        [Header("Cache")]
        private BasicResponse _basicResponse;
        private ConversationUI _conversationUI;
    
        public virtual void Initialize(BasicResponse basicResponse, ConversationUI conversationUI)
        {
            if (basicResponse == null) { Destroy(gameObject); return; }
            _basicResponse = basicResponse;
            _conversationUI = conversationUI;
            if (buttonImagePrimary) buttonImagePrimary.color = _basicResponse.ButtonColorPrimary;
            if (buttonImageSecondary) buttonImageSecondary.color = _basicResponse.ButtonColorSecondary;
            if (text)
            {
                text.color = _basicResponse.TextColor;
                text.text = _basicResponse.Text;
            }
        }

        public void OnClick()
        {
            if (_basicResponse == null) return;
            _basicResponse.ExecuteResponse();
            _conversationUI.CurrentNode = _basicResponse.Node;
        }
    }
}