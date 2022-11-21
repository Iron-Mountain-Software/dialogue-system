using UnityEngine;
using UnityEngine.UI;

namespace ARISE.DialogueSystem.UI
{
    public class DialogueLineImage : MonoBehaviour
    {
        [SerializeField] private Image image;
        
        private void Awake() => ConversationUI.OnDialogueLinePlayed += OnDialogueLinePlayed;
        private void OnDestroy() => ConversationUI.OnDialogueLinePlayed -= OnDialogueLinePlayed;
        
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
            image.color = sprite ? Color.white : Color.clear;
            image.preserveAspect = true;
        }
    }
}