using System;
using System.Collections.Generic;
using System.Linq;
using IronMountain.DialogueSystem.Nodes.Conditions;
using UnityEditor;
using UnityEngine;
using XNodeEditor;

namespace IronMountain.DialogueSystem.Editor
{
    public static class AddDialogueConditionNodeMenu
    {
        private static readonly List<Type> DialogueConditionNodeTypes;

        static AddDialogueConditionNodeMenu()
        {
            DialogueConditionNodeTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => !type.IsAbstract && (type == typeof(Condition) || type.IsSubclassOf(typeof(Condition))))
                .ToList();
        }
        
        public static void Open(NodeGraphEditor graphEditor, Vector2 position)
        {
            if (DialogueConditionNodeTypes.Count <= 0) return;
            GenericMenu menu = new GenericMenu();
            foreach (Type derivedType in DialogueConditionNodeTypes)
            {
                menu.AddItem(new GUIContent(
                        "Add " + derivedType.Name),
                    false,
                    () => graphEditor.CreateNode(derivedType, position));
            }
            menu.ShowAsContext();
        }
    }
}