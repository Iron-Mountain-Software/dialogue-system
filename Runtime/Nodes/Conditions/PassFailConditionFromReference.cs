using UnityEngine;

namespace ARISE.DialogueSystem.Nodes.Conditions
{
    public class PassFailConditionFromReference : PassFailCondition
    {
        [SerializeField] private bool not;
        [SerializeField] private SpellBoundAR.Conditions.Condition condition;
        public override string Name => not ? condition.NegatedName : condition.DefaultName;

        protected override bool TestCondition(ConversationUI conversationUI)
        {
            return not ? !condition.Evaluate() : condition.Evaluate();
        }
        
#if UNITY_EDITOR

        protected override bool ExtensionHasWarnings()
        {
            return false;
        }

        protected override bool ExtensionHasErrors()
        {
            return !condition;
        }
		
#endif
    }
}