using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class LocalIdentifierInFileExtension
{
    /// <summary>
    /// 获取LocalIdentifierInFile
    /// </summary>
    /// <param name="_object"></param>
    /// <returns></returns>
    public static long GetObjectLocalIdInFile(Object _object)
    {
        long idInFile = 0;
        SerializedObject serialize = new SerializedObject(_object);
        PropertyInfo inspectorModeInfo =
            typeof(SerializedObject).GetProperty("inspectorMode", BindingFlags.NonPublic | BindingFlags.Instance);
        if (inspectorModeInfo != null)
            inspectorModeInfo.SetValue(serialize, InspectorMode.Debug, null);
        SerializedProperty localIdProp = serialize.FindProperty("m_LocalIdentfierInFile");
        idInFile = localIdProp.longValue;
        return idInFile;
    }

    [MenuItem("Assets/GetObjectLocalIdInFile")]
    public static void GetSelectionLocalId()
    {
        Debug.Log(GetObjectLocalIdInFile(Selection.activeGameObject.GetComponent<Transform>()));
    }
}
