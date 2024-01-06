using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.IMGUI.Controls;

public class TreeViewPopupExampleWindow : EditorWindow
{
    [MenuItem("Tools/Example_78_TreeViewPopup")]

    static void Open()
    {
        EditorWindow.GetWindow<TreeViewPopupExampleWindow>();
    }



    private ObjectSelectTreeView<SceneAsset> sceneTreeView;
    private TreeViewState state = new TreeViewState();


    public void OnGUI()
    {
        if(GUILayout.Button("Show Popup"))
        {
            var selectPopup = SelectTreeViewWindow.Show<ObjectSelectTreeView<GameObject>, GameObject>(obj =>
            {
                Debug.Log( $" Example_78_TreeViewPopup_ {obj.name}");
            }, typeof(TreeViewPopupExampleWindow));
            selectPopup.SetData(GameObject.FindObjectsOfType<GameObject>());
        }


        GUILayout.Space(4f);
        EditorGUI.DrawRect(GUILayoutUtility.GetRect(0f, 1f), Color.black);
        GUILayout.Space(4f);

        InitSceneTreeView();

        if (sceneTreeView != null)
            sceneTreeView.OnGUI(GUILayoutUtility.GetRect(0, 100000, 0, 100000));
    }


    private void InitSceneTreeView()
    {
        if (sceneTreeView == null)
        {
            sceneTreeView = BaseSelectTreeView<SceneAsset>.Get<ObjectSelectTreeView<SceneAsset>>(state);
            List<SceneAsset> result = new List<SceneAsset>();
            string[] guids = AssetDatabase.FindAssets("t:SceneAsset", new string[] { "Assets"});
            foreach (var guid in guids)
            {

                var path = AssetDatabase.GUIDToAssetPath(guid);
                var asset = AssetDatabase.LoadAssetAtPath<SceneAsset>(path);
                result.Add(asset);
            }
            sceneTreeView.SetData(result);

            void Slect(object selectAsset)
            {
                if (selectAsset is SceneAsset sceneAsset)
                {
                    if (sceneAsset != null)
                    {
                        Debug.Log($" Example_78_TreeViewPopup_ {AssetDatabase.GetAssetPath(sceneAsset)}");
                    }
                }
            }

            sceneTreeView.AddMenu("Ñ¡Ôñ", Slect);
            sceneTreeView.DoubleClick = Slect;

        }
    }


}
