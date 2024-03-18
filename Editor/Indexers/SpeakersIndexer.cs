using System.Collections.Generic;
using System.Linq;
using IronMountain.DialogueSystem.Speakers;
using UnityEditor;

namespace IronMountain.DialogueSystem.Editor.Indexers
{
    public static class SpeakersIndexer
    {
        public static List<Speaker> Speakers = new ();

        static SpeakersIndexer()
        {
            LoadAssets();
        }

        private static void LoadAssets()
        {
            Speakers.Clear();
            AssetDatabase.Refresh();
            string[] guids = AssetDatabase.FindAssets($"t:{typeof(Speaker)}");
            for (int i = 0; i < guids.Length; i++ )
            {
                string assetPath = AssetDatabase.GUIDToAssetPath( guids[i] );
                Speaker speaker = AssetDatabase.LoadAssetAtPath<Speaker>( assetPath );
                if (speaker) Speakers.Add(speaker);
            }
        }
        
        public static Speaker Find(string query)
        {
            return Speakers.FirstOrDefault(speaker => speaker && ((ISpeaker) speaker).UsesNameOrAlias(query));
        }
    }
}