using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

using Object = UnityEngine.Object;

namespace CZToolKit.Core.Editors
{
    public static class EditorGUILayoutExtension
    {

        public static bool DrawFoldout(int hash, GUIContent guiContent)
        {
            string text = string.Concat(new object[]
            {
                c_EditorPrefsFoldoutKey,
                hash,
                ".",
                guiContent.text
            });
            bool @bool = EditorPrefs.GetBool(text, true);
            bool flag = EditorGUILayout.Foldout(@bool, guiContent, true);
            if (flag != @bool)
            {
                EditorPrefs.SetBool(text, flag);
            }
            return flag;
        }

        public static object DrawFields(object obj)
        {
            return DrawFields(obj, null);
        }

        public static object DrawFields(object obj, GUIContent guiContent)
        {
            if (obj == null) return null;

            List<Type> baseClasses = EditorGUILayoutExtension.GetBaseClasses(obj.GetType());
            BindingFlags bindingAttr = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            for (int i = baseClasses.Count - 1; i > -1; i--)
            {
                FieldInfo[] fields = baseClasses[i].GetFields(bindingAttr);
                for (int j = 0; j < fields.Length; j++)
                {
                    if (!AttributeCache.TryGetTypeAttribute(fields[j].DeclaringType, out NonSerializedAttribute nonAtt)
                        && !AttributeCache.TryGetFieldInfoAttribute(fields[j], out HideInInspector hideAtt)
                        && ((!fields[j].IsPrivate && !fields[j].IsFamily) || AttributeCache.TryGetFieldInfoAttribute(fields[j], out SerializeField serAtt)))
                    {
                        if (guiContent == null)
                        {
                            string name = fields[j].Name;
                            if (AttributeCache.TryGetFieldInfoAttribute(fields[j], out TooltipAttribute tooltipAtt))
                                guiContent = new GUIContent(ObjectNames.NicifyVariableName(name), tooltipAtt.tooltip);
                            else
                                guiContent = new GUIContent(ObjectNames.NicifyVariableName(name));
                        }
                        EditorGUI.BeginChangeCheck();
                        object value = EditorGUILayoutExtension.DrawField(guiContent, fields[j], fields[j].GetValue(obj));
                        if (EditorGUI.EndChangeCheck())
                        {
                            fields[j].SetValue(obj, value);
                            GUI.changed = true;
                        }
                        guiContent = null;
                    }
                }
            }
            return obj;
        }


        public static List<Type> GetBaseClasses(Type t)
        {
            List<Type> list = new List<Type>();
            while (t != null)
            {
                list.Add(t);
                t = t.BaseType;
            }
            return list;
        }


        public static object DrawField(GUIContent guiContent, FieldInfo field, object value)
        {
            // 自定义Drawer，先禁用后续修复
            //ObjectDrawer objectDrawer;
            //if ((objectDrawer = ObjectDrawerUtility.GetObjectDrawer(task, field)) != null)
            //{
            //    if (value == null && !field.FieldType.IsAbstract)
            //    {
            //        if (typeof(ScriptableObject).IsAssignableFrom(field.FieldType))
            //        {
            //            value = ScriptableObject.CreateInstance(field.FieldType);
            //        }
            //        else
            //        {
            //            value = Activator.CreateInstance(field.FieldType, true);
            //        }
            //    }
            //    objectDrawer.Value = value;
            //    objectDrawer.OnGUI(guiContent);
            //    if (objectDrawer.Value != value)
            //    {
            //        value = objectDrawer.Value;
            //        GUI.changed = true;
            //    }
            //    return value;
            //}
            //ObjectDrawerAttribute[] array;
            //if ((array = (field.GetCustomAttributes(typeof(ObjectDrawerAttribute), true) as ObjectDrawerAttribute[])).Length > 0 && (objectDrawer = ObjectDrawerUtility.GetObjectDrawer(task, array[0])) != null)
            //{
            //    if (value == null)
            //    {
            //        if (typeof(ScriptableObject).IsAssignableFrom(field.FieldType))
            //        {
            //            value = ScriptableObject.CreateInstance(field.FieldType);
            //        }
            //        else
            //        {
            //            value = Activator.CreateInstance(field.FieldType, true);
            //        }
            //    }
            //    objectDrawer.Value = value;
            //    objectDrawer.OnGUI(guiContent);
            //    if (objectDrawer.Value != value)
            //    {
            //        value = objectDrawer.Value;
            //        GUI.changed = true;
            //    }
            //    return value;
            //}
            return EditorGUILayoutExtension.DrawField(guiContent, field, field.FieldType, value);
        }

