using SpellBoundAR.DialogueSystem.Speakers;
using UnityEngine;
using UnityEngine.UI;

namespace SpellBoundAR.DialogueSystem.UI
{
    public class DialogueLineImage : MonoBehaviour
    {
        [SerializeField] private Image image;
        
        private void Awake() => ConversationPlayer.OnDialogueLinePlayed += OnDialogueLinePlayed;
        private void OnDestroy() => ConversationPlayer.OnDialogueLinePlayed -= OnDialogueLinePlayed;
        
        private void OnDialogueLinePlayed(Conversation conversation, DialogueLine dialogueLine)
        {
            if (dialogueLine == null) return;
            SetImage(dialogueLine.Sprite);
            gameObject.SetActive(dialogueLine.Sprite);
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