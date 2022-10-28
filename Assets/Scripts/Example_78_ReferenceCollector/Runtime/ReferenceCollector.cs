#region 注 释

/***
 *
 *  Title:
 *  
 *  Description:
 *  
 *  Date:
 *  Version:
 *  Writer: 半只龙虾人
 *  Github: https://github.com/HalfLobsterMan
 *  Blog: https://www.crosshair.top/
 *
 */

#endregion

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace CZToolKit.Core
{
    [DisallowMultipleComponent]
    public class ReferenceCollector : MonoBehaviour, ISerializationCallbackReceiver
    {
        [Serializable]
        public class ReferencePair
        {
            public string key;
            public UnityObject value;
        }

        [SerializeField] private List<ReferencePair> references = new List<ReferencePair>();

        private Dictionary<string, UnityObject> referencesDict;

        private Dictionary<string, UnityObject> InternalReferencesDict
        {
            get
            {
                if (referencesDict == null)
                    referencesDict = new Dictionary<string, UnityObject>();
                return referencesDict;
            }
        }

        public IReadOnlyDictionary<string, UnityObject> ReferencesDict
        {
            get
            {
                if (referencesDict == null)
                    referencesDict = new Dictionary<string, UnityObject>();
                return referencesDict;
            }
        }

#if UNITY_EDITOR
        public void Add()
        {
            string key = string.Empty;
            do
            {
                key = UnityEngine.Random.Range(int.MinValue, int.MaxValue).ToString();
            } while (ReferencesDict.ContainsKey(key));

            references.Add(new ReferencePair() { key = key });
        }

        public void RemoveAt(int index)
        {
            references.RemoveAt(index);
        }

        public void RemoveAt(ReferencePair pair)
        {
            references.Remove(pair);
        }

        public void ClearEmpty()
        {
            references.RemoveAll(pair => string.IsNullOrEmpty(pair.key) || pair.value == null);
        }

        public void Clear()
        {
            references.Clear();
        }

        public void Sort()
        {
            QuickSort(references, (a, b) => { return a.key.CompareTo(b.key); });
        }

        public static void QuickSort<T>(IList<T> original, Func<T, T, int> comparer)
        {
            if (original.Count <= 1)
                return;
            QuickSort(0, original.Count - 1);

            void QuickSort(int left, int right)
            {
                if (left < right)
                {
                    int middleIndex = (left + right) / 2;
                    T middle = original[middleIndex];
                    int i = left;
                    int j = right;
                    while (true)
                    {
                        // 双指针收缩
                        // 找到一个大于中数的下标和一个小于中数的下标，交换位置
                        while (i < j && comparer(original[i], middle) < 0)
                        {
                            i++;
                        }

                        ;
                        while (j > i && comparer(original[j], middle) > 0)
                        {
                            j--;
                        }

                        ;
                        if (i == j) break;

                        T temp = original[i];
                        original[i] = original[j];
                        original[j] = temp;

                        if (comparer(original[i], original[j]) == 0) j--;
                    }

                    QuickSort(left, i);
                    QuickSort(i + 1, right);
                }
            }
        }
#endif

        public UnityObject Get(string key)
        {
            if (InternalReferencesDict.TryGetValue(key, out var value))
                return value;
            return null;
        }

        public T Get<T>(string key) where T : UnityObject
        {
            if (InternalReferencesDict.TryGetValue(key, out var value))
                return value as T;
            return null;
        }

        private void RefreshDict()
        {
            InternalReferencesDict.Clear();
            foreach (var pair in references)
            {
                if (string.IsNullOrEmpty(pair.key))
                    continue;
                InternalReferencesDict[pair.key] = pair.value;
            }
        }

        public void OnBeforeSerialize()
        {
        }

        public void OnAfterDeserialize()
        {
            RefreshDict();
        }
    }
}