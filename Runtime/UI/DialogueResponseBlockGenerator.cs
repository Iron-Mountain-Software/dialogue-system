using System;
using SpellBoundAR.DialogueSystem.Nodes;
using UnityEngine;

namespace SpellBoundAR.DialogueSystem.UI
{
    public class DialogueResponseBlockGenerator : MonoBehaviour
    {
        [SerializeField] private Transform dialogueResponseBlockParent;
        [SerializeField] private GameObject dialogueResponseBlockPrefab;

        private Transform DialogueResponseBlockParent => dialogueResponseBlockParent
            ? dialogueResponseBlockParent
            : transform;
        
        private void Awake() => DialogueResponseBlockNode.OnDialogueResponseBlockEntered += OnDialogueResponseBlockEntered;
        private void OnDestroy() => DialogueResponseBlockNode.OnDialogueResponseBlockEntered -= OnDialogueResponseBlockEntered;

        private void OnDialogueResponseBlockEntered(DialogueResponseBlockNode dialogueResponseBlock, ConversationPlayer conversationUI)
        {
            if (!dialogueResponseBlockPrefab) return;
            GameObject instantiated = Instantiate(dialogueResponseBlockPrefab, DialogueResponseBlockParent);
            DialogueResponseBlock instantiatedDialogueResponseBlock = instantiated.GetComponent<DialogueResponseBlock>();
            instantiatedDialogueResponseBlock.Initialize(dialogueResponseBlock, conversationUI);
        }
        
        private void OnValidate()
        {
            if (!dialogueResponseBlockParent) dialogueResponseBlockParent = transform;
        }
    }
}