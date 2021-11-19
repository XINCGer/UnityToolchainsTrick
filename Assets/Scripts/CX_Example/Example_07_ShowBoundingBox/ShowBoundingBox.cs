//---------------------------------------------------------------------------------------
// Author: chenxuan@bolygon.com
// Date: 2021-11-15 01:44:01
// Name: ShowBoundingBox
//---------------------------------------------------------------------------------------

using UnityEngine;

namespace Example_07_ShowBoundingBox
{
    public class ShowBoundingBox : MonoBehaviour
    {
        public bool ShowBound;
    
        /// <summary>
        /// 计算包围盒
        /// </summary>
        private Bounds CalculateBound(GameObject obj)
        {
            var res = new Bounds(obj.transform.position, Vector3.zero);
            var renderList = obj.GetComponentsInChildren<Renderer>();
            if (renderList == null || renderList.Length <= 0)
                return res;
            bool isNew = true;
            for (int i = 0; i < renderList.Length; i++)
            {
                if (!renderList[i].enabled)
                    continue;
                if (isNew)
                {
                    res = renderList[i].bounds;
                    isNew = false;
                }
                res.Encapsulate(renderList[i].bounds);
            }
    
            return res;
        }
    
        private void OnDrawGizmos()
        {
            if(!ShowBound) return;
            var bound = CalculateBound(this.gameObject);
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(bound.center, bound.extents * 2);
        }
    }
}

