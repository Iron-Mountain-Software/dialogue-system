using System;
using SpellBoundAR.AssetManagement.Editor;
using UnityEditor;
using UnityEngine;

namespace SpellBoundAR.DialogueSystem.Editor
{
    [CustomEditor(typeof(Database), true)]
    public class DatabaseInspector : SingletonDatabaseInspector
    {
        private static readonly string ExportsDirectory = "Exports";
        private static readonly string ExportsFile = "Dialogue Lines.txt";

        private Database _database;
        private ConversationListEditor _conversationListEditor = null;

        private void OnEnable()
        {
            _database = (Database) target;
            _conversationListEditor = new ConversationListEditor();
            if (_conversationListEditor != null) _conversationListEditor.OnSelectedConversationChanged += OnSelectedConversationChanged;
        }

        private void OnSelectedConversationChanged()
        {
            if (_conversationListEditor != null && _conversationListEditor.SelectedConversation)
            {
                UnityEditor.Selection.activeObject = _conversationListEditor.SelectedConversation;
            }
        }

        private void OnDisable()
        {
            if (_conversationListEditor != null) _conversationListEditor.OnSelectedConversationChanged -= OnSelectedConversationChanged;
        }

        protected override void RebuildLists()
        {
            Utilities.FillWithAssetsOfType(((Database) target).Conversations.list, target);
        }

        protected override void SortLists()
        {
            _database.Conversations.SortList();
        }

        protected override void RebuildDictionaries()
        {
            _database.Conversations.RebuildDictionary();
        }

        public override string ToString()
        {
            return _database.Conversations.ToString("Conversations");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.Space();
            if (GUILayout.Button("Export Dialogue Lines (EN)")) ExportDialogueLines(0);
            if (GUILayout.Button("Export Dialogue Lines (ES)")) ExportDialogueLines(1);
            EditorGUILayout.Space();
            _conversationListEditor.Draw(_database.Conversations.list);
        }

        private void ExportDialogueLines(int language)
        {
            /*
            RebuildLists();
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[language];
            Dictionary<string, List<string>> dialogueLines = new Dictionary<string, List<string>>();
            foreach (Conversation dialogueInteraction in ((Database)target).Conversations.list)
            {
                if (dialogueInteraction.Speaker == null) continue;
                string key = dialogueInteraction.Speaker.Name;
                if (!dialogueLines.ContainsKey(key)) dialogueLines.Add(key, new List<string>());
                foreach (Node node in dialogueInteraction.nodes)
                {
                    switch (node)
                    {
                        case DialogueLineWithAlternatesNode dialogueLineWithAlternatesNode:
                        {
                            string mainText = dialogueLineWithAlternatesNode.Text;
                            if (!dialogueLines[key].Contains(mainText)) dialogueLines[key].Add(mainText);
                            foreach (DialogueLineMainContent content in dialogueLineWithAlternatesNode.AlternateContent)
                            {
                                string alternateText = content.Text;
                                if (!dialogueLines[key].Contains(alternateText)) dialogueLines[key].Add(alternateText);
                            }
                            break;
                        }
                        case DialogueLineNode dialogueLineNode:
                        {
                            string text = dialogueLineNode.Text;
                            if (!dialogueLines[key].Contains(text)) dialogueLines[key].Add(text);
                            break;
                        }
                    }
                }
            }
            foreach (KeyValuePair<string, List<string>> entry in dialogueLines)
            {
                string file = entry.Key + ".txt";
                string data = string.Empty;
                foreach (string text in entry.Value) data += text + "\n";
                SaveSystem.SaveSystem.SaveFile(ExportsDirectory, file, data);
            }
            */
        }
    }
}