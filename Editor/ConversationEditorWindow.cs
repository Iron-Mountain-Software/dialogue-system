using IronMountain.DialogueSystem.Editor.Nodes;
using IronMountain.DialogueSystem.Nodes;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngineInternal;
using XNode;
using XNodeEditor;

namespace IronMountain.DialogueSystem.Editor
{
    public class ConversationEditorWindow : NodeEditorWindow
    {
        private static readonly Color SideBarColor = new Color(0f, 0f, 0f, 0.85f);
        private Texture2D _sideBarTexture;
        private GUIStyle _h1;
        private GUIStyle _box;
        
        private Rect _headerSection;
        private Rect _sideBarSection;

        private bool _showDetails;
        private Conversation _conversation;
        private UnityEditor.Editor _selectedConversationEditor;
        private Vector2 _sidebarScroll = Vector2.zero;
        
        private Vector2 _cachedPanOffset;
        private float _cachedZoom;

        [OnOpenAsset(-1)]
        public static bool Open(int instanceID, int line)
        {
            Object asset = EditorUtility.InstanceIDToObject(instanceID);
            if (asset is not Conversation conversation) return false;
            Open(conversation);
            return true;
        }
        
        public static void Open(Conversation conversation)
        {
            if (!conversation) return;
            ConversationEditorWindow window = GetWindow(typeof(ConversationEditorWindow), false, "Conversation Editor", true) as ConversationEditorWindow;
            window.minSize = new Vector2(500, 400);
            window.wantsMouseMove = true;
            window.graph = conversation;
            window._conversation = conversation;
            window.Initialize();
        }

        private void Initialize()
        {
            _h1 = new () 
            {
                alignment = TextAnchor.MiddleLeft,
                fontSize = 20,
                fontStyle = FontStyle.Bold,
                normal = { textColor = Color.white }
            };
            
            _box = new ()
            {
                padding = new RectOffset(10,10,10,10)
            };
            
            _sideBarTexture = new Texture2D(1, 1);
            _sideBarTexture.SetPixel(0,0, SideBarColor);
            _sideBarTexture.Apply();
        }

        private void CalculateLayout()
        {
            _headerSection.x = 0;
            _headerSection.y = 0;
            _headerSection.width = position.width;
            _headerSection.height = 40;
            
            _sideBarSection.x = 0;
            _sideBarSection.y = 40;
            _sideBarSection.width = 350;
            _sideBarSection.height = position.height - 40;
        }

        protected override void OnGUI()
        {
            base.OnGUI();
            _cachedPanOffset = panOffset;
            _cachedZoom = zoom;
            onLateGUI += OnLateGUI;
        }
        
        private void OnLateGUI()
        {
            CalculateLayout();
            GUI.DrawTexture(_headerSection, _sideBarTexture);
            GUILayout.BeginArea(_headerSection, _box);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("i", GUILayout.Width(20)))
            {
                _showDetails = !_showDetails;
            }
            GUILayout.Label(" " + _conversation.name, _h1);
            GUILayout.Label("CREATE:", _h1, GUILayout.Width(90));
            if (GUILayout.Button("Start", GUILayout.Width(45)))
            {
                graphEditor.CreateNode(typeof(DialogueBeginningNode), WindowToGridPosition(new Vector2(position.width / 2f, position.height / 2f)));
            }
            if (GUILayout.Button("End", GUILayout.Width(45)))
            {
                graphEditor.CreateNode(typeof(DialogueEndingNode), WindowToGridPosition(new Vector2(position.width / 2f, position.height / 2f)));
            }
            if (GUILayout.Button("Line", GUILayout.Width(45)))
            {
                AddDialogueLineNodeMenu.Open(graphEditor, WindowToGridPosition(new Vector2(position.width / 2f, position.height / 2f)));
            }
            if (GUILayout.Button("Action", GUILayout.Width(55)))
            {
                AddDialogueActionNodeMenu.Open(graphEditor, WindowToGridPosition(new Vector2(position.width / 2f, position.height / 2f)));
            }
            if (GUILayout.Button("Condition", GUILayout.Width(70)))
            {
                AddDialogueConditionNodeMenu.Open(graphEditor, WindowToGridPosition(new Vector2(position.width / 2f, position.height / 2f)));
            }
            if (GUILayout.Button("Pass", GUILayout.Width(45)))
            {
                graphEditor.CreateNode(typeof(DialoguePassNode), WindowToGridPosition(new Vector2(position.width / 2f, position.height / 2f)));
            }
            GUILayout.EndHorizontal();
            GUILayout.EndArea();

            if (_showDetails) DrawInformation();
        }

        private void DrawInformation()
        {
            GUI.DrawTexture(_sideBarSection, _sideBarTexture);
            GUILayout.BeginArea(_sideBarSection, _box);
            if (_sideBarSection.Contains(Event.current.mousePosition))
            {
                panOffset = _cachedPanOffset;
                zoom = _cachedZoom;
            }
            if (_conversation)
            {
                _sidebarScroll = GUILayout.BeginScrollView(_sidebarScroll);
                UnityEditor.Editor.CreateCachedEditor(_conversation, null, ref _selectedConversationEditor);
                _selectedConversationEditor.OnInspectorGUI();
                GUILayout.EndScrollView();
            }
            else
            {
                GUILayout.Label("Conversation is null.");
                _selectedConversationEditor = null;
            }
            GUILayout.EndArea();
        }
    }
}
