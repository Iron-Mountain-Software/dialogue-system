using IronMountain.DialogueSystem.Narration;
using UnityEditor;

namespace IronMountain.DialogueSystem.Editor.Narration
{
    [CustomEditor(typeof(DialogueNarration))]
    public class DialogueNarrationInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("type"));
            if (serializedObject.FindProperty("type").enumValueFlag == 1)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("speaker"));
            }
            EditorGUILayout.PropertyField(serializedObject.FindProperty("audioSource"));
            serializedObject.ApplyModifiedProperties();
        }
    }
}