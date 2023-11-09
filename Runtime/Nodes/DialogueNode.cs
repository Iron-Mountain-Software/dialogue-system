using IronMountain.DialogueSystem.UI;
using UnityEngine;
using XNode;

namespace IronMountain.DialogueSystem.Nodes
{
    public abstract class DialogueNode : Node
    {
        [SerializeField] [HideInInspector] private string id;

        public abstract string Name { get; }
        public abstract DialogueNode GetNextNode(ConversationPlayer conversationUI);

        public virtual void OnNodeEnter(ConversationPlayer conversationUI) { }

        public virtual void OnNodeExit(ConversationPlayer conversationUI) { }

        public DialogueNode GetNextHaltingNode(ConversationPlayer conversationUI)
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