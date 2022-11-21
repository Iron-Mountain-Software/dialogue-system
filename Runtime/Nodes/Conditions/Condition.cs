using UnityEngine;

namespace ARISE.DialogueSystem.Nodes.Conditions
{
    public abstract class Condition : DialogueNode
    {
        [Input] public Connection input;
        
        public void LogErrors()
        {
            if (GetInputPort("input").ConnectionCount == 0) Debug.LogError("Dialogue Line Node Error: Empty Input: " + Name, this);
        }
        
#if UNITY_EDITOR
		
        protected override bool ExtensionHasErrors()
        {
            return false;
        }
		
#endif
    }
}