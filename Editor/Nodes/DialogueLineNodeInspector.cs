using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using IronMountain.DialogueSystem.Nodes;
using IronMountain.DialogueSystem.Speakers;
using UnityEditor;
using UnityEngine;
using XNode;
using XNodeEditor;

namespace IronMountain.DialogueSystem.Editor.Nodes
{
    [CustomNodeEditor(typeof(DialogueLineNode))]
    public class DialogueLineNodeInspector : NodeEditor
    {
        private bool _localize = false;
        private DialogueLineNode _dialogueLineNode;

        public override void OnCreate()
        {
            base.OnCreate();
            _localize = !((DialogueLineNode) target).LocalizedText.IsEmpty;
        }

        public virtual void DrawAdditionalProperties() { }

        public override void OnBodyGUI()
        {
            serializedObject.Update();
            if (!_dialogueLineNode) _dialogueLineNode = (DialogueLineNode) target;

            EditorGUILayout.BeginHorizontal();
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("input"));
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("output"));
            EditorGUILayout.EndHorizontal();

            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("speakerType"));
            if (((DialogueLineNode) target).SpeakerType == SpeakerType.Custom)
            {
                NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("customSpeaker"));
            }
            
            EditorGUILayout.Space(10);
            
            EditorGUILayout.BeginHorizontal();
            EditorGUI.BeginDisabledGroup(!_localize);
            if (GUILayout.Button("Simple")) _localize = false;
            EditorGUI.EndDisabledGroup();
            EditorGUI.BeginDisabledGroup(_localize);
            if (GUILayout.Button("Localized")) _localize = true;
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.EndHorizontal();

            if (_localize)
            {
                NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("text"), new GUIContent("Text"));
                NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("localizedAudio"), new GUIContent("Narration"));
            }
            else
            {
                NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("simpleText"), new GUIContent("Text"));
                
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Narration", GUILayout.Width(55));
                NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("audioClip"), GUIContent.none);
                EditorGUI.BeginDisabledGroup(!_dialogueLineNode.AudioClip);
                if (GUILayout.Button("Rename", GUILayout.Width(60)))
                {
                    string newName = _dialogueLineNode.Text;
                    newName = newName.Replace("/", "");
                    newName = newName.Replace("?", "");
                    newName = newName.Replace("<", "");
                    newName = newName.Replace(">", "");
                    newName = newName.Replace("\\", "");
                    newName = newName.Replace(":", "");
                    newName = newName.Replace("*", "");
                    newName = newName.Replace("|", "");
                    newName = newName.Replace("\"", "");
                    if (string.IsNullOrWhiteSpace(newName)) return;
                    _dialogueLineNode.AudioClip.name = newName;
                    string path = AssetDatabase.GetAssetPath(_dialogueLineNode.AudioClip);
                    AssetDatabase.RenameAsset(path, newName);
                    EditorUtility.SetDirty(_dialogueLineNode.AudioClip);
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                }
                EditorGUI.EndDisabledGroup();
                EditorGUILayout.EndHorizontal();
            }
            
            EditorGUILayout.Space(10);
            
            string[] strArray = new string[12]
            {
                "m_Script",
                "graph",
                "position",
                "ports",
                "input",
                "output",
                "speakerType",
                "customSpeaker",
                "text",
                "localizedAudio",
                "simpleText",
                "audioClip"
            };
            SerializedProperty iterator = serializedObject.GetIterator();
            bool enterChildren = true;
            while (iterator.NextVisible(enterChildren))
            {
                enterChildren = false;
                if (!strArray.Contains(iterator.name))
                    NodeEditorGUILayout.PropertyField(iterator, true, Array.Empty<GUILayoutOption>());
            }
            foreach (NodePort dynamicPort in target.DynamicPorts)
            {
                if (!NodeEditorGUILayout.IsDynamicPortListPort(dynamicPort))
                    NodeEditorGUILayout.PortField(dynamicPort, Array.Empty<GUILayoutOption>());
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