        // Token: 0x06000199 RID: 409 RVA: 0x0000E708 File Offset: 0x0000C908
        private static object DrawField(GUIContent guiContent, FieldInfo fieldInfo, Type fieldType, object value)
        {
            if (typeof(IList).IsAssignableFrom(fieldType))
            {
                return EditorGUILayoutExtension.DrawArrayField(guiContent, fieldInfo, fieldType, value);
            }
            return EditorGUILayoutExtension.DrawSingleField(guiContent, fieldInfo, fieldType, value);
        }

        // Token: 0x0600019A RID: 410 RVA: 0x0000E744 File Offset: 0x0000C944
        private static object DrawArrayField(GUIContent guiContent, FieldInfo fieldInfo, Type fieldType, object value)
        {
            Type type;
            if (fieldType.IsArray)
            {
                type = fieldType.GetElementType();
            }
            else
            {
                Type type2 = fieldType;
                while (!type2.IsGenericType)
                {
                    type2 = type2.BaseType;
                }
                type = type2.GetGenericArguments()[0];
            }
            IList list;
            if (value == null)
            {
                if (fieldType.IsGenericType || fieldType.IsArray)
                {
                    list = (Activator.CreateInstance(typeof(List<>).MakeGenericType(new Type[]
                    {
                        type
                    }), true) as IList);
                }
                else
                {
                    list = (Activator.CreateInstance(fieldType, true) as IList);
                }
                if (fieldType.IsArray)
                {
                    Array array = Array.CreateInstance(type, list.Count);
                    list.CopyTo(array, 0);
                    list = array;
                }
                GUI.changed = true;
            }
            else
            {
                list = (IList)value;
            }
            EditorGUILayout.BeginVertical(new GUILayoutOption[0]);
            if (EditorGUILayoutExtension.DrawFoldout(guiContent.text.GetHashCode(), guiContent))
            {
                EditorGUI.indentLevel++;
                bool flag = guiContent.text.GetHashCode() == EditorGUILayoutExtension.editingFieldHash;
                int num = (!flag) ? list.Count : EditorGUILayoutExtension.savedArraySize;
                int num2 = EditorGUILayout.IntField("Size", num, new GUILayoutOption[0]);
                if (flag && EditorGUILayoutExtension.editingArray && (GUIUtility.keyboardControl != EditorGUILayoutExtension.currentKeyboardControl || Event.current.keyCode == (KeyCode)13))
                {
                    if (num2 != list.Count)
                    {
                        Array array2 = Array.CreateInstance(type, num2);
                        int num3 = -1;
                        for (int i = 0; i < num2; i++)
                        {
                            if (i < list.Count)
                            {
                                num3 = i;
                            }
                            if (num3 == -1)
                            {
                                break;
                            }
                            object value2 = list[num3];
                            if (i >= list.Count && !typeof(Object).IsAssignableFrom(type) && !typeof(string).IsAssignableFrom(type))
                            {
                                value2 = Activator.CreateInstance(list[num3].GetType(), true);
                            }
                            array2.SetValue(value2, i);
                        }
                        if (fieldType.IsArray)
                        {
                            list = array2;
                        }
                        else
                        {
                            if (fieldType.IsGenericType)
                            {
                                list = (Activator.CreateInstance(typeof(List<>).MakeGenericType(new Type[]
                                {
                                    type
                                }), true) as IList);
                            }
                            else
                            {
                                list = (Activator.CreateInstance(fieldType, true) as IList);
                            }
                            for (int j = 0; j < array2.Length; j++)
                            {
                                list.Add(array2.GetValue(j));
                            }
                        }
                    }
                    EditorGUILayoutExtension.editingArray = false;
                    EditorGUILayoutExtension.savedArraySize = -1;
                    EditorGUILayoutExtension.editingFieldHash = -1;
                    GUI.changed = true;
                }
                else if (num2 != num)
                {
                    if (!EditorGUILayoutExtension.editingArray)
                    {
                        EditorGUILayoutExtension.currentKeyboardControl = GUIUtility.keyboardControl;
                        EditorGUILayoutExtension.editingArray = true;
                        EditorGUILayoutExtension.editingFieldHash = guiContent.text.GetHashCode();
                    }
                    EditorGUILayoutExtension.savedArraySize = num2;
                }
                for (int k = 0; k < list.Count; k++)
                {
                    GUILayout.BeginHorizontal(new GUILayoutOption[0]);
                    guiContent.text = "Element " + k;
                    list[k] = EditorGUILayoutExtension.DrawField(guiContent, fieldInfo, type, list[k]);
                    GUILayout.Space(6f);
                    GUILayout.EndHorizontal();
                }
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.EndVertical();
            return list;
        }

