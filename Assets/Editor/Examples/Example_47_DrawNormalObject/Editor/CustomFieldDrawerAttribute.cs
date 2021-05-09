using System;

namespace CZToolKit.Core.Editors
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class CustomFieldDrawerAttribute : Attribute
    {
        Type type;

        public Type Type { get { return type; } }

        public CustomFieldDrawerAttribute(Type _type) { type = _type; }
    }
}