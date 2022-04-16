using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UnityToolchinsTrick
{
    public class UnityInternalAPIEngineBridger 
    {
        public static void SetDefaultIcon(Texture2D icon)
        {
            string platformName = "";
            Texture2D[] icons = PlayerSettings.GetAllIconsForPlatform(platformName);
            int[] widths = PlayerSettings.GetIconWidthsOfAllKindsForPlatform(platformName);

            // Ensure the default icon list is always populated correctly
            if (icons.Length != widths.Length)
            {
                icons = new Texture2D[widths.Length];
            }

            icons[0] = icon;
            PlayerSettings.SetIconsForPlatform(platformName, icons);
            AssetDatabase.SaveAssets();
        }
    }
}

