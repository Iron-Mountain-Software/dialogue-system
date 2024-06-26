using System;
using System.Collections.Generic;
using System.IO;
using IronMountain.Conditions;
using IronMountain.DialogueSystem.Nodes;
using IronMountain.ResourceUtilities;
using IronMountain.SaveSystem;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Serialization;
using XNode;
using Object = UnityEngine.Object;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Localization;
#endif

namespace IronMountain.DialogueSystem
{
    public class Conversation : NodeGraph
    {
        public static event Action<Conversation> OnAnyIsActiveChanged;
        public static event Action<Conversation> OnAnyPlaythroughsChanged;
        
        public event Action OnIsActiveChanged;
        public event Action OnPlaythroughsChanged;

        public enum BehaviorWhenQueued
        {
            None,
            Played
        }
        
        // About
        [SerializeField] private string id;
        
        // Priority
        [SerializeField] private bool prioritizeOverDefault = true;
        [SerializeField] private int priority;
        
        // Invocation
        [SerializeField] private string defaultInvokingLine;
        [SerializeField] [FormerlySerializedAs("invokingLine")] private LocalizedString localizedInvokingLine;
        [SerializeField] private ResourceSprite invokingIcon;
        [SerializeField] private bool alertInConversationMenu;
        
        // Preview
        [SerializeField] private ConversationPreviewType previewType;
        [SerializeField] private LocalizedString previewText;
        
        // Playback
        [SerializeField] private Condition condition;
        [SerializeField] private BehaviorWhenQueued behaviorWhenQueued;
        [SerializeField] private bool looping;

        [Header("Cache")]
        private bool _isActive;
        private SavedInt _playthroughs;
        
        public string ID
        {
            get => id;
            set => id = value;
        }

        public string Name => name;
        public bool PrioritizeOverDefault => prioritizeOverDefault;
        public int Priority => priority;

        public string DefaultInvokingLine
        {
            get => defaultInvokingLine;
            set => defaultInvokingLine = value;
        }

        public string InvokingLine
        {
            get
            {
                if (Application.isPlaying)
                    return localizedInvokingLine.IsEmpty ? defaultInvokingLine : localizedInvokingLine.GetLocalizedString();
#if UNITY_EDITOR
                if (localizedInvokingLine.IsEmpty || string.IsNullOrEmpty(localizedInvokingLine.TableReference)) return defaultInvokingLine;
                var collection = LocalizationEditorSettings.GetStringTableCollection(localizedInvokingLine.TableReference);
                var entry = collection.SharedData.GetEntryFromReference(localizedInvokingLine.TableEntryReference);
                return entry != null ? entry.Key : defaultInvokingLine;
#else
				return defaultInvokingLine;
#endif
            }
        }

        public Sprite InvokingIcon => invokingIcon ? invokingIcon.Asset : null;
        public bool AlertInConversationMenu => alertInConversationMenu;
        public ConversationPreviewType PreviewType => previewType;

        public string PreviewText
        {
            get
            {
                if (Application.isPlaying) return previewText.IsEmpty ? string.Empty : previewText.GetLocalizedString();
#if UNITY_EDITOR
                if (previewText.IsEmpty || string.IsNullOrEmpty(previewText.TableReference)) return string.Empty;
                var collection = LocalizationEditorSettings.GetStringTableCollection(previewText.TableReference);
                var entry = collection.SharedData.GetEntryFromReference(previewText.TableEntryReference);
                return entry != null ? entry.Key : string.Empty;
#else
				return string.Empty;
#endif
            }
        }

        public Condition Condition
        {
            get => condition;
            set => condition = value;
        }

        public BehaviorWhenQueued BehaviorWhenEnqueued => behaviorWhenQueued;
        public bool Looping => looping;

        public bool IsActive
        {
            get => _isActive;
            private set
            {
                if (_isActive == value) return;
                _isActive = value;
                OnIsActiveChanged?.Invoke();
                OnAnyIsActiveChanged?.Invoke(this);
            }
        }

