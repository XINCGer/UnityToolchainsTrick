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
}


