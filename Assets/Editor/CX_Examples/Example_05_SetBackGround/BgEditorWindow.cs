//---------------------------------------------------------------------------------------
// Author: xuan_chen_work@foxmail.com
// Date: 2021-11-12 18:51:26
// Name: BgEditorWindow
//---------------------------------------------------------------------------------------
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class BgEditorWindow : EditorWindow
{
    private Image _bgImage;
    private IMGUIContainer _imguiContainer;
    public ScaleMode ScaleMode
    {
        set
        {
            if(_bgImage == null) return;
            _bgImage.scaleMode = value;
        }
    }

    public float Alpha
    {
        set
        {
            if(_bgImage == null) return;
            var color = Color.white;
            color.a = value;
            _bgImage.tintColor = color;
        }
    }

    public void SetBgImage(Texture2D bgTex)
    {
        _bgImage = new Image();
        _bgImage.image = bgTex;
        Alpha = 1;
        this.rootVisualElement.Add(_bgImage);
        _imguiContainer = new IMGUIContainer();
        _bgImage.Add(_imguiContainer);
        _imguiContainer.onGUIHandler += BgOnGUI;
    }

    private void OnGUI()
    {
        if(_bgImage == null) return;
        var windowRect = this.position;
        _bgImage.style.height = windowRect.height;
        _bgImage.style.width = windowRect.width;
        
    }
    protected abstract void BgOnGUI();
}

public class TestEditorWindow : BgEditorWindow
{
    [MenuItem("CX_Tools/BgEditorWindow", priority = 4)]
    private static void ShowWindow()
    {
        var window = GetWindow<TestEditorWindow>();
        var texBg = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Editor/CX_Examples/Example_03_PrefabIconCreator/rennka.png");
        window.SetBgImage(texBg);
        window.ScaleMode = ScaleMode.ScaleToFit;
        window.Alpha = 0.3f;
        window.Show();
    }

    protected override void BgOnGUI()
    {
        GUILayout.Space(20);
        GUILayout.Label("aaaaa");
        if (GUILayout.Button("aaa"))
        {
            Debug.Log("aaa");
        }
    }
}
