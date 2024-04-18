using System;
using System.IO;
using IronMountain.DialogueSystem.Speakers;
using UnityEditor;
using UnityEngine;

namespace IronMountain.DialogueSystem.Editor.Windows
{
    public class NewSpeakerWindow : EditorWindow
    {
        private static int _speakerTypeIndex = 0;

        private string _folder = Path.Join("Assets", "Scriptable Objects", "Speakers");
        private string _name = "New Speaker";
        
        public static void Open()
        {
            NewSpeakerWindow window = GetWindow(typeof(NewSpeakerWindow), false, "Create Speaker", true) as NewSpeakerWindow;
            window.minSize = new Vector2(520, 160);
            window.maxSize = new Vector2(520, 160);
            window.wantsMouseMove = true;
        }

        protected void OnGUI()
        {
            EditorGUILayout.Space(10);
            
            EditorGUILayout.BeginHorizontal();
            _folder = EditorGUILayout.TextField("Folder: ", _folder);
            if (GUILayout.Button("Current", GUILayout.Width(60))) _folder = DirectoryUtilities.GetCurrentFolder();
            EditorGUILayout.EndHorizontal();
            
            _name = EditorGUILayout.TextField("Name", _name);

            _speakerTypeIndex = EditorGUILayout.Popup("Type", _speakerTypeIndex, TypeIndex.SpeakerTypeNames);
            
            EditorGUILayout.Space(10);
            
            if (GUILayout.Button("Create Speaker", GUILayout.Height(35))) CreateSpeaker();
        }

        private void CreateSpeaker()
        {
            Type speakerType = TypeIndex.SpeakerTypes[_speakerTypeIndex];
            Speaker speaker = CreateInstance(speakerType) as Speaker;
            if (!speaker) return;
            DirectoryUtilities.CreateFolders(_folder);
            string path = Path.Combine(_folder, _name + ".asset");
            
            AssetDatabase.CreateAsset(speaker, path);
            EditorUtility.SetDirty(speaker);
            AssetDatabase.SaveAssetIfDirty(speaker);
            AssetDatabase.Refresh();

            Close();
        }
    }
}