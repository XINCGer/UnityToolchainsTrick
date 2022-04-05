using System;
using UnityEngine;

public class BoneShow : MonoBehaviour
{
    public Transform Root;
    public bool Show = true;
    public float RootPointRed = 0.02f;
    public Color RootColor = Color.red;
    public Color RootPentahedron = Color.green;
    public float BoneSize = 0.02f;

    private Mesh _coneMesh;
    private void OnDrawGizmos()
    {
        if(Root == null || !Show) return;
        ShowBone(Root);
    }

    private void ShowBone(Transform bone)
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(bone.position, RootPointRed);
        var count = bone.childCount;
        if(count <= 0) return;
        for (int i = 0; i < count; i++)
        {
            var child = bone.GetChild(i);
            DrawPentahedron(bone.position,child.position, RootPentahedron, BoneSize);
            ShowBone(child);
        }
    }

    private void DrawPentahedron(Vector3 begin, Vector3 end, Color color, float size)
    {
        if (_coneMesh == null)
        {
            _coneMesh = new Mesh();
            _coneMesh.vertices = new[]
            {
                new Vector3(-0.5f, 0,0.5f),
                new Vector3(0.5f, 0,0.5f),
                new Vector3(0, 1,0),
                new Vector3(-0.5f, 0,-0.5f),
                new Vector3(0.5f, 0,-0.5f),
            };
            //unity是左手定则
            _coneMesh.triangles = new[]
            {
                0,1,2,
                1,4,2,
                4,3,2,
                3,0,2,
            };
            _coneMesh.RecalculateNormals();
        }

        Gizmos.color = color;
        var dir = end - begin;
        var dis = Vector3.Distance(end, begin);
        var qu = Quaternion.FromToRotation(Vector3.up, dir);
        Gizmos.DrawWireMesh(_coneMesh, begin, qu, new Vector3(size, dis, size));
    }

}
