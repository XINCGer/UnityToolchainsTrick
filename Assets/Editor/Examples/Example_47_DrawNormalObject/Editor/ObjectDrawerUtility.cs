using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace CZToolKit.Core.Editors
{
    // Token: 0x02000023 RID: 35
    internal static class ObjectDrawerUtility
    {
        // Token: 0x0600025F RID: 607 RVA: 0x000171E4 File Offset: 0x000153E4
        private static void BuildObjectDrawers()
        {
            if (ObjectDrawerUtility.mapBuilt)
            {
                return;
            }
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (assembly != null)
                {
                    try
                    {
                        foreach (Type type in assembly.GetExportedTypes())
                        {
                            CustomObjectDrawer[] array;
                            if (typeof(ObjectDrawer).IsAssignableFrom(type) && type.IsClass && !type.IsAbstract && (array = (type.GetCustomAttributes(typeof(CustomObjectDrawer), false) as CustomObjectDrawer[])).Length > 0)
                            {
                                ObjectDrawerUtility.objectDrawerTypeMap.Add(array[0].Type, type);
                            }
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            ObjectDrawerUtility.mapBuilt = true;
        }

        private static bool ObjectDrawerForType(Type type, ref ObjectDrawer objectDrawer, ref Type objectDrawerType, int hash)
        {
            ObjectDrawerUtility.BuildObjectDrawers();
            if (!ObjectDrawerUtility.objectDrawerTypeMap.ContainsKey(type))
            {
                return false;
            }
            objectDrawerType = ObjectDrawerUtility.objectDrawerTypeMap[type];
            if (ObjectDrawerUtility.objectDrawerMap.ContainsKey(hash))
            {
                objectDrawer = ObjectDrawerUtility.objectDrawerMap[hash];
            }
            return true;
        }

        public static ObjectDrawer GetObjectDrawer(FieldInfo field)
        {
            ObjectDrawer objectDrawer = null;
            Type type = null;
            if (!ObjectDrawerUtility.ObjectDrawerForType(field.FieldType, ref objectDrawer, ref type, field.GetHashCode()))
            {
                return null;
            }
            if (objectDrawer == null)
            {
                objectDrawer = (Activator.CreateInstance(type) as ObjectDrawer);
                ObjectDrawerUtility.objectDrawerMap.Add(field.GetHashCode(), objectDrawer);
            }
            objectDrawer.FieldInfo = field;
            return objectDrawer;
        }

        public static ObjectDrawer GetObjectDrawer(ObjectDrawerAttribute attribute)
        {
            ObjectDrawer objectDrawer = null;
            Type type = null;
            if (!ObjectDrawerUtility.ObjectDrawerForType(attribute.GetType(), ref objectDrawer, ref type, attribute.GetHashCode()))
            {
                return null;
            }
            if (objectDrawer != null)
                return objectDrawer;
            objectDrawer = (Activator.CreateInstance(type) as ObjectDrawer);
            objectDrawer.Attribute = attribute;
            ObjectDrawerUtility.objectDrawerMap.Add(attribute.GetHashCode(), objectDrawer);
            return objectDrawer;
        }

        private static Dictionary<Type, Type> objectDrawerTypeMap = new Dictionary<Type, Type>();

        private static Dictionary<int, ObjectDrawer> objectDrawerMap = new Dictionary<int, ObjectDrawer>();

        private static bool mapBuilt = false;
    }
}
