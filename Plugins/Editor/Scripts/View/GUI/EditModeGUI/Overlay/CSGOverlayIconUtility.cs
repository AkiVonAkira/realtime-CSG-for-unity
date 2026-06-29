#if UNITY_2021_3_OR_NEWER
using System.IO;
using UnityEditor;
using UnityEngine;
using UpmPackageInfo = UnityEditor.PackageManager.PackageInfo;

namespace RealtimeCSG
{
    internal static class CSGOverlayIconUtility
    {
        const string ResourcesGuiPrefix = "GUI/";
        const string GuiFolderSuffix = "/Plugins/Editor/Resources/GUI/";
        const string LegacyPackageGuiPath =
            "Packages/com.prenominal.realtimecsg/Plugins/Editor/Resources/GUI/";

        internal static Texture2D Load(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return null;

            var resourceName = ResourcesGuiPrefix + Path.GetFileNameWithoutExtension(fileName);
            var texture = Resources.Load<Texture2D>(resourceName);
            if (texture != null)
                return texture;

            var packageInfo = UpmPackageInfo.FindForAssembly(
                typeof(CSGOverlayIconUtility).Assembly
            );
            if (packageInfo != null && !string.IsNullOrEmpty(packageInfo.assetPath))
            {
                texture = AssetDatabase.LoadAssetAtPath<Texture2D>(
                    packageInfo.assetPath + GuiFolderSuffix + fileName
                );
                if (texture != null)
                    return texture;
            }

            return AssetDatabase.LoadAssetAtPath<Texture2D>(LegacyPackageGuiPath + fileName);
        }
    }
}
#endif
