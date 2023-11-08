using SpellBoundAR.DialogueSystem.UI;
using UnityEngine;

namespace SpellBoundAR.DialogueSystem.Nodes.Conditions
{
    [NodeWidth(500)]
    public class PassFailConditionFromReference : PassFailCondition
    {
        [SerializeField] private bool not;
        [SerializeField] public IronMountain.Conditions.Condition condition;
        public override string Name => not ? condition.NegatedName : condition.DefaultName;

        protected override bool TestCondition(ConversationPlayer conversationUI)
        {
            return not 
                ? !condition.Evaluate() 
                : condition.Evaluate();
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