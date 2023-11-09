using UnityEngine;
using UnityEngine.UI;

namespace IronMountain.DialogueSystem.UI
{
    [RequireComponent(typeof(Image))]
    public class DialogueLineImage : MonoBehaviour
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
            if (dialogueLine == null) return;
            SetImage(dialogueLine.Sprite);
        }

        private void SetImage(Sprite sprite)
        {
            if (!image) return;
            image.sprite = sprite;
            image.enabled = sprite;
            image.preserveAspect = true;
        }
    }
}