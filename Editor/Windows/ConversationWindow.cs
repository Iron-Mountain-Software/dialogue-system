using System;
using System.Collections.Generic;
using IronMountain.DialogueSystem.Nodes;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using XNodeEditor;
using Object = UnityEngine.Object;

namespace IronMountain.DialogueSystem.Editor.Windows
{
    public class ConversationEditor : NodeEditorWindow
    {
        public static ConversationEditor Current { get; private set; }

        private static readonly Vector2 MinSize = new (500, 400);
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

        public Vector2 GridCenterPosition => WindowToGridPosition(new Vector2(position.width / 2f, position.height / 2f));
        
        [OnOpenAsset(-1)]
        public static bool Open(int instanceID, int line)
        {
            Object asset = EditorUtility.InstanceIDToObject(instanceID);
            if (asset is not Conversation conversation) return false;
            Open(conversation);
            return true;
        }
        
        public static ConversationEditor Open()
        {
            ConversationEditor window = GetWindow<ConversationEditor>(
                "Conversation", true, 
                typeof(ConversationIndex));
            window.minSize = MinSize;
            window.wantsMouseMove = true;
            window.InitializeStyles();
            return window;
        }
        
        public static ConversationEditor Open(Conversation conversation)
        {
            ConversationEditor window = Open();
            window.graph = conversation;
            window._conversation = conversation;
            window.CenterNodes();
            return window;
        }

        private void CenterNodes()
        {
            if (!graph || graph.nodes.Count == 0) return;
            Vector2 center = Vector2.zero;
            foreach (var node in graph.nodes)
            {
                center += node ? node.position : Vector2.zero;
            }
            center /= graph.nodes.Count;
            foreach (var node in graph.nodes)
            {
                if (!node) continue;
                node.position -= center;
            }
        }

        private void InitializeStyles()
        {
            _h1 ??= new GUIStyle
            {
                alignment = TextAnchor.MiddleLeft,
                fontSize = 20,
                fontStyle = FontStyle.Bold,
                normal = {textColor = Color.white}
            };
            
            _box ??= new GUIStyle
            {
                padding = new RectOffset(10,10,10,10)
            };

            if (!_sideBarTexture)
            {
                _sideBarTexture = new Texture2D(1, 1);
                _sideBarTexture.SetPixel(0,0, SideBarColor);
                _sideBarTexture.Apply();
            }
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
            Current = this;
            _cachedPanOffset = panOffset;
            _cachedZoom = zoom;
            onLateGUI += OnLateGUI;
        }
        
        private void OnLateGUI()
        {
            CalculateLayout();
            InitializeStyles();
            GUI.DrawTexture(_headerSection, _sideBarTexture);
            GUILayout.BeginArea(_headerSection, _box);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("i", GUILayout.Width(20)))
            {
                _showDetails = !_showDetails;
            }
            GUILayout.Label(" " + (_conversation ? _conversation.Name : string.Empty), _h1);
            GUILayout.Label("CREATE:", _h1, GUILayout.Width(90));
            if (GUILayout.Button("Start", GUILayout.Width(40)))
            {
                graphEditor.CreateNode(typeof(DialogueBeginningNode), GridCenterPosition);
            }
            if (GUILayout.Button("End", GUILayout.Width(35)))
            {
                graphEditor.CreateNode(typeof(DialogueEndingNode), GridCenterPosition);
            }
            if (GUILayout.Button("Lines", GUILayout.Width(45)))
            {
                DialogueLinesCreatorWindow.Open(null);
            }
            if (GUILayout.Button("Responses", GUILayout.Width(75)))
            {
                graphEditor.CreateNode(typeof(DialogueResponseBlockNode), GridCenterPosition);
            }
            if (GUILayout.Button("Action", GUILayout.Width(56)))
            {
                RenderCreateMenu(TypeIndex.DialogueActionNodeTypes);
            }
            if (GUILayout.Button("Condition", GUILayout.Width(70)))
            {
                RenderCreateMenu(TypeIndex.DialogueConditionNodeTypes);
            }
            if (GUILayout.Button("Randomizer", GUILayout.Width(83)))
            {
                graphEditor.CreateNode(typeof(DialogueRandomizerNode), GridCenterPosition);
            }
            if (GUILayout.Button("Pass", GUILayout.Width(42)))
            {
                graphEditor.CreateNode(typeof(DialoguePassNode), GridCenterPosition);
            }
            GUILayout.EndHorizontal();
            GUILayout.EndArea();

            if (_showDetails) DrawInformation();
        }
        
        private void RenderCreateMenu(List<Type> types)
        {
            if (types == null || types.Count == 0) return;
            GenericMenu menu = new GenericMenu();
            foreach (Type derivedType in types)
            {
                menu.AddItem(new GUIContent(
                        "Add " + derivedType.Name),
                    false,
                    () => graphEditor.CreateNode(derivedType, GridCenterPosition));
            }
            menu.ShowAsContext();
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
