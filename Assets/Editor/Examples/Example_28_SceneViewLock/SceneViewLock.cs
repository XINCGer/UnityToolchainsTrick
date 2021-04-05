using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ToolKits
{
    public class SceneViewLock
    {
        [MenuItem("Tools/SceneViewLock/LockDefault", priority = 28)]
        private static void LockDefault()
        {
            var layer = LayerMask.NameToLayer("Default");
            LockLayer(layer);
        }

        [MenuItem("Tools/SceneViewLock/UnLockDefault", priority = 28)]
        private static void UnLocakDefault()
        {
            var layer = LayerMask.NameToLayer("Default");
            UnLockLayer(layer);
        }

        /// <summary>
        /// 设置锁定
        /// </summary>
        public static void LockLayer(int layer)
        {
            Tools.lockedLayers |= 1 << layer;
        }

        /// <summary>
        /// 取消锁定
        /// </summary>
        public static void UnLockLayer(int layer)
        {
            Tools.lockedLayers &= ~(1 << layer);
        }

        /// <summary>
        /// 切换锁定
        /// </summary>
        public static void SwichLockLayer(int layer)
        {
            Tools.lockedLayers ^= 1 << layer;
        }

        /// <summary>
        /// 判断是否锁定
        /// </summary>
        public static bool IsLayerLocked(int layer)
        {
            return (Tools.lockedLayers & 1 << layer) == 1 << layer;
        }
    }
}