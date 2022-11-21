using System.Collections.Generic;
using SpellBoundAR.AssetManagement;
using SpellBoundAR.Characters;
using UnityEngine;

namespace SpellBoundAR.DialogueSystem
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Dialogue/Scripted Conversation Entity")]
    public class ScriptedConversationEntity : IdentifiableScriptableObject, IConversationEntity
    {
        [Header("Settings")]
        [SerializeField] private Sprite depiction;
        [SerializeField] private ConversationEntityConfiguration configuration;

        public Sprite Depiction => depiction;
        public ConversationEntity ConversationEntity { get; private set; }

        public Conversation DefaultConversation => ConversationEntity.DefaultConversation;
        public List<Conversation> Conversations => ConversationEntity.Conversations;
        public List<Conversation> GetActiveDialogue() => ConversationEntity.GetActiveDialogue();
        public Conversation GetNextPrioritizedConversation() => ConversationEntity.GetNextPrioritizedConversation();
        public Conversation GetNextConversation() => ConversationEntity.GetNextConversation();
        public Sprite GetPortrait(PortraitType type) => ConversationEntity.GetPortrait(type);

        private void OnEnable()
        {
            ConversationEntity = new ConversationEntity(this, this, configuration);
        }

        private void OnDisable()
        {
            ConversationEntity = null;
        }
    }
}
