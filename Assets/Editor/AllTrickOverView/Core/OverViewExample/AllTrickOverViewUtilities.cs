//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2021年4月11日 9:22:34
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using UnityEditor;

namespace AllTrickOverView.Core
{
    public static class AllTrickOverViewUtilities
    {
        private static readonly Dictionary<Type, AExample_Base> AllTrickOverViewExamples =
            new Dictionary<Type, AExample_Base>();

        private static readonly Dictionary<Type, TrickOverViewItem> AllTrickOverViewItems =
            new Dictionary<Type, TrickOverViewItem>();

        private static readonly AllTrickOverViewUtilities.CategoryComparer CategorySorter =
            new AllTrickOverViewUtilities.CategoryComparer();

        public static void Init()
        {
            
        }
        
        static AllTrickOverViewUtilities()
        {
            Assembly assembly = Assembly.GetAssembly(typeof(AllTrickOverViewUtilities));
            Type[] types = assembly.GetTypes();

            foreach (var type in types)
            {
                object[] objects = type.GetCustomAttributes(typeof(TrickOverViewAttribute), true);
                if (objects.Length == 0 || type.IsAbstract)
                {
                    continue;
                }

                AExample_Base temp = Activator.CreateInstance(type) as AExample_Base;
                AllTrickOverViewExamples.Add(type, temp);
                AllTrickOverViewItems.Add(type, new TrickOverViewItem(temp));
            }
        }

        public static void BuildMenuTree(OdinMenuTree tree)
        {
            foreach (var allTrickOverViewInfo in AllTrickOverViewExamples)
            {
                TrickOverViewInfo trickOverViewInfo = (allTrickOverViewInfo.Value).GetTrickOverViewInfo();
                OdinMenuItem menuItem =
                    new OdinMenuItem(tree, trickOverViewInfo.Name, allTrickOverViewInfo.Key)
                    {
                        Value = allTrickOverViewInfo.Key,
                        SearchString = trickOverViewInfo.Name + trickOverViewInfo.Description
                    };
                tree.AddMenuItemAtPath(trickOverViewInfo.Category, menuItem);
            }

            tree.MenuItems.Sort(AllTrickOverViewUtilities.CategorySorter);
            tree.MarkDirty();
        }

        private class CategoryComparer : IComparer<OdinMenuItem>
        {
            // Token: 0x06001195 RID: 4501 RVA: 0x00055E98 File Offset: 0x00054098
            public int Compare(OdinMenuItem x, OdinMenuItem y)
            {
                int num;
                if (!AllTrickOverViewUtilities.CategoryComparer.Order.TryGetValue(x.Name, out num))
                {
                    num = 0;
                }

                int num2;
                if (!AllTrickOverViewUtilities.CategoryComparer.Order.TryGetValue(y.Name, out num2))
                {
                    num2 = 0;
                }

                if (num == num2)
                {
                    return x.Name.CompareTo(y.Name);
                }

                return num.CompareTo(num2);
            }

            // Token: 0x040009C3 RID: 2499
            private static readonly Dictionary<string, int> Order = new Dictionary<string, int>
            {
                {
                    "Essentials",
                    -10
                },
                {
                    "Misc",
                    8
                },
                {
                    "Meta",
                    9
                },
                {
                    "Unity",
                    10
                },
                {
                    "Debug",
                    50
                }
            };
        }

        public static TrickOverViewItem GetItemByType(Type type)
        {
            if (AllTrickOverViewItems.TryGetValue(type, out var trickOverViewItem))
            {
                return trickOverViewItem;
            }

            return null;
        }

        public static AExample_Base GetExampleByType(Type type)
        {
            if (AllTrickOverViewExamples.TryGetValue(type, out var aExampleBase))
            {
                return aExampleBase;
            }

            return null;
        }
    }
}