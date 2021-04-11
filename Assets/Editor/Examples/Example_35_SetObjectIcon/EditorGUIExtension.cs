using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace SetObjectIcon
{
    public static class EditorGUIExtension
    {
        //sv_label_0 - sv_label_7
        //sv_icon_name0 - sv_icon_name7
        //sv_icon_dot0_sml - sv_icon_dot15_sml
        //sv_icon_dot0_pix16_gizmo - sv_icon_dot15_pix16_gizmo
        public static void SetIcon(Object obj, string name)
        {
            Texture2D tex = EditorGUIUtility.IconContent(name).image as Texture2D;
            object[] args = new object[] {obj, tex};
            typeof(EditorGUIUtility).InvokeMember("SetIconForObject", BindingFlags.InvokeMethod | BindingFlags.Static | BindingFlags.NonPublic, null, null, args);
        }
    }
}