        private static object DrawSingleField(GUIContent guiContent, FieldInfo fieldInfo, Type fieldType, object value)
        {
            if (fieldType.Equals(typeof(int)))
            {
                return EditorGUILayout.IntField(guiContent, (int)value);
            }
            if (fieldType.Equals(typeof(float)))
            {
                return EditorGUILayout.FloatField(guiContent, (float)value);
            }
            if (fieldType.Equals(typeof(double)))
            {
                return EditorGUILayout.FloatField(guiContent, Convert.ToSingle((double)value));
            }
            if (fieldType.Equals(typeof(long)))
            {
                return (long)EditorGUILayout.IntField(guiContent, Convert.ToInt32((long)value));
            }
            if (fieldType.Equals(typeof(bool)))
            {
                return EditorGUILayout.Toggle(guiContent, (bool)value);
            }
            if (fieldType.Equals(typeof(string)))
            {
                return EditorGUILayout.TextField(guiContent, (string)value);
            }
            if (fieldType.Equals(typeof(byte)))
            {
                return Convert.ToByte(EditorGUILayout.IntField(guiContent, Convert.ToInt32(value)));
            }
            if (fieldType.Equals(typeof(Vector2)))
            {
                return EditorGUILayout.Vector2Field(guiContent, (Vector2)value);
            }
            if (fieldType.Equals(typeof(Vector2Int)))
            {
                return EditorGUILayout.Vector2IntField(guiContent, (Vector2Int)value);
            }
            if (fieldType.Equals(typeof(Vector3)))
            {
                return EditorGUILayout.Vector3Field(guiContent, (Vector3)value);
            }
            if (fieldType.Equals(typeof(Vector3Int)))
            {
                return EditorGUILayout.Vector3IntField(guiContent, (Vector3Int)value);
            }
            if (fieldType.Equals(typeof(Vector3)))
            {
                return EditorGUILayout.Vector3Field(guiContent, (Vector3)value);
            }
            if (fieldType.Equals(typeof(Vector4)))
            {
                return EditorGUILayout.Vector4Field(guiContent.text, (Vector4)value);
            }
            if (fieldType.Equals(typeof(Quaternion)))
            {
                Quaternion quaternion = (Quaternion)value;
                Vector4 vector = Vector4.zero;
                vector.Set(quaternion.x, quaternion.y, quaternion.z, quaternion.w);
                vector = EditorGUILayout.Vector4Field(guiContent.text, vector);
                quaternion.Set(vector.x, vector.y, vector.z, vector.w);
                return quaternion;
            }
            if (fieldType.Equals(typeof(Color)))
            {
                return EditorGUILayout.ColorField(guiContent, (Color)value);
            }
            if (fieldType.Equals(typeof(Rect)))
            {
                return EditorGUILayout.RectField(guiContent, (Rect)value);
            }
            if (fieldType.Equals(typeof(Matrix4x4)))
            {
                GUILayout.BeginVertical(new GUILayoutOption[0]);
                if (EditorGUILayoutExtension.DrawFoldout(guiContent.text.GetHashCode(), guiContent))
                {
                    EditorGUI.indentLevel++;
                    Matrix4x4 matrix4x = (Matrix4x4)value;
                    for (int i = 0; i < 4; i++)
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            EditorGUI.BeginChangeCheck();
                            matrix4x[i, j] = EditorGUILayout.FloatField("E" + i.ToString() + j.ToString(), matrix4x[i, j]);
                            if (EditorGUI.EndChangeCheck())
                            {
                                GUI.changed = true;
                            }
                        }
                    }
                    value = matrix4x;
                    EditorGUI.indentLevel--;
                }
                GUILayout.EndVertical();
                return value;
            }
            if (fieldType.Equals(typeof(AnimationCurve)))
            {
                if (value == null)
                {
                    value = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
                    GUI.changed = true;
                }
                return EditorGUILayout.CurveField(guiContent, (AnimationCurve)value);
            }
            if (fieldType.Equals(typeof(LayerMask)))
            {
                return EditorGUILayoutExtension.DrawLayerMask(guiContent, (LayerMask)value);
            }
            if (typeof(Object).IsAssignableFrom(fieldType))
            {
                return EditorGUILayout.ObjectField(guiContent, (Object)value, fieldType, true);
            }
            if (fieldType.IsEnum)
            {
                return EditorGUILayout.EnumPopup(guiContent, (Enum)value);
            }
            if (fieldType.IsClass || (fieldType.IsValueType && !fieldType.IsPrimitive))
            {
                if (typeof(Delegate).IsAssignableFrom(fieldType))
                {
                    return null;
                }
                int hashCode = guiContent.text.GetHashCode();
                if (EditorGUILayoutExtension.drawnObjects.Contains(hashCode))
                {
                    return null;
                }
                try
                {
                    EditorGUILayoutExtension.drawnObjects.Add(hashCode);
                    GUILayout.BeginVertical(new GUILayoutOption[0]);
                    if (value == null)
                    {
                        if (fieldType.IsGenericType && fieldType.GetGenericTypeDefinition() == typeof(Nullable<>))
                        {
                            fieldType = Nullable.GetUnderlyingType(fieldType);
                        }
                        value = Activator.CreateInstance(fieldType, true);
                    }
                    if (EditorGUILayoutExtension.DrawFoldout(hashCode, guiContent))
                    {
                        EditorGUI.indentLevel++;
                        value = EditorGUILayoutExtension.DrawFields(value);
                        EditorGUI.indentLevel--;
                    }
                    EditorGUILayoutExtension.drawnObjects.Remove(hashCode);
                    GUILayout.EndVertical();
                    return value;
                }
                catch (Exception)
                {
                    GUILayout.EndVertical();
                    EditorGUILayoutExtension.drawnObjects.Remove(hashCode);
                    return null;
                }
            }
            EditorGUILayout.LabelField("Unsupported Type: " + fieldType);
            return null;
        }

