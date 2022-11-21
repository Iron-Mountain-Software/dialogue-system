using SpellBoundAR.AssetManagement;
using UnityEngine;

namespace ARISE.DialogueSystem
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Gameplay/Dialogue/Database")]
    public class Database : SingletonDatabase<Database>
    {
        [SerializeField] private DatabaseTable<Conversation> conversations = new ();
        public DatabaseTable<Conversation> Conversations => conversations;
    }
}