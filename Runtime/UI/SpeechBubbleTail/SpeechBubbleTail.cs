using IronMountain.DialogueSystem.Speakers;
using UnityEngine;
using UnityEngine.UI;

namespace IronMountain.DialogueSystem.UI.SpeechBubbleTail
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(CanvasRenderer))]
    public class SpeechBubbleTail : Graphic
    {
        [SerializeField] private ConversationPlayer conversationPlayer;
        [SerializeField] private Transform anchor;
        [SerializeField] private new Camera camera;
        
        protected override void Awake()
        {
            conversationPlayer = GetComponentInParent<ConversationPlayer>();
            if (conversationPlayer) conversationPlayer.OnDefaultSpeakerChanged += RefreshAnchor;
            SpeechBubbleAnchorsManager.OnAnchorsChanged += RefreshAnchor;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            if (!camera) camera = Camera.main;
        }

        protected override void OnDestroy()
        {
            if (conversationPlayer) conversationPlayer.OnDefaultSpeakerChanged -= RefreshAnchor;
            SpeechBubbleAnchorsManager.OnAnchorsChanged -= RefreshAnchor;
        }
        
        private void RefreshAnchor()
        {
            ISpeaker speaker = conversationPlayer ? conversationPlayer.DefaultSpeaker : null;
            anchor = SpeechBubbleAnchorsManager.GetAnchor(speaker);
        }
        
        private void Update() => SetVerticesDirty();

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();

            if (anchor == null || camera == null) return;

            Vector3 screenPosition = camera.WorldToScreenPoint(anchor.position);

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