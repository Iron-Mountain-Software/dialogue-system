using System.Collections.Generic;
using IronMountain.DialogueSystem.Nodes;
using IronMountain.DialogueSystem.Responses;
using UnityEngine;

namespace IronMountain.DialogueSystem.UI.Responses
{
    public class DialogueResponseGridBlock : DialogueResponseBlock
    {
        [SerializeField] protected float xPadding;
        [SerializeField] protected float yPadding;
        [SerializeField] protected float spacing;
        [SerializeField] protected GameObject rowPrefab;

        public override DialogueResponseBlock Initialize(DialogueResponseBlockNode dialogueResponseBlock, ConversationPlayer conversationUI)
        {
            if (!responseButtonPrefab || !parent) return this;
            List<BasicResponse> responses = dialogueResponseBlock.GetResponses(conversationUI);
            responses.Sort((responseX, responseY) => responseY.Row == responseX.Row
                ? responseX.Column - responseY.Column
                : responseX.Row - responseY.Row);
            int currentRowIndex = int.MinValue;
            float cumulativeRowHeight = 0f;
            RectTransform currentRow = null;
            foreach (BasicResponse response in responses)
            {
                if (response == null) continue;
                if (!currentRow || response.Row != currentRowIndex)
                {
                    GameObject instantiatedRow = Instantiate(rowPrefab, parent);
                    currentRow = instantiatedRow.GetComponent<RectTransform>();
                    currentRow.anchorMin = new Vector2(xPadding,
                        yPadding + (currentRow.GetSiblingIndex() * spacing) + cumulativeRowHeight);
                    currentRow.anchorMax = new Vector2(1 - xPadding,
                        yPadding + (currentRow.GetSiblingIndex() * spacing) + cumulativeRowHeight +
                        response.Style.Height);
                    currentRow.offsetMin = Vector2.zero;
                    currentRow.offsetMax = Vector2.zero;
                    currentRowIndex = response.Row;
                    cumulativeRowHeight += response.Style.Height;
                }

                Instantiate(responseButtonPrefab, currentRow).Initialize(this, response);
            }
            return this;
        }
    }
}
