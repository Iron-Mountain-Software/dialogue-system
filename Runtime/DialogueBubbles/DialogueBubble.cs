using System;
using System.Collections.Generic;
using SpellBoundAR.DialogueSystem.Selection;
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
        private ConversationSelector ConversationSelector { get; set; }

        private void Awake()
        {
            _animator = GetComponent<DialogueBubbleAnimator>();
            SpeakerController = GetComponentInParent<SpeakerController>();
            ConversationSelector = GetComponentInParent<ConversationSelector>();
        }

        private void OnEnable()
        {
            if (ConversationSelector) ConversationSelector.OnNextConversationChanged += ResolveConversation;
            if (SpeakerController)
            {
                SpeakerController.OnEnabled += Refresh;
                SpeakerController.OnDisabled += Refresh;
            }
            Refresh();
        }

        private void OnDisable()
        {
            if (ConversationSelector) ConversationSelector.OnNextConversationChanged -= ResolveConversation;
            if (SpeakerController)
            {
                SpeakerController.OnEnabled -= Refresh;
                SpeakerController.OnDisabled -= Refresh;
            }
        }

        private void Refresh()
        {
            Conversation nextConversation = ConversationSelector ? ConversationSelector.NextConversation : null;
            ResolveConversation(nextConversation);
        }

        private void ResolveConversation(Conversation nextConversation)
        {
            if (!SpeakerController || !SpeakerController.enabled || !nextConversation)
            {
                SetText(string.Empty);
                Disappear();
                return;
            }
            
            if (nextConversation == SpeakerController.Speaker.DefaultConversation)
            {
                foreach(Conversation testConversation in SpeakerController.Speaker.Conversations)
                {
                    if (!testConversation
                        || testConversation.PreviewType == ConversationPreviewType.None
                        || testConversation == SpeakerController.Speaker.DefaultConversation
                        || !testConversation.IsActive) continue;
                    AppearFor(testConversation);
                    return;
                }
            }
            
            AppearFor(nextConversation);
        }

        private void AppearFor(Conversation conversation)
        {
            switch (conversation.PreviewType)
            {
                case ConversationPreviewType.None:
                    SetText(string.Empty);
                    Disappear();
                    break;
                case ConversationPreviewType.SpeechBubble:
                    SetText(conversation.PreviewText);
                    SetSprite(speechBubbleSprite, speechBubbleColor);
                    Appear();
                    break;
                case ConversationPreviewType.ThoughtBubble:
                    SetText(conversation.PreviewText);
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