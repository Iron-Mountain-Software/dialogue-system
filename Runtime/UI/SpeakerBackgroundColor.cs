using System.Collections;
using IronMountain.DialogueSystem.Speakers;
using UnityEngine;
using UnityEngine.UI;

namespace IronMountain.DialogueSystem.UI
{
    [RequireComponent(typeof(Graphic))]
    public class SpeakerBackgroundColor : MonoBehaviour
    {
        [Header("Static Settings")]
        private const float AnimationSeconds = .5f;
    
        [Header("Settings")]
        [SerializeField] private Color happyColor = new Color(0.99f, 0.05f, 0.18f);
        [SerializeField] private Color neutralColor = new Color(0.99f, 0.56f, 0.05f);
        [SerializeField] private Color sadColor = new Color(0.06f, 0.25f, 0.98f);
        
        [Header("References")]
        [SerializeField] private ConversationPlayer conversationPlayer;
        [SerializeField] private Graphic graphic;
    
        private void Awake()
        {
            if (!conversationPlayer) conversationPlayer = GetComponentInParent<ConversationPlayer>();
            if (!graphic) graphic = GetComponent<Graphic>();
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
            if (!graphic || dialogueLine == null) return;
            StopAllCoroutines();
            switch(dialogueLine.Portrait)
            {
                case SpeakerPortraitCollection.PortraitType.Happy:
                    StartCoroutine(LerpColor(graphic.color, happyColor));
                    break;
                case SpeakerPortraitCollection.PortraitType.Neutral:
                    StartCoroutine(LerpColor(graphic.color, neutralColor));
                    break;
                case SpeakerPortraitCollection.PortraitType.Sad:
                    StartCoroutine(LerpColor(graphic.color, sadColor));
                    break;
            }
        }

        private IEnumerator LerpColor(Color startColor, Color endColor)
        {
            for (float timer = 0; timer < AnimationSeconds; timer += Time.deltaTime)
            {
                float progress = timer / AnimationSeconds;
                if (graphic) graphic.color = Color.Lerp(startColor, endColor, progress);
                yield return null;
            }
            if (graphic) graphic.color = endColor;
        }
    }
}