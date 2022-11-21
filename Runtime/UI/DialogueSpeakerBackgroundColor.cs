using System.Collections;
using SpellBoundAR.Characters;
using UnityEngine;
using UnityEngine.UI;

namespace ARISE.DialogueSystem.UI
{
    [RequireComponent(typeof(Image))]
    public class DialogueSpeakerBackgroundColor : MonoBehaviour
    {
        [Header("Static Settings")]
        private const float AnimationSeconds = .5f;
    
        [Header("Settings")]
        [SerializeField] private Color happyColor = new Color(0.99f, 0.05f, 0.18f);
        [SerializeField] private Color neutralColor = new Color(0.99f, 0.56f, 0.05f);
        [SerializeField] private Color sadColor = new Color(0.06f, 0.25f, 0.98f);
        
        [Header("Cache")]
        private Image _image;
    
        private void Awake()
        {
            _image = GetComponent<Image>();
            _image.sprite = null;
            _image.preserveAspect = true;
            ConversationUI.OnDialogueLinePlayed += OnDialogueLinePlayed;
        }

        private void OnDestroy()
        {
            ConversationUI.OnDialogueLinePlayed -= OnDialogueLinePlayed;
        }

        private void OnDialogueLinePlayed(Conversation conversation, DialogueLine dialogueLine)
        {
            if (!_image || dialogueLine == null) return;
            StopAllCoroutines();
            switch(dialogueLine.Portrait)
            {
                case PortraitType.Happy:
                    StartCoroutine(LerpColor(_image.color, happyColor));
                    break;
                case PortraitType.Neutral:
                    StartCoroutine(LerpColor(_image.color, neutralColor));
                    break;
                case PortraitType.Sad:
                    StartCoroutine(LerpColor(_image.color, sadColor));
                    break;
            }
        }

        private IEnumerator LerpColor(Color startColor, Color endColor)
        {
            for (float timer = 0; timer < AnimationSeconds; timer += Time.deltaTime)
            {
                float progress = timer / AnimationSeconds;
                if (_image) _image.color = Color.Lerp(startColor, endColor, progress);
                yield return null;
            }
            if (_image) _image.color = endColor;
        }
    }
}