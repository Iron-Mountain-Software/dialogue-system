using IronMountain.DialogueSystem.UI;
using UnityEngine;

namespace IronMountain.DialogueSystem.Nodes.Conditions
{
    [NodeWidth(500)]
    public class PassFailConditionFromReference : PassFailCondition
    {
        [SerializeField] public IronMountain.Conditions.Condition condition;
        
        public override string Name => condition ? condition.ToString() : "NULL CONDITION";

        protected override bool TestCondition(ConversationPlayer conversationUI)
        {
            return condition && condition.Evaluate();
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