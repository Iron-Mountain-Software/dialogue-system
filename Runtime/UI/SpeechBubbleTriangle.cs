using SpellBoundAR.MainCameraManagement;
using UnityEngine;
using UnityEngine.UI;

namespace SpellBoundAR.DialogueSystem.UI
{
    public class SpeechBubbleTriangle : Graphic 
    {
        [Header("Cache")]
        private Transform _target;

        protected override void Awake() => ConversationUI.OnDialogueInteractionStarted += OnDialogueInteractionStarted;
        protected override void OnDestroy() => ConversationUI.OnDialogueInteractionStarted -= OnDialogueInteractionStarted;
        private void Update() => SetVerticesDirty();

        private void OnDialogueInteractionStarted(Conversation conversation)
        {
            //Character character = CharactersManager.Characters.Find(test => test.Instance == conversation.Character);
            //_target = character ? character.transform : null; 
        }

        protected override void OnPopulateMesh(VertexHelper vh) {
            vh.Clear();

            if (_target == null) { return; }

            Vector3 screenPosition = MainCameraManager.Instance.MainCamera.WorldToScreenPoint(_target.position);

            float width = rectTransform.rect.width;
            float height = rectTransform.rect.height;

            float pivotX = rectTransform.pivot.x;
            float pivotY = rectTransform.pivot.x;

            UIVertex vertex = UIVertex.simpleVert;
            vertex.color = color;

            float positionRatio = Mathf.Clamp01(screenPosition.x / Screen.width);

            float middleX = positionRatio * width;
            float leftX = Mathf.Clamp(0, middleX - (positionRatio * 180), width);
            float rightX = Mathf.Clamp(0, middleX + ((1 - positionRatio) * 180), width);

            // Top Left
            vertex.position = new Vector3(leftX - (width * pivotX), height - (height * pivotY));
            vh.AddVert(vertex);

            // Top Right
            vertex.position = new Vector3(rightX - (width * pivotX), height - (height * pivotY));
            vh.AddVert(vertex);

            // Bottom
            vertex.position = new Vector3(middleX - (width * pivotX), 0 - (height * pivotY));
            vh.AddVert(vertex);

            vh.AddTriangle(0, 1, 2);
        }
    }
}