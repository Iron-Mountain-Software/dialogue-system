using System;
using IronMountain.DialogueSystem.Responses;
using UnityEngine;
using UnityEngine.UI;

namespace IronMountain.DialogueSystem.UI.Responses
{
    public class DialogueResponseButton : MonoBehaviour
    {
        public event Action OnResponseChanged;

        [Header("References")]
        [SerializeField] private Button button;
        [SerializeField] private Image buttonImagePrimary;
        [SerializeField] private Image buttonImageSecondary;

        [Header("Cache")]
        private DialogueResponseBlock _responseBlock;
        private BasicResponse _response;

        public BasicResponse Response
        {
            get => _response;
            private set
            {
                if (_response == value) return;
                _response = value;
                OnResponseChanged?.Invoke();
            }
        }

        private void Awake() => OnValidate();

        private void OnValidate()
        {
            if (!button) button = GetComponent<Button>();
        }

        private void OnEnable()
        {
            if (button) button.onClick.AddListener(ExecuteResponse);
        }

        private void OnDisable()
        {
            if (button) button.onClick.RemoveListener(ExecuteResponse);
        }

        public virtual void Initialize(DialogueResponseBlock responseBlock, BasicResponse response)
        {
            _responseBlock = responseBlock;
            Response = response;
            if (buttonImagePrimary) buttonImagePrimary.color = _response.Style.ButtonColorPrimary;
            if (buttonImageSecondary) buttonImageSecondary.color = _response.Style.ButtonColorSecondary;
        }

        private void ExecuteResponse()
        {
            if (_responseBlock) _responseBlock.Submit(Response);
        }
    }
}