using System;
using System.IO;
using System.Reflection;
using UnityEditor;

namespace IronMountain.DialogueSystem.Editor
{
    public static class DirectoryUtilities
    {
        public static string GetCurrentFolder()
        {
            Type projectWindowUtilType = typeof(ProjectWindowUtil);
            MethodInfo getActiveFolderPath = projectWindowUtilType.GetMethod("GetActiveFolderPath", BindingFlags.Static | BindingFlags.NonPublic);
            return getActiveFolderPath is not null 
                ? getActiveFolderPath.Invoke(null, new object[0]).ToString()
                : string.Empty;
        }
        
        public static void CreateFolders(string folder)
        {
            string[] subfolders = folder.Split(Path.DirectorySeparatorChar);
            if (subfolders.Length == 0) return;
            string parent = subfolders[0];
            for (int index = 1; index < subfolders.Length; index++)
            {
                var subfolder = subfolders[index];
                string child = Path.Join(parent, subfolder);
                if (!AssetDatabase.IsValidFolder(child)) AssetDatabase.CreateFolder(parent, subfolder);
                parent = child;
            }
        }
    }
}