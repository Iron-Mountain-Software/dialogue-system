using System.Collections.Generic;
using IronMountain.DialogueSystem.Nodes;
using IronMountain.DialogueSystem.Responses;
using UnityEngine;
using UnityEngine.Events;

namespace IronMountain.DialogueSystem.UI.Responses
{
    public class DialogueResponseBlock : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float destructionDelay = 1;
        [SerializeField] private float xPadding;
        [SerializeField] private float yPadding;
        [SerializeField] private float spacing;
        [Space]
        [SerializeField] private Transform rowParent;
        [SerializeField] private GameObject rowPrefab;
        [SerializeField] private DialogueResponseButton dialogueResponsePrefab;
        [Space]
        [SerializeField] private UnityEvent onDestroy;

        private bool _submitted;
        
        private Transform RowParent => rowParent ? rowParent : transform;

        public void Initialize(DialogueResponseBlockNode dialogueResponseBlock, ConversationPlayer conversationUI)
        {
            List<BasicResponse> responses = dialogueResponseBlock.GetResponses(conversationUI);
            responses.Sort((responseX, responseY) => responseY.Row == responseX.Row ? responseX.Column - responseY.Column : responseX.Row - responseY.Row);
            int currentRowIndex = int.MinValue;
            float cumulativeRowHeight = 0f;
            RectTransform currentRow = null;
            foreach (BasicResponse response in responses)
            {
                if (response == null) continue;
                if (!currentRow || response.Row != currentRowIndex)
                {
                    GameObject instantiatedRow = Instantiate(rowPrefab, RowParent);
                    currentRow = instantiatedRow.GetComponent<RectTransform>();
                    currentRow.anchorMin = new Vector2(xPadding, yPadding + (currentRow.GetSiblingIndex() * spacing) + cumulativeRowHeight);
                    currentRow.anchorMax = new Vector2(1 - xPadding, yPadding + (currentRow.GetSiblingIndex() * spacing) + cumulativeRowHeight + response.Style.Height);
                    currentRow.offsetMin = Vector2.zero;
                    currentRow.offsetMax = Vector2.zero;
                    currentRowIndex = response.Row;
                    cumulativeRowHeight += response.Style.Height;
                }
                Instantiate(dialogueResponsePrefab, currentRow).Initialize(this, response);
            }
        }

        public void Submit(BasicResponse response)
        {
            if (_submitted) return;
            response?.Execute();
            _submitted = true;
        }

        public void Destroy()
        {
            onDestroy?.Invoke();
            Destroy(gameObject, destructionDelay);
        }

        private void OnValidate()
        {
            if (!rowParent) rowParent = transform;
        }
    }
}
