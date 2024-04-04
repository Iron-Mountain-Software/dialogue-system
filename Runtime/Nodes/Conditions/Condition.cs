using UnityEngine;

namespace IronMountain.DialogueSystem.Nodes.Conditions
{
    public abstract class Condition : DialogueNode
    {
        [Input] public Connection input;

#if UNITY_EDITOR

        public override void RefreshErrors()
        {
            base.RefreshErrors();
            if (GetInputPort("input").ConnectionCount == 0) Errors.Add("Bad input.");
        }
        
#endif
    }
}