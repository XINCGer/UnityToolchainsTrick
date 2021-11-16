//---------------------------------------------------------------------------------------
// Author: xuan_chen_work@foxmail.com
// Date: 2021-11-15 14:45:02
// Name: PrefabPreview
//---------------------------------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;

namespace Example_06_PrefabIconCreator
{
    public static class PrefabPreview
    {
        /// <summary>
        /// 计算投影平面
        /// </summary>
        private struct ProjectionPlane
        {
            private readonly Vector3 m_Normal;
            private readonly float m_Distance;

            public ProjectionPlane(Vector3 inNormal, Vector3 inPoint)
            {
                m_Normal = Vector3.Normalize(inNormal);
                m_Distance = -Vector3.Dot(inNormal, inPoint);
            }

            public Vector3 ClosestPointOnPlane(Vector3 point)
            {
                float d = Vector3.Dot(m_Normal, point) + m_Distance;
                return point - m_Normal * d;
            }

            public float GetDistanceToPoint(Vector3 point)
            {
                float signedDistance = Vector3.Dot(m_Normal, point) + m_Distance;
                if (signedDistance < 0f)
                    signedDistance = -signedDistance;

                return signedDistance;
            }
        }

        private const float defaultAspect = 1.6f;

        /// <summary>
        /// 计算包围盒
        /// </summary>
        private static Bounds CalculateBounds(GameObject obj)
        {
            var res = new Bounds(obj.transform.position, Vector3.zero);
            var renderList = obj.GetComponentsInChildren<Renderer>();
            if (renderList == null || renderList.Length <= 0)
            {
                Debug.LogError("This prefab not exit renderer!!");
                return res;
            }

            for (int i = 0; i < renderList.Length; i++)
            {
                if (!renderList[i].enabled)
                    continue;
                res.Encapsulate(renderList[i].bounds);
            }

            return res;
        }

        /// <summary>
        /// 计算相机最佳距离, 正交返回的是最大size和距离，透视返回的是距离，无视size
        /// </summary>
        /// <param name="bounds"></param>
        /// <param name="renderCam"></param>
        /// <returns></returns>
        private static (float distance, float size) CalculateBestDistance(Bounds bounds, Camera renderCam)
        {
            //计算第一象限8个点的世界坐标
            var poss = new List<Vector3>();
            var pos = bounds.center + bounds.extents; //+++
            poss.Add(pos);
            pos.x -= 2 * bounds.extents.x; //-++
            poss.Add(pos);
            pos.y -= 2 * bounds.extents.y; //--+
            poss.Add(pos);
            pos.x += 2 * bounds.extents.x; //+-+
            poss.Add(pos);
            pos.z -= 2 * bounds.extents.z; //+--
            poss.Add(pos);
            pos.x -= 2 * bounds.extents.x; //---
            poss.Add(pos);
            pos.y += 2 * bounds.extents.y; //-+-
            poss.Add(pos);
            pos.x += 2 * bounds.extents.x; //++-
            poss.Add(pos);

            if (renderCam.orthographic)
            {
                var minX = Mathf.Infinity;
                var minY = minX;
                var maxX = Mathf.NegativeInfinity;
                var maxY = maxX;
                for (int i = 0; i < poss.Count; i++)
                {
                    Vector3 localPoint = renderCam.transform.InverseTransformPoint(poss[i]);
                    if (localPoint.x < minX)
                        minX = localPoint.x;
                    if (localPoint.x > maxX)
                        maxX = localPoint.x;
                    if (localPoint.y < minY)
                        minY = localPoint.y;
                    if (localPoint.y > maxY)
                        maxY = localPoint.y;
                }

                return (bounds.extents.magnitude + 1f, Mathf.Max(maxY - minY, (maxX - minX) / defaultAspect) * 0.5f);
            }
            else
            {
                var projectionPlaneHorizontal = new ProjectionPlane(renderCam.transform.up, bounds.center);
                var projectionPlaneVertical = new ProjectionPlane(renderCam.transform.right, bounds.center);
                var maxDistance = Mathf.NegativeInfinity;
                for (int i = 0; i < poss.Count; i++)
                {
                    Vector3 intersectionPoint = projectionPlaneHorizontal.ClosestPointOnPlane(poss[i]);
                    float horizontalDistance = projectionPlaneHorizontal.GetDistanceToPoint(poss[i]);
                    float verticalDistance = projectionPlaneVertical.GetDistanceToPoint(poss[i]);
                    // Credit: https://docs.unity3d.com/Manual/FrustumSizeAtDistance.html
                    float distanceV = verticalDistance / Mathf.Tan(renderCam.fieldOfView * 0.5f * Mathf.Deg2Rad);
                    var hfieldOfView = Camera.VerticalToHorizontalFieldOfView(renderCam.fieldOfView, defaultAspect);
                    float distanceH = horizontalDistance / Mathf.Tan(hfieldOfView * 0.5f * Mathf.Deg2Rad);
                    var distance = Mathf.Max(distanceH,distanceV);
                    float distanceToCenter = (intersectionPoint - renderCam.transform.forward * distance - bounds.center).sqrMagnitude;
                    if (distanceToCenter > maxDistance)
                        maxDistance = distanceToCenter;
                }
                return (Mathf.Sqrt(maxDistance), 0f);
            }
        }

        public static PreviewSettingData CreateDefaultPreviewSetting(GameObject obj, Camera renderCam)
        {
            var setting = new PreviewSettingData();
            renderCam.transform.rotation = Quaternion.Euler(setting.PitchAngle, setting.StartAngle, 0);
            setting.Bounds = CalculateBounds(obj);
            var (dis,size) = CalculateBestDistance(setting.Bounds, renderCam);
            setting.Distance = dis;
            return setting;
        }

        public static Texture2D CreatePreviewTexture(Camera renderCam, PreviewSettingData setting, int width)
        {
            //经验参数
            var offsetting = 1+ (defaultAspect <= 1 ? 0 : defaultAspect * 0.04f);
            //相机设定
            renderCam.transform.rotation = Quaternion.Euler(setting.PitchAngle, setting.StartAngle, 0);
            var camPos = setting.Bounds.center + setting.CenterOffSet - offsetting *  setting.Distance * renderCam.transform.forward;
            renderCam.transform.position = camPos;
            renderCam.clearFlags = CameraClearFlags.SolidColor;
            renderCam.backgroundColor = setting.BgColor;
            return CreateTextureFromCam(renderCam, width);
        }

        /// <summary>
        /// 直接用相机拍摄
        /// </summary>
        /// <param name="renderCam"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        public static Texture2D CreateTextureFromCam(Camera renderCam, int width)
        {
            //拍摄
            Texture2D result = null;
            RenderTexture temp = RenderTexture.active;
            var height = (int) (width / defaultAspect);
            RenderTexture renderTex = new RenderTexture(width, height, 16)
            {
                antiAliasing = 2
            }; //获取临时渲染纹理
            RenderTexture.active = renderTex;
            renderCam.targetTexture = renderTex;
            renderCam.Render();
            result = new Texture2D(width, height, TextureFormat.RGBA32, false);
            result.ReadPixels(new Rect(0, 0, width, height), 0, 0, false);//destY (width - height)/ 2
            result.Apply(false, false);
            //释放临时纹理
            renderCam.targetTexture = null;
            RenderTexture.active = temp;
            renderTex.Release();
            return result;
        }
    }
}