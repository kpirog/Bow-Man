using System.IO;
using System.Linq;
using UnityEngine;

namespace Utils
{
    public static class ResourcesExtension
    {
        private static readonly string ResourcesPath = Application.dataPath + "/Resources/Prefabs";

        public static string GetPrefabPath(string resourceName)
        {
            var directories = Directory.GetDirectories(ResourcesPath, "*", SearchOption.AllDirectories);

            foreach (var directory in directories)
            {
                if (Directory.GetFiles(directory).Any(x => x.Contains(resourceName)))
                {
                    return "Prefabs/" + Path.GetFileName(directory) + "/" + resourceName;
                }
            }

            return "";
        }
    }
}