        private static LayerMask DrawLayerMask(GUIContent guiContent, LayerMask layerMask)
        {
            if (EditorGUILayoutExtension.layerNames == null)
            {
                EditorGUILayoutExtension.InitLayers();
            }
            int num = 0;
            for (int i = 0; i < EditorGUILayoutExtension.layerNames.Length; i++)
            {
                if ((layerMask.value & EditorGUILayoutExtension.maskValues[i]) == EditorGUILayoutExtension.maskValues[i])
                {
                    num |= 1 << i;
                }
            }
            int num2 = EditorGUILayout.MaskField(guiContent, num, EditorGUILayoutExtension.layerNames);
            if (num2 != num)
            {
                num = 0;
                for (int j = 0; j < EditorGUILayoutExtension.layerNames.Length; j++)
                {
                    if ((num2 & 1 << j) != 0)
                    {
                        num |= EditorGUILayoutExtension.maskValues[j];
                    }
                }
                layerMask.value = num;
            }
            return layerMask;
        }

        private static void InitLayers()
        {
            List<string> list = new List<string>();
            List<int> list2 = new List<int>();
            for (int i = 0; i < 32; i++)
            {
                string text = LayerMask.LayerToName(i);
                if (!string.IsNullOrEmpty(text))
                {
                    list.Add(text);
                    list2.Add(1 << i);
                }
            }
            EditorGUILayoutExtension.layerNames = list.ToArray();
            EditorGUILayoutExtension.maskValues = list2.ToArray();
        }

        private const string c_EditorPrefsFoldoutKey = "CZToolKit.Core.Editors.Foldout.";

        private static int currentKeyboardControl = -1;

        private static bool editingArray = false;

        private static int savedArraySize = -1;

        private static int editingFieldHash;

        private static HashSet<int> drawnObjects = new HashSet<int>();

        private static string[] layerNames;

        private static int[] maskValues;
    }
}
