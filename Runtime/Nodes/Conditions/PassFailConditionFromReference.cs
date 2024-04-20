using IronMountain.DialogueSystem.UI;
using UnityEngine;

namespace IronMountain.DialogueSystem.Nodes.Conditions
{
    [NodeWidth(500)]
    public class PassFailConditionFromReference : PassFailCondition
    {
        [SerializeField] public IronMountain.Conditions.Condition condition;
        
        public override string Name => condition ? condition.ToString() : "NULL CONDITION";

        protected override bool TestCondition(ConversationPlayer conversationPlayer)
        {
            return condition && condition.Evaluate();
        }
        
#if UNITY_EDITOR

        public override void RefreshErrors()
        {
            base.RefreshErrors();
            if (!condition) Errors.Add("No condition.");
        }
		
#endif
    }
}