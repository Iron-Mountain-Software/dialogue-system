using IronMountain.Conditions;
using UnityEngine;

namespace SpellBoundAR.DialogueSystem.Conditions
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Gameplay/Dialogue/Conditions/Conversation Played")]
    public class ConditionConversationPlaythroughs : Condition
    {
        [SerializeField] private Conversation conversation;
        [SerializeField] private NumericalComparisonType comparison = NumericalComparisonType.GreaterThan;
        [SerializeField] private int playthroughs = 0;
        
        private void OnEnable()
        {
            if (conversation) conversation.OnPlaythroughsChanged += OnPlaythroughsChanged;
            OnPlaythroughsChanged();
        }

        private void OnDisable()
        {
            if (conversation) conversation.OnPlaythroughsChanged -= OnPlaythroughsChanged;
        }

        private void OnPlaythroughsChanged() => FireOnConditionStateChanged();

        public override bool Evaluate()
        {
            return conversation && EvaluationUtilities.Compare(conversation.Playthroughs, playthroughs, comparison);
        }

        public override string DefaultName => (conversation ? conversation.name : "Null") + " playthroughs is " + comparison + " " + playthroughs;
        
        public override string NegatedName => (conversation ? conversation.name : "Null") + " playthroughs is NOT " + comparison + " " + playthroughs;
        
        public override Sprite Depiction => null;
        
        public override bool HasErrors()
        {
            return !conversation;
        }
    }
}
