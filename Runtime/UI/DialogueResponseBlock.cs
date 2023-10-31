using System.Collections.Generic;
using SpellBoundAR.DialogueSystem.Nodes;
using SpellBoundAR.DialogueSystem.Nodes.ResponseGenerators;
using SpellBoundAR.DialogueSystem.Responses;
using UnityEngine;
using UnityEngine.Events;

namespace SpellBoundAR.DialogueSystem.UI
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
        [SerializeField] private GameObject dialogueResponsePrefab;
        [SerializeField] private GameObject dialogueResponseWithIconPrefab;
        [SerializeField] private UnityEvent onDestroy;
        
        private Transform RowParent => rowParent ? rowParent : transform;

        public void Initialize(DialogueResponseBlockNode dialogueResponseBlock, ConversationPlayer conversationUI)
        {
            List<ResponseGenerator> responseGenerators = dialogueResponseBlock.GetResponseGenerators();
            List<BasicResponse> dialogueResponses = new List<BasicResponse>();
            foreach (ResponseGenerator responseGenerator in responseGenerators)
            {
                dialogueResponses.AddRange(responseGenerator.GetDialogueResponses(conversationUI));
            }
            dialogueResponses.Sort((responseX, responseY) => responseY.Row == responseX.Row ? responseX.Column - responseY.Column : responseX.Row - responseY.Row);
            int currentRowIndex = int.MinValue;
            float cumulativeRowHeight = 0f;
            RectTransform currentRow = null;
            foreach (BasicResponse dialogueResponse in dialogueResponses)
            {
                if (!currentRow || dialogueResponse.Row != currentRowIndex)
                {
                    GameObject instantiatedRow = Instantiate(rowPrefab, RowParent);
                    currentRow = instantiatedRow.GetComponent<RectTransform>();
                    currentRow.anchorMin = new Vector2(xPadding, yPadding + (currentRow.GetSiblingIndex() * spacing) + cumulativeRowHeight);
                    currentRow.anchorMax = new Vector2(1 - xPadding, yPadding + (currentRow.GetSiblingIndex() * spacing) + cumulativeRowHeight + dialogueResponse.Style.Height);
                    currentRow.offsetMin = Vector2.zero;
                    currentRow.offsetMax = Vector2.zero;
                    currentRowIndex = dialogueResponse.Row;
                    cumulativeRowHeight += dialogueResponse.Style.Height;
                }
                GameObject instantiated = Instantiate(dialogueResponse.Icon ? dialogueResponseWithIconPrefab : dialogueResponsePrefab, currentRow);
                DialogueResponseButton responseButton = instantiated.GetComponent<DialogueResponseButton>();
                responseButton.Initialize(dialogueResponse, conversationUI);
            }
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
