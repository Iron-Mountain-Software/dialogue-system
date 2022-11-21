using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace ARISE.DialogueSystem.DialogueBubbles
{
    public class DialogueBubble : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private TextMeshPro text;

        [Header("Sprites")]
        [SerializeField] private Sprite speechBubbleSprite;
        [SerializeField] private Color speechBubbleColor;
        [SerializeField] private Sprite thoughtBubbleSprite;
        [SerializeField] private Color thoughtBubbleColor;

        [Header("Cache")]
        private DialogueBubbleAnimator _animator;

        private DialogueTrigger DialogueTrigger { get; set; }

        private void Awake()
        {
            _animator = GetComponent<DialogueBubbleAnimator>();
            DialogueTrigger = GetComponentInParent<DialogueTrigger>();
            if (!DialogueTrigger) return;
            DialogueTrigger.OnEnabled += OnDialogueTriggerEnabled;
            DialogueTrigger.OnDisabled += OnDialogueTriggerDisabled;
        }

        private void OnDestroy()
        {
            if (!DialogueTrigger) return;
            DialogueTrigger.OnEnabled -= OnDialogueTriggerEnabled;
            DialogueTrigger.OnDisabled -= OnDialogueTriggerDisabled;
        }
        
        private void Start() => Refresh();

        private void OnDialogueTriggerEnabled()
        {
            if (DialogueTrigger && DialogueTrigger.ConversationSelector != null) DialogueTrigger.ConversationSelector.OnNextConversationChanged += ResolveState;
            Refresh();
        }

        private void OnDialogueTriggerDisabled()
        {
            if (DialogueTrigger && DialogueTrigger.ConversationSelector != null) DialogueTrigger.ConversationSelector.OnNextConversationChanged -= ResolveState;
            Refresh();
        }

        private void Refresh()
        {
            Conversation conversation = null;
            if (DialogueTrigger
                && DialogueTrigger.enabled
                && DialogueTrigger.ConversationSelector != null)
            {
                conversation = DialogueTrigger.ConversationSelector.NextConversation;
            }
            ResolveState(conversation);
        }

        private void ResolveState(Conversation conversation)
        {
            if (!DialogueTrigger || !DialogueTrigger.enabled || !conversation)
            {
                SetText(string.Empty);
                Disappear();
            }
            else if (conversation == DialogueTrigger.ConversationEntity.DefaultConversation)
            {
                List<Conversation> activeConversations = DialogueTrigger.ConversationEntity.GetActiveDialogue();
                foreach(Conversation activeConversation in activeConversations)
                {
                    if (activeConversation.PreviewType != ConversationPreviewType.None)
                    {
                        AppearFor(activeConversation ? activeConversation : conversation);
                        return;
                    }
                }
                AppearFor(conversation);
            }
            else AppearFor(conversation);
        }

        private void AppearFor(Conversation interaction)
        {
            switch (interaction.PreviewType)
            {
                case ConversationPreviewType.None:
                    SetText(string.Empty);
                    Disappear();
                    break;
                case ConversationPreviewType.SpeechBubble:
                    SetText(interaction.PreviewText);
                    SetSprite(speechBubbleSprite, speechBubbleColor);
                    Appear();
                    break;
                case ConversationPreviewType.ThoughtBubble:
                    SetText(interaction.PreviewText);
                    SetSprite(thoughtBubbleSprite, thoughtBubbleColor);
                    Appear();
                    break;
            }
        }

        private void SetText(string content)
        {
            if (!text) return;
            text.text = content;
        }

        private void SetSprite(Sprite sprite, Color color)
        {
            if (!spriteRenderer) return;
            spriteRenderer.sprite = sprite;
            spriteRenderer.color = color;
        }

        private void Appear()
        {
            if (_animator) _animator.ScaleUp();
        }

        private void Disappear()
        {
            if (_animator) _animator.ScaleDown();
        }
    }
}