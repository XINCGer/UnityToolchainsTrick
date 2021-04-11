using System;
using System.Reflection;
using UnityEngine;
using UnityEditor;

namespace Beans.Unity.ETE
{
	/// <summary>
	/// Abstracts the reflection required to use the internal TransformRotationGUI class.
	/// </summary>
	public class TransformRotationGUI
	{
		private object transformRotationGUI;
		private FieldInfo eulerAnglesField;
		private MethodInfo onEnableMethod;
		private MethodInfo rotationFieldMethod;
		private MethodInfo setLocalEulerAnglesMethod;

		private SerializedProperty property;

		public Vector3 eulerAngles => (Vector3)eulerAnglesField.GetValue (transformRotationGUI);

		public TransformRotationGUI ()
		{
			if (transformRotationGUI == null)
			{
				var transformRotationGUIType = Type.GetType ("UnityEditor.TransformRotationGUI,UnityEditor");
				var transformType = typeof (Transform);

				eulerAnglesField = transformRotationGUIType.GetField ("m_EulerAngles", BindingFlags.Instance | BindingFlags.NonPublic);
				onEnableMethod = transformRotationGUIType.GetMethod ("OnEnable");
				rotationFieldMethod = transformRotationGUIType.GetMethod ("RotationField", new Type[] { });
				setLocalEulerAnglesMethod = transformType.GetMethod ("SetLocalEulerAngles", BindingFlags.Instance | BindingFlags.NonPublic);

				transformRotationGUI = Activator.CreateInstance (transformRotationGUIType);
			}
		}

		/// <summary>
		/// Initializes the GUI.
		/// </summary>
		/// <param name="property">A serialized quaternion property.</param>
		/// <param name="content">The content to draw the property with.</param>
		public void Initialize (SerializedProperty property, GUIContent content)
		{
			this.property = property;
			onEnableMethod.Invoke (transformRotationGUI, new object[] { property, content });
		}

		/// <summary>
		/// Draws the rotation GUI.
		/// </summary>
		public void Draw ()
		{
			rotationFieldMethod.Invoke (transformRotationGUI, null);
		}

		public void Reset ()
		{
			var targets = property.serializedObject.targetObjects;
			var parameters = new object[] { Vector3.zero, 0 };

			Undo.RecordObjects (targets, "Reset Rotation");
			foreach (var target in targets)
				setLocalEulerAnglesMethod.Invoke (target, parameters);
		}
	}
}