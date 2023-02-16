using SpellBoundAR.DialogueSystem.Animation;
using UnityEditor;
using UnityEngine;

namespace SpellBoundAR.DialogueSystem.Editor
{
    [CustomEditor(typeof(DialogueAnimationController), true)]
    public class DialogueAnimationControllerInspector : UnityEditor.Editor
    {
        private DialogueAnimationController _dialogueAnimationController;

        private void OnEnable()
        {
            _dialogueAnimationController = (DialogueAnimationController) target;
        }

        public override void OnInspectorGUI()
        {
            GUILayout.Label("Settings");

            GUILayout.Space(5);

            DrawAnimationRow("Nod", "nod", AnimationType.Nod);
            DrawAnimationRow("Exclamation", "exclamation", AnimationType.Exclamation);
            DrawAnimationRow("Question", "question", AnimationType.Question);
            DrawAnimationRow("Hunch", "hunch", AnimationType.Hunch);
            
            GUILayout.Space(10);
            
            EditorGUILayout.PropertyField(
                serializedObject.FindProperty("initialStates"), 
                new GUIContent("States that the Animations can be played from")
            );
            
            GUILayout.Label("References");
            EditorGUILayout.PropertyField(serializedObject.FindProperty("animator"));
            
            serializedObject.ApplyModifiedProperties();
        }

        private void DrawAnimationRow(string buttonLabel, string propertyName, AnimationType animationType)
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button(buttonLabel, GUILayout.MaxWidth(100)))
            {
                _dialogueAnimationController.PlayAnimation(animationType);
            }
            EditorGUILayout.PropertyField(serializedObject.FindProperty(propertyName), GUIContent.none);
            EditorGUILayout.EndHorizontal();
        }
    }
}
