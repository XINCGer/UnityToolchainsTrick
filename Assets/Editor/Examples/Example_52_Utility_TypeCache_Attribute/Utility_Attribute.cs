using System;
using System.Collections.Generic;
using System.Reflection;

namespace CZToolKit.Core
{
    public static partial class Utility_Attribute
    {
        #region Class
        /// <summary> 保存类的特性，在编译时重载 </summary>
        private static readonly Dictionary<Type, Attribute[]> TypeAttributes = new Dictionary<Type, Attribute[]>();

        /// <summary> 尝试获取目标类型的目标特性 </summary>
        public static bool TryGetTypeAttribute<AttributeType>(Type _type, out AttributeType _attribute)
            where AttributeType : Attribute
        {
            if (TryGetTypeAttributes(_type, out Attribute[] attributes))
            {
                foreach (var tempAttribute in attributes)
                {
                    _attribute = tempAttribute as AttributeType;
                    if (_attribute != null)
                        return true;
                }
            }

            _attribute = null;
            return false;
        }

        /// <summary> 尝试获取目标类型的所有特性 </summary>
        public static bool TryGetTypeAttributes(Type _type, out Attribute[] _attributes)
        {
            if (TypeAttributes.TryGetValue(_type, out _attributes))
                return _attributes == null || _attributes.Length > 0;

            _attributes = _type.GetCustomAttributes() as Attribute[];
            TypeAttributes[_type] = _attributes;
            return _attributes == null || _attributes.Length > 0;
        }
        #endregion

        #region Field
        /// <summary> 保存字段的特性，在编译时重载 </summary>
        private static readonly Dictionary<Type, Dictionary<string, Attribute[]>> TypeFieldAttributes =
            new Dictionary<Type, Dictionary<string, Attribute[]>>();

        /// <summary> 尝试获取目标类型的目标字段的目标特性 </summary>
        public static bool TryGetFieldInfoAttribute<AttributeType>(FieldInfo _fieldInfo,
            out AttributeType _attribute)
            where AttributeType : Attribute
        {
            _attribute = null;
            if (_fieldInfo == null) return false;
            if (TryGetFieldInfoAttributes(_fieldInfo, out Attribute[] attributes))
            {
                for (int i = 0; i < attributes.Length; i++)
                {
                    _attribute = attributes[i] as AttributeType;
                    if (_attribute != null)
                        return true;
                }
            }
            return false;
        }

        /// <summary> 尝试获取目标类型的目标字段的目标特性 </summary>
        public static bool TryGetFieldAttribute<AttributeType>(Type _type, string _fieldName,
            out AttributeType _attribute)
            where AttributeType : Attribute
        {
            return TryGetFieldInfoAttribute(Utility_Refelection.GetFieldInfo(_type, _fieldName), out _attribute);
        }

        /// <summary> 尝试获取目标类型的目标字段的所有特性 </summary>
        public static bool TryGetFieldInfoAttributes(FieldInfo _fieldInfo,
            out Attribute[] _attributes)
        {
            Dictionary<string, Attribute[]> fieldTypes;
            if (TypeFieldAttributes.TryGetValue(_fieldInfo.DeclaringType, out fieldTypes))
            {
                if (fieldTypes.TryGetValue(_fieldInfo.Name, out _attributes))
                {
                    if (_attributes != null && _attributes.Length > 0)
                        return true;
                    return false;
                }
            }
            else
                fieldTypes = new Dictionary<string, Attribute[]>();

            _attributes = _fieldInfo.GetCustomAttributes(typeof(Attribute), true) as Attribute[];
            fieldTypes[_fieldInfo.Name] = _attributes;
            TypeFieldAttributes[_fieldInfo.DeclaringType] = fieldTypes;
            if (_attributes.Length > 0)
                return true;
            return false;
        }
        #endregion

        #region Method
        /// <summary> 保存方法的特性，在编译时重载 </summary>
        private static readonly Dictionary<Type, Dictionary<string, Attribute[]>> TypeMethodAttributes =
            new Dictionary<Type, Dictionary<string, Attribute[]>>();

        public static bool TryGetMethodInfoAttribute<AttributeType>(MethodInfo _methodInfo,
            out AttributeType _attribute)
            where AttributeType : Attribute
        {
            if (TryGetMethodInfoAttributes(_methodInfo, out Attribute[] attributes))
            {
                for (int i = 0; i < attributes.Length; i++)
                {
                    _attribute = attributes[i] as AttributeType;
                    if (_attribute != null)
                        return true;
                }
            }

            _attribute = null;
            return false;
        }

        /// <summary> 尝试获取目标类型的目标字段的目标特性 </summary>
        public static bool TryGetMethodAttribute<AttributeType>(Type _type, string _methodName,
            out AttributeType _attribute)
            where AttributeType : Attribute
        {
            return TryGetMethodInfoAttribute(Utility_Refelection.GetMethodInfo(_type, _methodName), out _attribute);
        }

        /// <summary> 尝试获取目标类型的目标字段的所有特性 </summary>
        public static bool TryGetMethodInfoAttributes(MethodInfo _methodInfo,
            out Attribute[] _attributes)
        {
            Dictionary<string, Attribute[]> methodTypes;
            if (TypeFieldAttributes.TryGetValue(_methodInfo.DeclaringType, out methodTypes))
            {
                if (methodTypes.TryGetValue(_methodInfo.Name, out _attributes))
                {
                    if (_attributes != null && _attributes.Length > 0)
                        return true;
                    return false;
                }
            }
            else
                methodTypes = new Dictionary<string, Attribute[]>();

            _attributes = _methodInfo.GetCustomAttributes(typeof(Attribute), true) as Attribute[];
            methodTypes[_methodInfo.Name] = _attributes;
            TypeFieldAttributes[_methodInfo.DeclaringType] = methodTypes;
            if (_attributes.Length > 0)
                return true;
            return false;
        }

        /// <summary> 尝试获取目标类型的目标字段的所有特性 </summary>
        public static bool TryGetMethodAttributes(Type _type, string _methodName,
            out Attribute[] _attributes)
        {
            return TryGetMethodInfoAttributes(Utility_Refelection.GetMethodInfo(_type, _methodName), out _attributes);
        }
        #endregion
    }
}