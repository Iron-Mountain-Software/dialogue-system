using SpellBoundAR.Conditions;
using UnityEngine;

namespace SpellBoundAR.DialogueSystem.Conditions
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Gameplay/Dialogue/Conditions/Conversation Played")]
    public class ConditionConversationPlayed : Condition
    {
        [SerializeField] private Conversation conversation;

        private void OnEnable()
        {
            if (conversation) conversation.OnPlaythroughsChanged += OnPlaythroughsChanged;
        }

        private void OnDisable()
        {
            if (conversation) conversation.OnPlaythroughsChanged -= OnPlaythroughsChanged;
        }

        private void OnPlaythroughsChanged() => FireOnConditionStateChanged();

        public override bool Evaluate() => conversation && conversation.Playthroughs > 0;
        public override string DefaultName => (conversation ? conversation.name : "Null") + " was Played";
        public override string NegatedName => (conversation ? conversation.name : "Null") + " was NOT Played";
        
        public override Sprite Depiction => null;
        
        public override bool HasErrors()
        {
            return !conversation;
        }
    }
}
