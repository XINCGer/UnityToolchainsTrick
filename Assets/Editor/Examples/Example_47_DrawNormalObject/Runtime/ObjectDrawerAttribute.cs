using System;

namespace CZToolKit.Core
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Class | AttributeTargets.Interface, Inherited = true, AllowMultiple = false)]
    public abstract class ObjectDrawerAttribute : Attribute { }
}
