using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;



public class TreeViewItem<T> : TreeViewItem 
{
    public T data { get; set; }


    public TreeViewItem(int id, int depth, string displayName, T data) :base (id, depth, displayName)
    {
        this.data = data;
    }
}

public class BaseSelectTreeView<T> : TreeView 
{
    private IList<T> modelDatas  ;

    SearchField searchField = new SearchField();

    public Action<T> DoubleClick;
    public Action<T> SingleClick;
    public Action<T> SelectionChange;

    //获取Treeview实例
    public static K Get<K>(TreeViewState state) where K : BaseSelectTreeView<T>
    {
            return (K)System.Activator.CreateInstance(typeof(K), new System.Object[] { state });
    }

    public BaseSelectTreeView(TreeViewState state) : base(state)
    {
        showAlternatingRowBackgrounds = true;
        searchField.downOrUpArrowKeyPressed += this.SetFocusAndEnsureSelectedItem;
        Reload();
    }

    public BaseSelectTreeView(TreeViewState state, IList<T> modelDatas) : base(state)
    {
        modelDatas = this.modelDatas;
        searchField.downOrUpArrowKeyPressed += this.SetFocusAndEnsureSelectedItem;
        Reload();
    }


    public override void OnGUI(Rect rect)
    {
        searchString = searchField.OnGUI(new Rect(rect.x, rect.y, rect.width, 20), searchString);
        base.OnGUI(new Rect(rect.x, rect.y + 20, rect.width, rect.height - 20));
    }
    protected override TreeViewItem BuildRoot()
    {
        return new TreeViewItem { id = 0, depth = -1, displayName = "root" };
    }

    protected override IList<TreeViewItem> BuildRows(TreeViewItem root)
    {
        int index = 1;
        if (root != null && root.children != null)
            root.children.Clear();
        var rows = GetRows() ?? new List<TreeViewItem>(200);
        rows.Clear();

        if(modelDatas != null)
        {
            foreach (var item in modelDatas)
            {
                TreeViewItem<T> treeViewItem = new TreeViewItem<T>(index, 1, GetRowTitle(item), item);
                index++;
                root.AddChild(treeViewItem);
            }

        }

        SetupDepthsFromParentsAndChildren(root);
        return base.BuildRows(root);
    }


    protected override void DoubleClickedItem(int id)
    {
        var item = FindItem(id, rootItem) as TreeViewItem<T>;
        if (item != null)
        {
            DoubleClick?.Invoke(item.data);
            EditorApplication.QueuePlayerLoopUpdate();
            UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
        }
          
    }

    protected override void SingleClickedItem(int id)
    {
        var item = FindItem(id, rootItem) as TreeViewItem<T>;
        if (item != null)
            SingleClick?.Invoke(item.data);
    }

    protected override void SelectionChanged(IList<int> selectedIds)
    {
        var item = FindItem(selectedIds[0], rootItem) as TreeViewItem<T>;
        if (item != null)
            SelectionChange?.Invoke(item.data);
    }


    protected override void ContextClickedItem(int id)
    {
        var item = FindItem(id, rootItem) as TreeViewItem<T>;
        ShowContextMenu(item);
    }

    public void SetSelection(int id)
    {
        this.SetSelection(new List<int>() { id });
    }


    public void UnSelection()
    {
        SetSelection(0);
    }


    private List<KeyValuePair<string, GenericMenu.MenuFunction2>> contextMenus;
    private void ShowContextMenu(TreeViewItem<T> item)
    {
        if (item == null) return;
        if (contextMenus == null) contextMenus = new List<KeyValuePair<string, GenericMenu.MenuFunction2>>();
        GenericMenu menu = new GenericMenu();
        foreach (var data in contextMenus)
        {
            menu.AddItem(new GUIContent(data.Key), false, data.Value, item.data);
        }
        if (contextMenus != null && contextMenus.Count > 0)
            menu.AddSeparator("");
        menu.AddItem(new GUIContent("定位对象"), false, PingData, item.data);


        menu.ShowAsContext();
    }


    /// <summary>
    /// 自定位资源路径
    /// </summary>
    /// <param name="data">Type is T</param>
    protected virtual void PingData(object data)
    {
        Debug.Log("Please realize this function by yourself ");
    }




    //可代码自己添加菜单选项
    public void AddMenu(string menuName, GenericMenu.MenuFunction2 action)
    {
        if (contextMenus == null)
            contextMenus = new List<KeyValuePair<string, GenericMenu.MenuFunction2>>();
        contextMenus.Add(new KeyValuePair<string, GenericMenu.MenuFunction2>(menuName, action));
    }


    public virtual void SetData(IList<T> modelDatas)
    {
        this.modelDatas = modelDatas;
        Reload();
    }


    //每行显示内容字符串自定义
    public virtual  string GetRowTitle(T data)
    {
        return data.ToString();
    }


    /// <summary>
    /// 用于初始化数据
    /// </summary>
    /// <returns></returns>
    public virtual List<T>  Create()
    {
        return new List<T>(); 
    }



        


}

