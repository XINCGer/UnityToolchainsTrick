using AllTrickOverView.Core;

namespace AllTrickOverView.Examples
{
    public class Example_GetObjectLocalIdInFile : AExample_Base
    {
        public static TrickOverViewInfo TrickOverViewInfo =
            new TrickOverViewInfo("GetObjectLocalIdInFile",
                "获取LocalIdentifierInFile",
                "Assets",
                "/// <summary>\n/// 获取LocalIdentifierInFile\n/// </summary>\n/// <param name=\"_object\"></param>\n/// <returns></returns>\npublic static long GetObjectLocalIdInFile(Object _object)\n{\n    long idInFile = 0;\n    SerializedObject serialize = new SerializedObject(_object);\n    PropertyInfo inspectorModeInfo =\n        typeof(SerializedObject).GetProperty(\"inspectorMode\", BindingFlags.NonPublic | BindingFlags.Instance);\n    if (inspectorModeInfo != null)\n        inspectorModeInfo.SetValue(serialize, InspectorMode.Debug, null);\n    SerializedProperty localIdProp = serialize.FindProperty(\"m_LocalIdentfierInFile\");\n    idInFile = localIdProp.longValue;\n    return idInFile;\n}",
                "Assets/Editor/Examples/Example_72_GetLocalIdentifierInFile",
                typeof(Example_GetObjectLocalIdInFile),
                picPath : "Assets/Editor/Examples/Example_72_GetLocalIdentifierInFile/QQ截图20220408102311.png",
                videoPath : "");

        public override TrickOverViewInfo GetTrickOverViewInfo()
        {
            return TrickOverViewInfo;
        }
    }
}
