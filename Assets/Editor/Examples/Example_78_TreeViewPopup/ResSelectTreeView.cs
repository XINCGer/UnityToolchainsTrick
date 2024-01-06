using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEditor;


/// <summary>
/// 资源选择TreeView界面 需要自定义资源数据结构
/// </summary>
/// <typeparam name="T"></typeparam>
public class ResSelectTreeView<T> : BaseSelectTreeView<T>  where T : BaseModelData
{
    public ResSelectTreeView(TreeViewState state) : base(state) { }
    public ResSelectTreeView(TreeViewState state, IList<T> modelDatas) : base(state, modelDatas) { }

    public override string GetRowTitle(T data)
    {
        if (data != null)
        {
            string extraDes = data.GetExtraDes();
            return string.IsNullOrEmpty(extraDes) ? data.GetShowName() : data.GetShowName() + "_" + extraDes;
        }
        return "";
    }


/*     public override List<T> Create()
    {
        var readList = CutsceneEditReader.ReadList<T>();
        List<T> results = new List<T>();
        foreach (var modelData in readList)
        {
            results.Add(modelData as T);
        }

        SetData(results);
        return results;
    }*/

    protected override void PingData(object data)
    {

        T modelData =  data as T;

        var asset =  AssetDatabase.LoadMainAssetAtPath(modelData.AssetName);
        if(asset != null)
        {
            Selection.activeObject = asset;
            EditorGUIUtility.PingObject(asset);
        }
    }

}

