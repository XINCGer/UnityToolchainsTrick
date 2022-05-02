using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace CX_Example_17
{
    public class ShowLODGroupGUI : EditorWindow
    {
        private readonly int m_LODSliderId = "LODSliderIDHash".GetHashCode();
        private int m_NumberOfLODs = 3;
        private List<float> m_lodHeight = new List<float>{0.8f, 0.5f, 0.3f};
        private int m_SelectedLOD = -1;
        private int m_SelectedLODSlider = -1;

        [MenuItem("CX_Tools/ShowLODGroupGUI")]
        private static void ShowWindow()
        {
            var window = GetWindow<ShowLODGroupGUI>();
            window.titleContent = new GUIContent("ShowLODGroupGUI");
            window.Show();
        }

        private void OnGUI()
        {
            Rect rect = GUILayoutUtility.GetRect(0.0f, 30f, GUILayout.ExpandWidth(true));
            var lodinfos = MyLODGroupGUI.CreateLODInfos(this.m_NumberOfLODs, rect,
                (Func<int, string>) (i => $"LOD {i}"), (Func<int, float>) (i => m_lodHeight[i]));
            DrawLODLevelSlider(rect, lodinfos);
            using (new GUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Add") && m_NumberOfLODs < 8)
                {
                    m_NumberOfLODs++;
                    var insertIndex = m_SelectedLOD == -1 ? 0 : m_SelectedLOD;

                    var frontValue = insertIndex == 0 ? 1 : m_lodHeight[insertIndex - 1];
                    var value = m_lodHeight[insertIndex] + (frontValue - m_lodHeight[insertIndex])/ 2;
                    m_lodHeight.Insert(insertIndex, value);
                }

                if (GUILayout.Button("Remove") && m_NumberOfLODs > 1 && m_SelectedLOD != -1)
                {
                    m_NumberOfLODs--;
                    m_lodHeight.RemoveAt(m_SelectedLOD);
                    m_SelectedLOD = -1;
                    m_SelectedLODSlider = -1;
                }
            }
            
        }

        private void DrawLODLevelSlider(Rect sliderPosition, LODData lods)
        {
            int controlId1 = GUIUtility.GetControlID(this.m_LODSliderId, FocusType.Passive);
            UnityEngine.Event current = UnityEngine.Event.current;
            switch (current.GetTypeForControl(controlId1))
            {
                case EventType.MouseDown:
                    if (current.button == 1 && sliderPosition.Contains(current.mousePosition))
                    {
                        bool flag2 = false;
                        foreach (LODInfo lod in lods.Infos)
                        {
                            if (lod.m_RangePosition.Contains(current.mousePosition))
                            {
                                this.m_SelectedLOD = lod.LODLevel;
                                flag2 = true;
                                break;
                            }
                        }
                        if (!flag2)
                            this.m_SelectedLOD = -1;
                        current.Use();
                        Debug.Log($"MouseDown m_SelectedLOD/m_SelectedLODSlider:{m_SelectedLOD}/{m_SelectedLODSlider}");
                        break;
                    }
                    Rect rect1 = sliderPosition;
                    rect1.x -= 5f;
                    rect1.width += 10f;
                    if (rect1.Contains(current.mousePosition))
                    {
                        current.Use();
                        GUIUtility.hotControl = controlId1;
                        bool flag = false;
                        IOrderedEnumerable<LODInfo> orderedEnumerable1 = lods.Infos.Where<LODInfo>((Func<LODInfo, bool>) (lod => (double) lod.ScreenPercent > 0.5)).OrderByDescending<LODInfo, int>((Func<LODInfo, int>) (x => x.LODLevel));
                        IOrderedEnumerable<LODInfo> orderedEnumerable2 = lods.Infos.Where<LODInfo>((Func<LODInfo, bool>) (lod => (double) lod.ScreenPercent <= 0.5)).OrderBy<LODInfo, int>((Func<LODInfo, int>) (x => x.LODLevel));
                        List<LODInfo> lodInfoList = new List<LODInfo>();
                        lodInfoList.AddRange((IEnumerable<LODInfo>) orderedEnumerable1);
                        lodInfoList.AddRange((IEnumerable<LODInfo>) orderedEnumerable2);
                        foreach (LODInfo lodInfo in lodInfoList)
                        {
                            if (lodInfo.m_ButtonPosition.Contains(current.mousePosition))
                            {
                                this.m_SelectedLODSlider = lodInfo.LODLevel;
                                flag = true;
                                this.BeginLODDrag();
                                break;
                            }
                        }
                        if (!flag)
                        {
                            foreach (LODInfo lodInfo in lodInfoList)
                            {
                                if (lodInfo.m_RangePosition.Contains(current.mousePosition))
                                {
                                    this.m_SelectedLODSlider = -1;
                                    this.m_SelectedLOD = lodInfo.LODLevel;
                                    break;
                                }
                            }
                        }
                    }
                    Debug.Log($"MouseDown m_SelectedLOD/m_SelectedLODSlider:{m_SelectedLOD}/{m_SelectedLODSlider}");
                    break;
                case EventType.MouseUp:
                    if (GUIUtility.hotControl == controlId1)
                    {
                        GUIUtility.hotControl = 0;
                        this.EndLODDrag();
                        current.Use();
                    }
                    Debug.Log($"MouseUp m_SelectedLOD/m_SelectedLODSlider:{m_SelectedLOD}/{m_SelectedLODSlider}");
                    break;
                case EventType.MouseDrag:
                    if (GUIUtility.hotControl == controlId1 && this.m_SelectedLODSlider >= 0 && lods.Infos[this.m_SelectedLODSlider] != null)
                    {
                        current.Use();
                        float cameraPercent = MyLODGroupGUI.GetCameraPercent(current.mousePosition, sliderPosition);
                        MyLODGroupGUI.SetSelectedLODLevelPercentage(cameraPercent - 1f / 1000f, this.m_SelectedLODSlider, ref lods);
                        m_lodHeight[m_SelectedLODSlider] = lods.Infos[this.m_SelectedLODSlider].RawScreenPercent;
                        this.UpdateLODDrag();
                    }
                    Debug.Log($"MouseDrag m_SelectedLOD/m_SelectedLODSlider:{m_SelectedLOD}/{m_SelectedLODSlider}");
                    break;
                case EventType.Repaint:
                    Debug.Log("Repaint");
                    MyLODGroupGUI.DrawLODSlider(sliderPosition, lods.OriginData, this.m_SelectedLOD);
                    break;
            }
        }

        private void BeginLODDrag()
        {
            Debug.Log("StartLODDrag");
        }

        private void UpdateLODDrag()
        {
            Debug.Log("UpdateLODDrag");
        }

        private void EndLODDrag()
        {
            Debug.Log("EndLODDrag");
        }
    }
}