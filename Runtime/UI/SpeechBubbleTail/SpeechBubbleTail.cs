using SpellBoundAR.DialogueSystem.Speakers;
using SpellBoundAR.MainCameraManagement;
using UnityEngine;
using UnityEngine.UI;

namespace SpellBoundAR.DialogueSystem.UI.SpeechBubbleTail
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(CanvasRenderer))]
    public class SpeechBubbleTail : Graphic
    {
        [Header("Cache")]
        private ConversationPlayer _conversationUI;
        private Transform _anchor;

        protected override void Awake()
        {
            _conversationUI = GetComponentInParent<ConversationPlayer>();
            if (_conversationUI) _conversationUI.OnDefaultSpeakerChanged += RefreshAnchor;
            SpeechBubbleAnchorsManager.OnAnchorsChanged += RefreshAnchor;
        }

        protected override void OnDestroy()
        {
            if (_conversationUI) _conversationUI.OnDefaultSpeakerChanged -= RefreshAnchor;
            SpeechBubbleAnchorsManager.OnAnchorsChanged -= RefreshAnchor;
        }
        
        private void RefreshAnchor()
        {
            ISpeaker speaker = _conversationUI ? _conversationUI.DefaultSpeaker : null;
            _anchor = SpeechBubbleAnchorsManager.GetAnchor(speaker);
        }
        
        private void Update() => SetVerticesDirty();

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();

            if (_anchor == null) { return; }

            Vector3 screenPosition = MainCameraManager.Instance.MainCamera.WorldToScreenPoint(_anchor.position);

            Vector2 pivot = rectTransform.pivot;
            Rect rect = rectTransform.rect;
            float width = rect.width;
            float height = rect.height;
            
            UIVertex vertex = UIVertex.simpleVert;
            vertex.color = color;

            float positionRatio = Mathf.Clamp01(screenPosition.x / Screen.width);

            float middleX = positionRatio * width;
            float leftX = Mathf.Clamp(0, middleX - (positionRatio * 180), width);
            float rightX = Mathf.Clamp(0, middleX + ((1 - positionRatio) * 180), width);

            // Top Left
            vertex.position = new Vector3(leftX - width * pivot.x, height - height * pivot.y);
            vh.AddVert(vertex);

            // Top Right
            vertex.position = new Vector3(rightX - width * pivot.x, height - height * pivot.y);
            vh.AddVert(vertex);

            // Bottom
            vertex.position = new Vector3(middleX - width * pivot.x, 0 - height * pivot.y);
            vh.AddVert(vertex);

            vh.AddTriangle(0, 1, 2);
        }
    }
}