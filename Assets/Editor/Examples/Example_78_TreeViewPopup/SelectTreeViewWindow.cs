using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using System;


public class SelectTreeViewWindow: EditorWindow 
{ 
    protected EditorWindow parentWindow;
    public static SelectTreeViewWindow current { get; private set; }

    private TreeView treeView;
    private TreeViewState state = new TreeViewState();


    public static T GetOpenWindow<T>() where T : UnityEditor.EditorWindow
    {
        T[] array = Resources.FindObjectsOfTypeAll(typeof(T)) as T[];
        T val = (array.Length != 0) ? array[0] : null;
        return val;
    }

    public static EditorWindow GetOpenWindow(Type parentWinType) 
    {
        var array =  Resources.FindObjectsOfTypeAll(parentWinType);
        var val = (array.Length != 0) ? array[0] : null;
        return val as EditorWindow;
    }


    public static T Show<T, I>(Action<I> cb, Type parentWinType ) where T : BaseSelectTreeView<I>
    {
        var parentWin = GetOpenWindow(parentWinType);
        if (parentWin == null) return null;


        var win = EditorWindow.GetWindow<SelectTreeViewWindow>(true, "Select", true);
        win.parentWindow = parentWin;
        win.RestPostion();

        T treeView = BaseSelectTreeView<I>.Get<T>(win.state);
        win.treeView = treeView;
        treeView.DoubleClick = cb;
        treeView.Create();
        return treeView;
    }




    private void RestPostion()
    {
        Rect parentPos = parentWindow.position;
        float splitW = parentPos.width / 3;
        Rect rect = new Rect(parentPos.x + splitW, parentPos.y, splitW, parentPos.height);
        position = rect;
    }



    private void OnGUI()
    {
        if(treeView != null)
            treeView.OnGUI(GUILayoutUtility.GetRect(0, 100000, 0, 100000));
    }


    private void OnLostFocus()
    {
        Close();
    }

}


