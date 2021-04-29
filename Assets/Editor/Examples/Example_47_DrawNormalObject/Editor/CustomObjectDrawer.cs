using System;

namespace CZToolKit.Core.Editors
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class CustomObjectDrawer : Attribute
    {
        Type type;

        public Type Type { get { return type; } }

        public CustomObjectDrawer(Type _type) { type = _type; }
    }
}