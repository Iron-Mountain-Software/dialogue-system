using System.Collections.Generic;
using IronMountain.DialogueSystem.UI;
using UnityEngine;
using XNode;

namespace IronMountain.DialogueSystem.Nodes
{
    public abstract class DialogueNode : Node
    {
        [SerializeField] [HideInInspector] private string id;
        
        public abstract string Name { get; }
        public abstract DialogueNode GetNextNode(ConversationPlayer conversationPlayer);

        public abstract void OnNodeEnter(ConversationPlayer conversationPlayer);
        public abstract void OnNodeUpdate(ConversationPlayer conversationPlayer);
        public abstract void OnNodeExit(ConversationPlayer conversationPlayer);

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

        public override void OnCreateConnection(NodePort @from, NodePort to) => OnValidate();
        public override void OnRemoveConnection(NodePort port) => OnValidate();

        public readonly List<string> Warnings = new ();
        public readonly List<string> Errors = new ();

        public bool HasWarnings() => Warnings is {Count: > 0};
        public bool HasErrors() => Errors is {Count: > 0};
        
        protected virtual void OnValidate()
        {
            RefreshName();
            RefreshWarnings();
            RefreshErrors();
            if (graph is Conversation conversation) conversation.OnValidate();
        }
        
        [ContextMenu("Refresh Name")]
        private void RefreshName()
        {
            name = Name;
        }

        [ContextMenu("Refresh Warnings")]
        public virtual void RefreshWarnings()
        {
            Warnings.Clear();
        }
        
        [ContextMenu("Refresh Errors")]
        public virtual void RefreshErrors()
        {
            Errors.Clear();
            if (!graph) Errors.Add("Null Graph");
        }

#endif
    }
}