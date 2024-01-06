using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.IMGUI.Controls;
using UnityEditor;


/// <summary>
/// 对象选择界面TreeViwe（针对UnityObject）
/// </summary>
/// <typeparam name="T"></typeparam>
public class ObjectSelectTreeView<T> : BaseSelectTreeView<T> where T:UnityEngine.Object
{
    public ObjectSelectTreeView(TreeViewState state) : base(state) { }
    public ObjectSelectTreeView(TreeViewState state, IList<T> modelDatas) : base(state, modelDatas) { }


        public override string GetRowTitle(T data)
    {
        if (data != null)
            return data.name;
        return "";
    }

    protected override void PingData(object data)
    {
        if (data == null) return;

        T uObject = data as T;
        Selection.activeObject = uObject;
        EditorGUIUtility.PingObject(uObject);
    }
}

