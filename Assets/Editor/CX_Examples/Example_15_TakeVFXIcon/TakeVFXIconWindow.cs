using System;
using System.IO;
using UnityEditor;
using UnityEngine;

public class TakeVFXIconWindow : EditorWindow
{
    [MenuItem("CX_Tools/TakeVFXIconWindow")]
    private static void ShowWindow()
    {
        var window = GetWindow<TakeVFXIconWindow>();
        window.titleContent = new GUIContent("Take VFX Icon");
        window.Show();
    }

    private int _width;
    private int _height;
    private Texture2D _image;

    private void OnGUI()
    {
        _width = EditorGUILayout.IntField("Icon Width:", _width);
        _height = EditorGUILayout.IntField("Icon Height:", _height);
        if (GUILayout.Button("Take Snap by MainCam"))
        {
            _image = TakeSnap();
        }

        if (_image != null && GUILayout.Button("Save Image"))
        {
            var savePath = EditorUtility.SaveFilePanel("Export", "Assets", "vfx", "png");
            if(string.IsNullOrEmpty(savePath)) return;
            byte[] bytes = _image.EncodeToPNG();
            try
            {
                File.WriteAllBytes(savePath, bytes);
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
                Debug.LogError($"the prefab is Can't.path = {savePath}");
            }
        }

        if (_image != null)
        {
            GUILayout.Label(_image);
        }
    }

    public const string CreateVFXImage_AB_MatPath =
        "Assets/Editor/CX_Examples/Example_15_TakeVFXIcon/CreateVFXImage(AphaBlend)_mat.mat";

    private Texture2D TakeSnap()
    {
        var mainCam = Camera.main;
        if (mainCam == null)
        {
            EditorUtility.DisplayDialog("Error", "No Found MainCamera!!!", "OK");
            return null;
        }
        UnityEngine.Material calMat = null;
        Texture2D result = null;
        var temp = RenderTexture.active;
        RenderTexture outputtex = RenderTexture.GetTemporary(_width, _height, 16);
        calMat = AssetDatabase.LoadAssetAtPath<UnityEngine.Material>(CreateVFXImage_AB_MatPath);
        //先拍白色背景
        var whiteTex = TakeTextureByMainCam(mainCam, Color.white);
        //再拍黑色背景
        var blackTex = TakeTextureByMainCam(mainCam, Color.black);
        //创建一个新的RenderTexture
        RenderTexture.active = outputtex;
        calMat.SetTexture("_WhiteBgTex", whiteTex);
        Graphics.Blit(blackTex, outputtex, calMat);
        result = new Texture2D(_width, _height, TextureFormat.RGBA32, false);
        result.ReadPixels(new Rect(0, 0, _width, _height), 0, 0, false); //destY (width - height)/ 2
        result.Apply(false);
        RenderTexture.active = temp;
        RenderTexture.ReleaseTemporary(outputtex);
        return result;
    }

    private Texture2D TakeTextureByMainCam(Camera camera, Color backColor)
    {
        Texture2D result = null;
        var tempColor = camera.backgroundColor;
        var tempClearFlags = camera.clearFlags;
        camera.backgroundColor = backColor;
        camera.clearFlags = CameraClearFlags.Color;
        try
        {
            RenderTexture temp = RenderTexture.active;
            RenderTexture renderTex = RenderTexture.GetTemporary(_width, _height, 16);
            RenderTexture.active = renderTex;
            camera.targetTexture = renderTex;
            camera.Render();
            camera.targetTexture = null;
            result = new Texture2D(_width, _height, TextureFormat.RGBA32, false);
            result.ReadPixels(new Rect(0, 0, _width, _height), 0, 0, false); //destY (width - height)/ 2
            result.Apply(false);
            RenderTexture.active = temp;
            RenderTexture.ReleaseTemporary(renderTex);
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            throw;
        }
        finally
        {
            camera.backgroundColor = tempColor;
            camera.clearFlags = tempClearFlags;
        }
        return result;
    }

}
