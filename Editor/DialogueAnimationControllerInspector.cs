using System;
using SpellBoundAR.DialogueSystem.Animation;
using UnityEditor;
using UnityEngine;

namespace ARISE.DialogueSystem.Editor.Animation
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

            DrawAnimationRow("Nod", "nod", _dialogueAnimationController.Nod);
            DrawAnimationRow("Exclamation", "exclamation", _dialogueAnimationController.Exclamation);
            DrawAnimationRow("Question", "question", _dialogueAnimationController.Question);
            DrawAnimationRow("Hunch", "hunch", _dialogueAnimationController.Hunch);
            
            GUILayout.Space(10);
            
            EditorGUILayout.PropertyField(
                serializedObject.FindProperty("initialStates"), 
                new GUIContent("States that the Animations can be played from")
            );
            
            GUILayout.Label("References");
            EditorGUILayout.PropertyField(serializedObject.FindProperty("animator"));
            
            serializedObject.ApplyModifiedProperties();
        }

        private void DrawAnimationRow(string buttonLabel, string propertyName, string animationName)
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button(buttonLabel, GUILayout.MaxWidth(100)))
            {
                _dialogueAnimationController.PlayAnimation(animationName);
            }
            EditorGUILayout.PropertyField(serializedObject.FindProperty(propertyName), GUIContent.none);
            EditorGUILayout.EndHorizontal();
        }
    }
}
