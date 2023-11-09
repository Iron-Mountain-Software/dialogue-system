using UnityEngine;
using UnityEngine.UI;

namespace IronMountain.DialogueSystem.UI
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Image))]
    public class SpeakerPortraitImage : MonoBehaviour
    {
        [SerializeField] private ConversationPlayer conversationPlayer;
        [SerializeField] private Image image;
    
        private void Awake()
        {
            if (!conversationPlayer) conversationPlayer = GetComponentInParent<ConversationPlayer>();
            if (!image) image = GetComponent<Image>();
        }

        private void OnValidate()
        {
            if (!conversationPlayer) conversationPlayer = GetComponentInParent<ConversationPlayer>();
            if (!image) image = GetComponent<Image>();
        }

        private void OnEnable()
        {
            if (conversationPlayer) conversationPlayer.OnDialogueLinePlayed += OnDialogueLinePlayed;
        }

        private void OnDisable()
        {
            if (conversationPlayer) conversationPlayer.OnDialogueLinePlayed -= OnDialogueLinePlayed;
        }

        private void OnDialogueLinePlayed(Conversation conversation, DialogueLine dialogueLine)
        {
            SetImageSprite(dialogueLine is {Speaker: {Portraits: { }}}
                ? dialogueLine.Speaker.Portraits.GetPortrait(dialogueLine.Portrait)
                : null);
        }

        private void SetImageSprite(Sprite sprite)
        {
            if (!image) return;
            image.sprite = sprite;
            image.enabled = image.sprite;
            image.preserveAspect = true;
        }
    }
}
