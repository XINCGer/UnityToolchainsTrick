using UnityEditor;
using UnityEngine;

namespace Example_19_InternalBridge
{
    public class InternalMeshUtilBridge
    {
        public static int CalcTriangleCount(Mesh mesh)
        {
            return InternalMeshUtil.CalcTriangleCount(mesh);
        }
    }

    public class InternalShaderUtilBridge
    {
        public static void OpenShaderCombinations(Shader shader, bool usedBySceneOnly = true)
        {
            ShaderUtil.OpenShaderCombinations(shader, usedBySceneOnly);
        }
        
        public static string[] GetShaderGlobalKeywords(Shader shader)
        {
            return ShaderUtil.GetShaderGlobalKeywords(shader);
        }

        public static string[] GetAllGlobalKeywords()
        {
            return ShaderUtil.GetAllGlobalKeywords();
        }

        public static string[] GetShaderLocalKeywords(Shader shader)
        {
            return ShaderUtil.GetShaderLocalKeywords(shader);
        }
    }
}


