using UnityEngine;
using UnityEngine.UI;

namespace IronMountain.DialogueSystem.Speakers.UI
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Image))]
    public class SpeakerControllerPortraitImage : MonoBehaviour
    {
        private enum Type
        {
            Portrait,
            FullBody
        }

        [SerializeField] private Type type;
        [SerializeField] private SpeakerPortraitCollection.PortraitType expression =
            SpeakerPortraitCollection.PortraitType.Neutral;

        [Header("Cache")]
        private Image _image;
        private SpeakerController _speakerController;

        private void Awake()
        {
            _image = GetComponent<Image>();
            _speakerController = GetComponentInParent<SpeakerController>();
        }

        private void OnEnable()
        {
            if (_speakerController) _speakerController.OnSpeakerChanged += Refresh;
            Refresh();
        }

        private void OnDisable()
        {
            if (_speakerController) _speakerController.OnSpeakerChanged += Refresh;
        }

        private void Refresh()
        {
            if (!_image) return;
            switch (type)
            {
                case Type.Portrait:
                    _image.sprite = _speakerController && _speakerController.Speaker != null
                        ? _speakerController.Speaker.Portraits.GetPortrait(expression)
                        : null;
                    break;
                case Type.FullBody:
                    _image.sprite = _speakerController && _speakerController.Speaker != null
                        ? _speakerController.Speaker.FullBodyPortraits.GetPortrait(expression)
                        : null;
                    break;
            }
        }
    }
}