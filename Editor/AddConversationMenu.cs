using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace IronMountain.DialogueSystem.Editor
{
    public static class AddConversationMenu
    {
        private static readonly List<Type> ConversationTypes;

        static AddConversationMenu()
        {
            ConversationTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => !type.IsAbstract && (type == typeof(Conversation) || type.IsSubclassOf(typeof(Conversation))))
                .ToList();
        }
        
        public static void Open()
        {
            if (ConversationTypes.Count > 1)
            {
                GenericMenu menu = new GenericMenu();
                foreach (Type derivedType in ConversationTypes)
                {
                    menu.AddItem(new GUIContent(
                            "Add " + derivedType.Name),
                            false,
                            () => Save(ScriptableObject.CreateInstance(derivedType) as Conversation, "New " + derivedType.Name));
                }
                menu.ShowAsContext();
            }
            else Save(ScriptableObject.CreateInstance(typeof(Conversation)) as Conversation, "New Conversation");
        }

        private static void Save(Conversation conversation, string name)
        {
            string scriptableObjectsFolder = Path.Combine("Assets", "Scriptable Objects");
            string conversationsFolder = Path.Combine(scriptableObjectsFolder, "Conversations");
            if (!AssetDatabase.IsValidFolder(scriptableObjectsFolder)) AssetDatabase.CreateFolder("Assets", "Scriptable Objects");
            if (!AssetDatabase.IsValidFolder(conversationsFolder)) AssetDatabase.CreateFolder(scriptableObjectsFolder, "Conversations");
            string path = Path.Combine(conversationsFolder, name + ".asset");
            AssetDatabase.CreateAsset(conversation, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
            UnityEditor.Selection.activeObject = conversation;
        }
    }
}