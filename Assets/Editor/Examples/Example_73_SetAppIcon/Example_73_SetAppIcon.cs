using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UnityToolchinsTrick
{
    public class Example_73_SetAppIcon
    {
        private const string icon_path = "Assets/Editor/Examples/Example_73_SetAppIcon/icon.png";

        [MenuItem("Tools/Example_73_SetAppIcon/SetDefaultIcon", priority = 73)]
        private static void SetDefaultIcon()
        {
            Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(icon_path);
            UnityInternalAPIEngineBridger.SetDefaultIcon(texture);
        }

        [MenuItem("Tools/Example_73_SetAppIcon/SetAppIcon", priority = 73)]
        private static void SetAppIcon()
        {
            Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(icon_path);

            int[] iconSize = PlayerSettings.GetIconSizesForTargetGroup(BuildTargetGroup.iOS);
            Texture2D[] textureArray = new Texture2D[iconSize.Length];
            for (int i = 0; i < textureArray.Length; i++)
            {
                textureArray[i] = texture;
            }

            textureArray[0] = texture;
            PlayerSettings.SetIconsForTargetGroup(BuildTargetGroup.iOS, textureArray);
            AssetDatabase.SaveAssets();
        }
    }
}