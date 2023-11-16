using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using IronMountain.DialogueSystem.Nodes;
using UnityEditor;
using UnityEngine;
using XNodeEditor;

namespace IronMountain.DialogueSystem.Editor
{
    public static class AddDialogueLineNodeMenu
    {
        private static readonly List<Type> DialogueLineNodeTypes;

        static AddDialogueLineNodeMenu()
        {
            DialogueLineNodeTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => !type.IsAbstract && (type == typeof(DialogueLineNode) || type.IsSubclassOf(typeof(DialogueLineNode))))
                .ToList();
        }
        
        public static void Open(NodeGraphEditor graphEditor, Vector2 position)
        {
            if (DialogueLineNodeTypes.Count > 1)
            {
                GenericMenu menu = new GenericMenu();
                foreach (Type derivedType in DialogueLineNodeTypes)
                {
                    menu.AddItem(new GUIContent(
                            "Add " + derivedType.Name),
                            false,
                            () => graphEditor.CreateNode(derivedType, position));
                }
                menu.ShowAsContext();
            }
            else graphEditor.CreateNode(typeof(DialogueLineNode), position);
        }
    }
}