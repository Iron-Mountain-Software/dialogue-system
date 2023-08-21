using System.Collections.Generic;
using SpellBoundAR.DialogueSystem.Speakers;
using TMPro;
using UnityEngine;

namespace SpellBoundAR.DialogueSystem.DialogueBubbles
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

        private SpeakerController SpeakerController { get; set; }

        private void Awake()
        {
            _animator = GetComponent<DialogueBubbleAnimator>();
            SpeakerController = GetComponentInParent<SpeakerController>();
            if (!SpeakerController) return;
            SpeakerController.OnEnabled += OnDialogueTriggerEnabled;
            SpeakerController.OnDisabled += OnDialogueTriggerDisabled;
        }

        private void OnDestroy()
        {
            if (!SpeakerController) return;
            SpeakerController.OnEnabled -= OnDialogueTriggerEnabled;
            SpeakerController.OnDisabled -= OnDialogueTriggerDisabled;
        }
        
        private void Start() => Refresh();

        private void OnDialogueTriggerEnabled()
        {
            if (SpeakerController && SpeakerController.ConversationSelector != null) SpeakerController.ConversationSelector.OnNextConversationChanged += ResolveState;
            Refresh();
        }

        private void OnDialogueTriggerDisabled()
        {
            if (SpeakerController && SpeakerController.ConversationSelector != null) SpeakerController.ConversationSelector.OnNextConversationChanged -= ResolveState;
            Refresh();
        }

        private void Refresh()
        {
            Conversation conversation = null;
            if (SpeakerController
                && SpeakerController.enabled
                && SpeakerController.ConversationSelector != null)
            {
                conversation = SpeakerController.ConversationSelector.NextConversation;
            }
            ResolveState(conversation);
        }

        private void ResolveState(Conversation conversation)
        {
            if (!SpeakerController || !SpeakerController.enabled || !conversation)
            {
                SetText(string.Empty);
                Disappear();
            }
            else if (conversation == SpeakerController.ConversationSelector.Speaker.DefaultConversation)
            {
                foreach(Conversation testConversation in SpeakerController.ConversationSelector.Speaker.Conversations)
                {
                    if (testConversation.IsActive && testConversation.PreviewType != ConversationPreviewType.None)
                    {
                        AppearFor(testConversation ? testConversation : conversation);
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