using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using Object = UnityEngine.Object;

namespace CX_Example_17
{
    public class LODData
    {
        public object OriginData;
        public List<LODInfo> Infos;
    }

    public class LODInfo
    {
        public Rect m_ButtonPosition;
        public Rect m_RangePosition;

        public LODInfo(int lodLevel, string name, float screenPercentage)
        {
            this.LODLevel = lodLevel;
            this.LODName = name;
            this.RawScreenPercent = screenPercentage;
        }

        public int LODLevel { get; private set; }

        public string LODName { get; private set; }

        public float RawScreenPercent { get; set; }

        public float ScreenPercent
        {
            get => MyLODGroupGUI.DelinearizeScreenPercentage(RawScreenPercent);
            set => this.RawScreenPercent = MyLODGroupGUI.LinearizeScreenPercentage(value);
        }
    }
    
    public static class MyLODGroupGUI
    {
        private static Type _lodGroupGUI;

        static MyLODGroupGUI()
        {
            var assembly = Assembly.GetAssembly(typeof(Editor));
            _lodGroupGUI = assembly.GetType("UnityEditor.LODGroupGUI");
        }
        
        public static float DelinearizeScreenPercentage(float percentage) => Mathf.Approximately(0.0f, percentage) ? 0.0f : Mathf.Sqrt(percentage);

        public static float LinearizeScreenPercentage(float percentage) => percentage * percentage;

        
        public static float GetCameraPercent(Vector2 position, Rect sliderRect) => LinearizeScreenPercentage(Mathf.Clamp((float) (1.0 - ((double) position.x - (double) sliderRect.x) / (double) sliderRect.width), 0.01f, 1f));
        
        public static void SetSelectedLODLevelPercentage(
            float newScreenPercentage,
            int lod,
            ref LODData lodData)
        {
            var method = _lodGroupGUI.GetMethod("SetSelectedLODLevelPercentage", BindingFlags.Static | BindingFlags.Public);
            method.Invoke(null, new object[] {newScreenPercentage,lod, lodData.OriginData});
            lodData = ChangeToLODData(lodData.OriginData);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="area"></param>
        /// <returns>lodInfos</returns>
        public static LODData CreateLODInfos(int lodcount, Rect area, Func<int, string> nameGen, Func<int, float> heightGen)
        {
            var method = _lodGroupGUI.GetMethod("CreateLODInfos", BindingFlags.Static | BindingFlags.Public);
            var data =  method.Invoke(null, new object[] {lodcount,area, nameGen, heightGen});
            return ChangeToLODData(data);
        }

        private static LODData ChangeToLODData(object data)
        {
            var res = new LODData
            {
                OriginData = data,
                Infos = new List<LODInfo>(),
            };

            var dataType = data.GetType();
            int count = Convert.ToInt32(dataType.GetProperty("Count").GetValue(data, null));
            for (int i = 0; i < count; i++)
            {
                // 获取列表子元素，然后子元素其实也是一个类，然后递归调用当前方法获取类Man的所有公共属性
                object item=dataType.GetProperty("Item").GetValue(data,new object[]{i});
                var itemType = item.GetType();
                var LODLevel = Convert.ToInt32(itemType.GetProperty("LODLevel").GetValue(item));
                var LODName = Convert.ToString(itemType.GetProperty("LODName").GetValue(item));
                var RawScreenPercent = Convert.ToSingle(itemType.GetProperty("RawScreenPercent").GetValue(item));
                var newInfo = new LODInfo(LODLevel, LODName, RawScreenPercent);
                newInfo.m_ButtonPosition = (Rect) itemType.GetField("m_ButtonPosition").GetValue(item);
                newInfo.m_RangePosition = (Rect) itemType.GetField("m_RangePosition").GetValue(item);
                res.Infos.Add(newInfo);
            }

            return res;
        }

        public static void DrawLODSlider(Rect area, object lodInfos, int selectedId)
        {
            var method = _lodGroupGUI.GetMethod("DrawLODSlider", BindingFlags.Static | BindingFlags.Public);
            method.Invoke(null, new object[] {area, lodInfos, selectedId});
        }
    }
}