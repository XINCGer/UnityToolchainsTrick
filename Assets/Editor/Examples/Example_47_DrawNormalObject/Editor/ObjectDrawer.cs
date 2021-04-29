using System.Reflection;
using UnityEngine;

namespace CZToolKit.Core.Editors
{

    public class ObjectDrawer
    {
        protected FieldInfo fieldInfo;

        protected ObjectDrawerAttribute attribute;

        protected object value;

        public FieldInfo FieldInfo
        {
            get { return this.fieldInfo; }
            set { this.fieldInfo = value; }
        }

        public ObjectDrawerAttribute Attribute
        {
            get { return this.attribute; }
            set { this.attribute = value; }
        }

        public object Value
        {
            get { return this.value; }
            set { this.value = value; }
        }

        public virtual void OnGUI(GUIContent label)
        {
        }
    }
}