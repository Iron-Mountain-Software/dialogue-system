using SpellBoundAR.DialogueSystem.Nodes;
using UnityEngine;

namespace SpellBoundAR.DialogueSystem.UI
{
    public class UI_DialogueResponseBlockGenerator : MonoBehaviour
    {
        [SerializeField] private GameObject dialogueResponseBlockPrefab;

        private void Awake()
        {
            DialogueResponseBlockNode.OnDialogueResponseBlockEntered += OnDialogueResponseBlockEntered;
        }

        private void OnDestroy()
        {
            DialogueResponseBlockNode.OnDialogueResponseBlockEntered -= OnDialogueResponseBlockEntered;
        }

        private void OnDialogueResponseBlockEntered(DialogueResponseBlockNode dialogueResponseBlock, ConversationUI conversationUI)
        {
            GameObject instantiated = Instantiate(dialogueResponseBlockPrefab, transform);
            UI_DialogueResponseBlock instantiatedDialogueResponseBlock = instantiated.GetComponent<UI_DialogueResponseBlock>();
            instantiatedDialogueResponseBlock.Initialize(dialogueResponseBlock, conversationUI);
        }
    }
}