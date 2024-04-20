using System.Collections.Generic;
using IronMountain.DialogueSystem.Nodes;
using IronMountain.DialogueSystem.Responses;
using UnityEngine;
using UnityEngine.Events;

namespace IronMountain.DialogueSystem.UI.Responses
{
    public class DialogueResponseBlock : MonoBehaviour
    {
        [SerializeField] protected DialogueResponseButton responseButtonPrefab;
        [SerializeField] protected Transform parent;
        [SerializeField] private float destructionDelay = 1;
        [Space]
        [SerializeField] protected UnityEvent onDestroy;
        
        private bool _submitted;

        private void OnValidate()
        {
            if (!parent) parent = transform;
        }
        
        public virtual DialogueResponseBlock Initialize(DialogueResponseBlockNode dialogueResponseBlock, ConversationPlayer conversationUI)
        {
            if (!responseButtonPrefab || !parent) return this;
            List<BasicResponse> responses = dialogueResponseBlock.GetResponses(conversationUI);
            responses.Sort((responseX, responseY) => responseY.Row == responseX.Row
                ? responseX.Column - responseY.Column
                : responseX.Row - responseY.Row);
            foreach (BasicResponse response in responses)
            {
                if (response == null) continue;
                Instantiate(responseButtonPrefab, parent).Initialize(this, response);
            }
            return this;
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
    }
}
