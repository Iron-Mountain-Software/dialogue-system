using SpellBoundAR.DialogueSystem.Speakers;
using UnityEngine;
using UnityEngine.UI;

namespace SpellBoundAR.DialogueSystem.UI
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

        private void OnDialogueLinePlayed(ISpeaker speaker, Conversation conversation, DialogueLine dialogueLine)
        {
            if (!conversation || speaker == null || dialogueLine == null) return;
            Sprite sprite = speaker.Portraits.GetPortrait(dialogueLine.Portrait);
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
