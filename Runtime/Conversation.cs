using SpellBoundAR.AssetManagement;
using SpellBoundAR.Conditions;
using SpellBoundAR.DialogueSystem.Nodes;
using SpellBoundAR.ResourceUtilities;
using UnityEditor;
using UnityEditor.Localization;
using UnityEngine;
using UnityEngine.Localization;
using XNode;

namespace SpellBoundAR.DialogueSystem
{
    public abstract class Conversation : NodeGraph, IIdentifiable
    {
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
        [SerializeField] private LocalizedString invokingLine;
        [SerializeField] private ResourceSprite invokingIcon;
        [SerializeField] private bool alertInConversationMenu;
        
        // Preview
        [SerializeField] private ConversationPreviewType previewType;
        [SerializeField] private LocalizedString previewText;
        
        // Playback
        [SerializeField] private Condition condition;
        [SerializeField] private BehaviorWhenQueued behaviorWhenQueued;
        [SerializeField] private bool looping;

        // States
        [SerializeField] private int playthroughs;

        public string ID
        {
            get => id;
            set => id = value;
        }

        public string Name => name;
        public abstract IConversationEntity Entity { get; }
        public bool PrioritizeOverDefault => prioritizeOverDefault;
        public int Priority => priority;

        public string InvokingLine
        {
            get
            {
                if (Application.isPlaying)
                    return invokingLine.IsEmpty ? string.Empty : invokingLine.GetLocalizedString();
#if UNITY_EDITOR
                if (invokingLine.IsEmpty || string.IsNullOrEmpty(invokingLine.TableReference)) return string.Empty;
                var collection = LocalizationEditorSettings.GetStringTableCollection(invokingLine.TableReference);
                var entry = collection.SharedData.GetEntryFromReference(invokingLine.TableEntryReference);
                return entry != null ? entry.Key : string.Empty;
#else
				return string.Empty;
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
        
        public virtual int Playthroughs
        {
            get => playthroughs;
            set => playthroughs = value;
        }
        
        private void OnEnable()
        {
            if (condition) condition.OnConditionStateChanged += OnConditionStateChanged;
            ConversationsManager.AllConversations.Add(this);
            OnConditionStateChanged();
        }

        private void OnDisable()
        {
            if (condition) condition.OnConditionStateChanged -= OnConditionStateChanged;
            ConversationsManager.AllConversations.Remove(this);
        }

        private void OnConditionStateChanged()
        {
            if (!condition) return;
            if (condition.Evaluate())
            {
                ConversationsManager.RegisterActiveConversation(this);
                if (Playthroughs > 0) return;
                if (behaviorWhenQueued == BehaviorWhenQueued.Played)
                {
                    ConversationManager.EnqueueConversation(this);
                }
            }
            else
            {
                ConversationsManager.UnregisterActiveConversation(this);
            }
        }

        public virtual void OnConversationStarted() { }

#if UNITY_EDITOR

        public virtual void Reset()
        {
            GenerateNewID();
        }

        [ContextMenu("Generate New ID")]
        public void GenerateNewID()
        {
            ID = GUID.Generate().ToString();
        }

        [ContextMenu("Test For Warnings")]
        public bool HasWarnings()
        {
            foreach (var node in nodes)
                if (node is DialogueNode dialogueNode && dialogueNode.HasWarnings())
                    return true;
            return false;
        }

        public bool GraphHasErrors()
        {
            foreach (var node in nodes)
                if (node is DialogueNode dialogueNode && dialogueNode.HasErrors())
                    return true;
            return false;
        }

        [ContextMenu("Log Graph Errors")]
        public void LogGraphErrors()
        {
            foreach (var node in nodes)
                if (node is DialogueNode dialogueNode && dialogueNode.HasErrors())
                    Debug.Log(dialogueNode.Name, dialogueNode);
        }

        public bool PreviewHasErrors => previewType != ConversationPreviewType.None &&
                                        (previewText.IsEmpty || string.IsNullOrEmpty(previewText.TableReference));

        public bool InvokerHasErrors => !prioritizeOverDefault &&
                                        (!invokingIcon || invokingLine.IsEmpty ||
                                         string.IsNullOrEmpty(invokingLine.TableReference));

        public bool ConditionHasErrors => !condition || condition.HasErrors();

        [ContextMenu("Test For Errors")]
        public bool HasErrors()
        {
            return string.IsNullOrWhiteSpace(id)
                   || ConditionHasErrors
                   || PreviewHasErrors
                   || InvokerHasErrors
                   || GraphHasErrors();
        }

#endif
    }
}