using System;
using UnityEngine;
using UnityEngine.UI;

namespace SpellBoundAR.DialogueSystem.UI
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
            conversationPlayer.OnDialogueLinePlayed += OnDialogueLinePlayed;
        }

        private void OnDisable()
        {
            conversationPlayer.OnDialogueLinePlayed -= OnDialogueLinePlayed;
        }

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