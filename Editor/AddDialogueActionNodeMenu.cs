using System;
using System.Collections.Generic;
using System.Linq;
using IronMountain.DialogueSystem.Nodes.Actions;
using UnityEditor;
using UnityEngine;
using XNodeEditor;

namespace IronMountain.DialogueSystem.Editor
{
    public static class AddDialogueActionNodeMenu
    {
        private static readonly List<Type> DialogueActionNodeTypes;

        static AddDialogueActionNodeMenu()
        {
            DialogueActionNodeTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => !type.IsAbstract && (type == typeof(DialogueAction) || type.IsSubclassOf(typeof(DialogueAction))))
                .ToList();
        }
        
        public static void Open(NodeGraphEditor graphEditor, Vector2 position)
        {
            if (DialogueActionNodeTypes.Count > 1)
            {
                GenericMenu menu = new GenericMenu();
                foreach (Type derivedType in DialogueActionNodeTypes)
                {
                    menu.AddItem(new GUIContent(
                            "Add " + derivedType.Name),
                        false,
                        () => graphEditor.CreateNode(derivedType, position));
                }
                menu.ShowAsContext();
            }
            else graphEditor.CreateNode(typeof(DialogueAction), position);
        }
    }
}