        public int Playthroughs
        {
            get => _playthroughs.Value;
            set
            {
                if (_playthroughs.Value == value) return;
                _playthroughs.Value = value;
            }
        }

        protected virtual string Directory => Path.Combine("Conversations", ID);

        protected virtual void OnEnable()
        {
            LoadSavedData();
            BroadcastSavedData();
            if (condition) condition.OnConditionStateChanged += RefreshActiveState;
            OnPlaythroughsChanged += RefreshActiveState;
            ConversationsManager.Register(this);
            RefreshActiveState();
        }

        protected virtual void OnDisable()
        {
            if (condition) condition.OnConditionStateChanged -= RefreshActiveState;
            OnPlaythroughsChanged -= RefreshActiveState;
            ConversationsManager.Unregister(this);
        }

        protected void LoadSavedData()
        {
            string directory = Directory;
            _playthroughs = new SavedInt(directory, "playthroughs.txt", 0, () =>
            {
                OnPlaythroughsChanged?.Invoke();
                OnAnyPlaythroughsChanged?.Invoke(this);
            });
        }

        protected void BroadcastSavedData()
        {
            OnPlaythroughsChanged?.Invoke();
            OnAnyPlaythroughsChanged?.Invoke(this);
        }

        public void RefreshActiveState()
        {
            bool conditionMet = condition && condition.Evaluate();
            bool alreadyPlayed = !looping && Playthroughs > 0;
            IsActive = conditionMet && !alreadyPlayed;
        }

        public virtual void OnConversationStarted() { }

#if UNITY_EDITOR

        public void OnValidate()
        {
            RefreshWarnings();
            RefreshErrors();
        }

        public readonly List<string> Warnings = new ();
        public readonly List<string> Errors = new ();

        public bool HasWarnings() => Warnings is {Count: > 0};
        public bool HasErrors() => Errors is {Count: > 0};
        
        public bool PreviewHasErrors => previewType != ConversationPreviewType.None &&
                                        (previewText.IsEmpty || string.IsNullOrEmpty(previewText.TableReference));

        public virtual void Reset()
        {
            GenerateNewID();
        }

        [ContextMenu("Generate New ID")]
        public void GenerateNewID()
        {
            ID = GUID.Generate().ToString();
        }

        [ContextMenu("Refresh Warnings")]
        public virtual void RefreshWarnings()
        {
            Warnings.Clear();
            foreach (var node in nodes)
            {
                if (node is not DialogueNode dialogueNode) continue;
                dialogueNode.RefreshWarnings();
                Warnings.AddRange(dialogueNode.Warnings);
            }
        }

        [ContextMenu("Refresh Errors")]
        public virtual void RefreshErrors()
        {
            Errors.Clear();
            if (string.IsNullOrWhiteSpace(id)) Errors.Add("No ID.");
            if (!condition) Errors.Add("Conversation is missing a condition.");
            if (condition && condition.HasErrors()) Errors.Add("Conversation condition has errors.");
            if (PreviewHasErrors) Errors.Add("Preview has errors.");
            foreach (var node in nodes)
            {
                if (node is not DialogueNode dialogueNode) continue;
                dialogueNode.RefreshErrors();
                Errors.AddRange(dialogueNode.Errors);
            }
        }

        [MenuItem("Iron Mountain/Align Selected Nodes %#-")]
        public static void AlignSelectedNodes()
        {
            float averageY = 0;
            int nodeCount = 0;
            foreach (Object selection in UnityEditor.Selection.objects)
            {
                if (selection is not Node node) continue;
                averageY += node.position.y;
                nodeCount++;
            }
            if (nodeCount <= 0) return;
            averageY /= nodeCount;
            foreach (Object selection in UnityEditor.Selection.objects)
            {
                if (selection is not Node node) continue;
                node.position.y = averageY;
            }
            UnityEditor.Selection.activeObject = null;
        }

#endif
    }
}