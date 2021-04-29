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
                EditorPrefs.SetBool(text, flag);
            return flag;
        }

        public static object DrawFields(object obj)
        {
            return DrawFields(obj, null);
        }

        public static bool CanDraw(FieldInfo _fieldInfo)
        {
            return !CSUtility.TryGetTypeAttribute(_fieldInfo.DeclaringType, out NonSerializedAttribute nonAtt)
                    && !CSUtility.TryGetFieldInfoAttribute(_fieldInfo, out HideInInspector hideAtt)
                    && ((!_fieldInfo.IsPrivate && !_fieldInfo.IsFamily) || CSUtility.TryGetFieldInfoAttribute(_fieldInfo, out SerializeField serAtt));
        }

        public static object DrawFields(object obj, GUIContent guiContent)
        {
            if (obj == null) return null;

            List<FieldInfo> fields = CSUtility.GetFieldInfos(obj.GetType());
            for (int j = 0; j < fields.Count; j++)
            {
                if (CanDraw(fields[j]))
                {
                    if (guiContent == null)
                    {
                        string name = fields[j].Name;
                        if (CSUtility.TryGetFieldInfoAttribute(fields[j], out TooltipAttribute tooltipAtt))
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
            return obj;
        }

        private static object DrawField(GUIContent _content, FieldInfo _fieldInfo, object _value)
        {
            if (typeof(IList).IsAssignableFrom(_fieldInfo.FieldType))
                return EditorGUILayoutExtension.DrawArrayField(_content, _fieldInfo, _fieldInfo.FieldType, _value);
            return EditorGUILayoutExtension.DrawSingleField(_content, _fieldInfo.FieldType, _value);
        }

        public static object DrawField(GUIContent _content, Type _fieldType, object _value)
        {
            if (typeof(IList).IsAssignableFrom(_fieldType))
                return EditorGUILayoutExtension.DrawArrayField(_content, null, _fieldType, _value);
            return EditorGUILayoutExtension.DrawSingleField(_content, _fieldType, _value);
        }

        private static object DrawArrayField(GUIContent _content, FieldInfo _fieldInfo, Type _fieldType, object _value)
        {
            Type elementType;
            if (_fieldType.IsArray)
                elementType = _fieldType.GetElementType();
            else
            {
                Type type2 = _fieldType;
                while (!type2.IsGenericType)
                {
                    type2 = type2.BaseType;
                }
                elementType = type2.GetGenericArguments()[0];
            }
            IList list;
            if (_value == null)
            {
                if (_fieldType.IsGenericType || _fieldType.IsArray)
                {
                    list = (Activator.CreateInstance(typeof(List<>).MakeGenericType(new Type[]
                    {
                        elementType
                    }), true) as IList);
                }
                else
                {
                    list = (Activator.CreateInstance(_fieldType, true) as IList);
                }
                if (_fieldType.IsArray)
                {
                    Array array = Array.CreateInstance(elementType, list.Count);
                    list.CopyTo(array, 0);
                    list = array;
                }
                GUI.changed = true;
            }
            else
            {
                list = (IList)_value;
            }
            EditorGUILayout.BeginVertical(new GUILayoutOption[0]);
            if (EditorGUILayoutExtension.DrawFoldout(_content.text.GetHashCode(), _content))
            {
                EditorGUI.indentLevel++;
                bool flag = _content.text.GetHashCode() == EditorGUILayoutExtension.editingFieldHash;
                int num = (!flag) ? list.Count : EditorGUILayoutExtension.savedArraySize;
                int num2 = EditorGUILayout.IntField("Size", num, new GUILayoutOption[0]);
                if (flag && EditorGUILayoutExtension.editingArray && (GUIUtility.keyboardControl != EditorGUILayoutExtension.currentKeyboardControl || Event.current.keyCode == (KeyCode)13))
                {
                    if (num2 != list.Count)
                    {
                        Array array2 = Array.CreateInstance(elementType, num2);
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
                            if (i >= list.Count && !typeof(Object).IsAssignableFrom(elementType) && !typeof(string).IsAssignableFrom(elementType))
                            {
                                value2 = Activator.CreateInstance(list[num3].GetType(), true);
                            }
                            array2.SetValue(value2, i);
                        }
                        if (_fieldType.IsArray)
                        {
                            list = array2;
                        }
                        else
                        {
                            if (_fieldType.IsGenericType)
                            {
                                list = (Activator.CreateInstance(typeof(List<>).MakeGenericType(new Type[]
                                {
                                    elementType
                                }), true) as IList);
                            }
                            else
                            {
                                list = (Activator.CreateInstance(_fieldType, true) as IList);
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
                        EditorGUILayoutExtension.editingFieldHash = _content.text.GetHashCode();
                    }
                    EditorGUILayoutExtension.savedArraySize = num2;
                }

                if (_fieldInfo != null
                    && (CSUtility.TryGetFieldInfoAttribute(_fieldInfo, out ObjectDrawerAttribute att)
                    || CSUtility.TryGetTypeAttribute(elementType, out att)))
                {
                    for (int k = 0; k < list.Count; k++)
                    {
                        ObjectDrawer objectDrawer;
                        if ((objectDrawer = ObjectDrawerUtility.GetObjectDrawer(att)) != null)
                        {
                            _content.text = "Element " + k;
                            objectDrawer.Value = list[k];
                            objectDrawer.OnGUI(_content);
                            if (objectDrawer.Value != list[k])
                            {
                                list[k] = objectDrawer.Value;
                                GUI.changed = true;
                            }
                        }
                    }
                }
                else
                {
                    for (int k = 0; k < list.Count; k++)
                    {
                        GUILayout.BeginHorizontal(new GUILayoutOption[0]);
                        _content.text = "Element " + k;

                        list[k] = EditorGUILayoutExtension.DrawField(_content, elementType, list[k]);

                        GUILayout.Space(6f);
                        GUILayout.EndHorizontal();
                    }
                }
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.EndVertical();
            return list;
        }

        private static object DrawSingleField(GUIContent _content, Type _fieldType, object _value)
        {
            if (_fieldType.Equals(typeof(int)))
            {
                return EditorGUILayout.IntField(_content, (int)_value);
            }
            if (_fieldType.Equals(typeof(float)))
            {
                return EditorGUILayout.FloatField(_content, (float)_value);
            }
            if (_fieldType.Equals(typeof(double)))
            {
                return EditorGUILayout.FloatField(_content, Convert.ToSingle((double)_value));
            }
            if (_fieldType.Equals(typeof(long)))
            {
                return (long)EditorGUILayout.IntField(_content, Convert.ToInt32((long)_value));
            }
            if (_fieldType.Equals(typeof(bool)))
            {
                return EditorGUILayout.Toggle(_content, (bool)_value);
            }
            if (_fieldType.Equals(typeof(string)))
            {
                return EditorGUILayout.TextField(_content, (string)_value);
            }
            if (_fieldType.Equals(typeof(byte)))
            {
                return Convert.ToByte(EditorGUILayout.IntField(_content, Convert.ToInt32(_value)));
            }
            if (_fieldType.Equals(typeof(Vector2)))
            {
                return EditorGUILayout.Vector2Field(_content, (Vector2)_value);
            }
            if (_fieldType.Equals(typeof(Vector2Int)))
            {
                return EditorGUILayout.Vector2IntField(_content, (Vector2Int)_value);
            }
            if (_fieldType.Equals(typeof(Vector3)))
            {
                return EditorGUILayout.Vector3Field(_content, (Vector3)_value);
            }
            if (_fieldType.Equals(typeof(Vector3Int)))
            {
                return EditorGUILayout.Vector3IntField(_content, (Vector3Int)_value);
            }
            if (_fieldType.Equals(typeof(Vector3)))
            {
                return EditorGUILayout.Vector3Field(_content, (Vector3)_value);
            }
            if (_fieldType.Equals(typeof(Vector4)))
            {
                return EditorGUILayout.Vector4Field(_content.text, (Vector4)_value);
            }
            if (_fieldType.Equals(typeof(Quaternion)))
            {
                Quaternion quaternion = (Quaternion)_value;
                Vector4 vector = Vector4.zero;
                vector.Set(quaternion.x, quaternion.y, quaternion.z, quaternion.w);
                vector = EditorGUILayout.Vector4Field(_content.text, vector);
                quaternion.Set(vector.x, vector.y, vector.z, vector.w);
                return quaternion;
            }
            if (_fieldType.Equals(typeof(Color)))
            {
                return EditorGUILayout.ColorField(_content, (Color)_value);
            }
            if (_fieldType.Equals(typeof(Rect)))
            {
                return EditorGUILayout.RectField(_content, (Rect)_value);
            }
            if (_fieldType.Equals(typeof(Matrix4x4)))
            {
                GUILayout.BeginVertical(new GUILayoutOption[0]);
                if (EditorGUILayoutExtension.DrawFoldout(_content.text.GetHashCode(), _content))
                {
                    EditorGUI.indentLevel++;
                    Matrix4x4 matrix4x = (Matrix4x4)_value;
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
                    _value = matrix4x;
                    EditorGUI.indentLevel--;
                }
                GUILayout.EndVertical();
                return _value;
            }
            if (_fieldType.Equals(typeof(AnimationCurve)))
            {
                if (_value == null)
                {
                    _value = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
                    GUI.changed = true;
                }
                return EditorGUILayout.CurveField(_content, (AnimationCurve)_value);
            }
            if (_fieldType.Equals(typeof(LayerMask)))
            {
                return EditorGUILayoutExtension.DrawLayerMask(_content, (LayerMask)_value);
            }
            if (typeof(Object).IsAssignableFrom(_fieldType))
            {
                return EditorGUILayout.ObjectField(_content, (Object)_value, _fieldType, true);
            }
            if (_fieldType.IsEnum)
            {
                return EditorGUILayout.EnumPopup(_content, (Enum)_value);
            }
            if (_fieldType.IsClass || (_fieldType.IsValueType && !_fieldType.IsPrimitive))
            {
                if (typeof(Delegate).IsAssignableFrom(_fieldType))
                {
                    return null;
                }
                int hashCode = _content.text.GetHashCode();
                if (EditorGUILayoutExtension.drawnObjects.Contains(hashCode))
                {
                    return null;
                }
                try
                {
                    EditorGUILayoutExtension.drawnObjects.Add(hashCode);
                    GUILayout.BeginVertical(new GUILayoutOption[0]);
                    if (_value == null)
                    {
                        if (_fieldType.IsGenericType && _fieldType.GetGenericTypeDefinition() == typeof(Nullable<>))
                        {
                            _fieldType = Nullable.GetUnderlyingType(_fieldType);
                        }
                        _value = Activator.CreateInstance(_fieldType, true);
                    }
                    if (EditorGUILayoutExtension.DrawFoldout(hashCode, _content))
                    {
                        EditorGUI.indentLevel++;
                        _value = EditorGUILayoutExtension.DrawFields(_value);
                        EditorGUI.indentLevel--;
                    }
                    EditorGUILayoutExtension.drawnObjects.Remove(hashCode);
                    GUILayout.EndVertical();
                    return _value;
                }
                catch (Exception)
                {
                    GUILayout.EndVertical();
                    EditorGUILayoutExtension.drawnObjects.Remove(hashCode);
                    return null;
                }
            }
            EditorGUILayout.LabelField("Unsupported Type: " + _fieldType);
            return null;
        }

        private static object DrawSingleField(GUIContent _content, FieldInfo _fieldInfo, object _value)
        {
            ObjectDrawer objectDrawer;
            if ((CSUtility.TryGetFieldInfoAttribute(_fieldInfo, out ObjectDrawerAttribute att) || CSUtility.TryGetTypeAttribute(_fieldInfo.FieldType, out att))
            && (objectDrawer = ObjectDrawerUtility.GetObjectDrawer(att)) != null)
            {
                if (_value == null && !_fieldInfo.FieldType.IsAbstract)
                {
                    if (typeof(ScriptableObject).IsAssignableFrom(_fieldInfo.FieldType))
                        _value = ScriptableObject.CreateInstance(_fieldInfo.FieldType);
                    else
                        _value = Activator.CreateInstance(_fieldInfo.FieldType, true);
                }
                objectDrawer.Value = _value;
                objectDrawer.OnGUI(_content);
                if (objectDrawer.Value != _value)
                {
                    _value = objectDrawer.Value;
                    GUI.changed = true;
                }
                return _value;
            }
            return DrawSingleField(_content, _fieldInfo.FieldType, _value);
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
