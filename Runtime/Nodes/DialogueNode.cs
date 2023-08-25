using SpellBoundAR.DialogueSystem.UI;
using UnityEngine;
using XNode;

namespace SpellBoundAR.DialogueSystem.Nodes
{
    public abstract class DialogueNode : Node
    {
        [SerializeField] [HideInInspector] private string id;

        public abstract string Name { get; }
        public abstract DialogueNode GetNextNode(ConversationUI conversationUI);

        public virtual void OnNodeEnter(ConversationUI conversationUI) { }

        public virtual void OnNodeExit(ConversationUI conversationUI) { }

        public DialogueNode GetNextHaltingNode(ConversationUI conversationUI)
        {
            DialogueNode node = GetNextNode(conversationUI);
            while (node)
            {
                if (node is DialogueLineNode) return node;
                if (node is DialogueResponseBlockNode) return node;
                node = node.GetNextNode(conversationUI);
            }
            return null;
        }
        
#if UNITY_EDITOR

        protected virtual void OnValidate()
        {
            RefreshName();
        }
        
        [ContextMenu("Refresh Name")]
        private void RefreshName()
        {
            name = Name;
        }
        
        public bool HasWarnings()
        {
            return ExtensionHasWarnings();
        }
        
        public bool HasErrors()
        {
            return !graph || ExtensionHasErrors();
        }

        protected abstract bool ExtensionHasWarnings();
        protected abstract bool ExtensionHasErrors();
        
#endif
    }
}