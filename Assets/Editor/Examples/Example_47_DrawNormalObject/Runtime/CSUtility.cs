using System;
using System.Collections.Generic;
using System.Reflection;

namespace CZToolKit.Core
{
    public static class CSUtility
    {
        #region GetChildrenType

        static readonly Dictionary<Type, IEnumerable<Type>> TypeCache = new Dictionary<Type, IEnumerable<Type>>();

        public static List<Type> GetBaseClasses(Type _type)
        {
            List<Type> list = new List<Type>();
            while (_type != null)
            {
                list.Add(_type);
                _type = _type.BaseType;
            }
            return list;
        }

        public static IEnumerable<Type> GetChildrenTypes<T>()
        {
            return GetChildrenTypes(typeof(T));
        }

        public static IEnumerable<Type> GetChildrenTypes(Type baseType)
        {
            if (TypeCache.TryGetValue(baseType, out IEnumerable<Type> childrenTypes))
            {
                foreach (var item in childrenTypes)
                {
                    yield return item;
                }
                yield break;
            }

            TypeCache[baseType] = childrenTypes = BuildCache(baseType);
            foreach (var type in childrenTypes)
            {
                yield return type;
            }
        }

        private static IEnumerable<Type> BuildCache(Type _baseType)
        {
            var selfAssembly = Assembly.GetAssembly(_baseType);
            if (selfAssembly.FullName.StartsWith("Assembly-CSharp") && !selfAssembly.FullName.Contains("-firstpass"))
            {
                // If is not used as a DLL, check only CSharp (fast)
                foreach (var type in selfAssembly.GetTypes())
                {
                    if (!type.IsAbstract && _baseType.IsAssignableFrom(type))
                    {
                        yield return type;
                    }
                }
            }
            else
            {
                Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
                // Else, check all relevant DDLs (slower)
                // ignore all unity related assemblies
                foreach (Assembly assembly in assemblies)
                {
                    if (assembly.FullName.StartsWith("Unity")) continue;
                    // unity created assemblies always have version 0.0.0
                    if (!assembly.FullName.Contains("Version=0.0.0")) continue;
                    foreach (var type in assembly.GetTypes())
                    {
                        if (type != null && !type.IsAbstract && _baseType.IsAssignableFrom(type))
                            yield return type;
                    }
                }
            }
        }

        #endregion

        #region GetMemberInfo
        static Dictionary<Type, FieldInfo[]> TypeFieldInfoCache = new Dictionary<Type, FieldInfo[]>();

        /// <summary> 获取字段，包括基类的私有字段 </summary>
        public static FieldInfo GetFieldInfo(Type _type, string _fieldName)
        {
            // 如果第一次没有找到，那么这个变量可能是基类的私有字段
            FieldInfo field = _type.GetField(_fieldName,
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            // 只搜索基类的私有字段
            while (field == null && (_type = _type.BaseType) != null)
            {
                field = _type.GetField(_fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
            }

            return field;
        }

        public static List<FieldInfo> GetFieldInfos(Type _type)
        {
            List<FieldInfo> fieldInfos =
                new List<FieldInfo>(
                    _type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance));

            // 获取类包含的所有字段(包含私有)
            while ((_type = _type.BaseType) != null)
            {
                fieldInfos.AddRange(_type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance));
            }
            return fieldInfos;
        }

        /// <summary> 获取方法，包括基类的私有方法 </summary>
        public static MethodInfo GetMethodInfo(Type _type, string _methodName)
        {
            // 如果第一次没有找到，那么这个变量可能是基类的私有字段
            MethodInfo method = _type.GetMethod(_methodName,
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            // 只搜索基类的私有方法
            while (method == null && (_type = _type.BaseType) != null)
            {
                method = _type.GetMethod(_methodName, BindingFlags.NonPublic | BindingFlags.Instance| BindingFlags.DeclaredOnly);
            }

            return method;
        }

        #endregion

        #region GetAttirubte

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
        /// <summary> 保存类的字段的特性，在编译时重载 </summary>
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
            return TryGetFieldInfoAttribute(GetFieldInfo(_type, _fieldName), out _attribute);
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
        /// <summary> 保存类的方法的特性，在编译时重载 </summary>
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
            return TryGetMethodInfoAttribute(GetMethodInfo(_type, _methodName), out _attribute);
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
            return TryGetMethodInfoAttributes(GetMethodInfo(_type, _methodName), out _attributes);
        }
        #endregion

        #endregion
    }
}