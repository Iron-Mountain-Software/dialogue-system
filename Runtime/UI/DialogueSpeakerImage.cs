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
            ConversationPlayer.OnDialogueLinePlayed += OnDialogueLinePlayed;
        }

        private void OnDestroy()
        {
            ConversationPlayer.OnDialogueLinePlayed -= OnDialogueLinePlayed;
        }

        private void OnDialogueLinePlayed(Conversation conversation, DialogueLine dialogueLine)
        {
            SetImageSprite(dialogueLine is {Speaker: {Portraits: { }}}
                ? dialogueLine.Speaker.Portraits.GetPortrait(dialogueLine.Portrait)
                : null);
        }

        private void SetImageSprite(Sprite sprite)
        {
            if (!_image) return;
            _image.sprite = sprite;
            _image.enabled = _image.sprite;
            _image.preserveAspect = true;
        }
    }
}
