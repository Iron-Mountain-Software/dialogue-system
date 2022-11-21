using UnityEngine;
using UnityEngine.UI;

namespace ARISE.DialogueSystem.UI
{
    [RequireComponent(typeof(Image))]
    public class DialogueSpeakerImage : MonoBehaviour
    {
        [Header("Cache")]
        private Image _image;
    
        private void Awake()
        {
            _image = GetComponent<Image>();
            ConversationUI.OnDialogueLinePlayed += OnDialogueLinePlayed;
        }

        private void OnDestroy()
        {
            ConversationUI.OnDialogueLinePlayed -= OnDialogueLinePlayed;
        }

        private void OnDialogueLinePlayed(Conversation conversation, DialogueLine dialogueLine)
        {
            if (!conversation || conversation.Entity == null || dialogueLine == null) return;
            Sprite sprite = conversation.Entity.GetPortrait(dialogueLine.Portrait);
            SetImageSprite(sprite);
        }

        private void SetImageSprite(Sprite sprite)
        {
            if (!_image) return;
            _image.sprite = sprite;
            _image.preserveAspect = true;
        }
    }
}
