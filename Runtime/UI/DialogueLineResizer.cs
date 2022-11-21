using UnityEngine;

namespace ARISE.DialogueSystem.UI
{
    public class DialogueLineResizer : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private Vector2 minimumAnchorWithImage = Vector2.zero;
        [SerializeField] private Vector2 maximumAnchorWithImage = Vector2.one;
        
        [SerializeField] private Vector2 minimumAnchorWithoutImage = Vector2.zero;
        [SerializeField] private Vector2 maximumAnchorWithoutImage = Vector2.one;

        [Header("Cache")]
        private RectTransform _rectTransform;
        
        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            ConversationUI.OnDialogueLinePlayed += OnDialogueLinePlayed;
        }

        private void OnDestroy()
        {
            ConversationUI.OnDialogueLinePlayed -= OnDialogueLinePlayed;
        }

        private void OnDialogueLinePlayed(Conversation conversation, DialogueLine dialogueLine)
        {
            if (dialogueLine == null) return;
            if (dialogueLine.Sprite)
            {
                _rectTransform.anchorMin = minimumAnchorWithImage;
                _rectTransform.anchorMax = maximumAnchorWithImage;
            }
            else
            {
                _rectTransform.anchorMin = minimumAnchorWithoutImage;
                _rectTransform.anchorMax = maximumAnchorWithoutImage;
            }
            _rectTransform.offsetMin = Vector2.zero;
            _rectTransform.offsetMax = Vector2.zero;
        }
    }
}
