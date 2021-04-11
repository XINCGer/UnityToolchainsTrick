using UnityEngine;
using UnityEditor;

namespace Beans.Unity.ETE
{
	[CustomEditor (typeof (Transform)), CanEditMultipleObjects]
	public class ExtendedTransformEditor : Editor
	{
		private class Content
		{
			public static Texture2D ResetTexture = AssetDatabasex.LoadAssetOfType<Texture2D> (EditorGUIUtility.isProSkin ? "ETE_Pro_Reset" : "ETE_Personal_Reset");

			public static readonly GUIContent Position	= new GUIContent ("Position", "The local position of this GameObject relative to the parent.");
			public static readonly GUIContent Rotation	= new GUIContent ("Rotation", "The local rotation of this Game Object relative to the parent.");
			public static readonly GUIContent Scale		= new GUIContent ("Scale", "The local scaling of this GameObject relative to the parent.");
			public static readonly GUIContent ResetPosition = new GUIContent (ResetTexture, "Reset the position.");
			public static readonly GUIContent ResetRotation = new GUIContent (ResetTexture, "Reset the rotation.");
			public static readonly GUIContent ResetScale = new GUIContent (ResetTexture, "Reset the scale.");

			public const string FloatingPointWarning = "Due to floating-point precision limitations, it is recommended to bring the world coordinates of the GameObject within a smaller range.";
		}

		private class Styles
		{
			public static GUIStyle ResetButton;

			static Styles ()
			{
				ResetButton = new GUIStyle ()
				{
					margin = new RectOffset (0, 0, 2, 0),
					fixedWidth = 15,
					fixedHeight = 15
				};
			}
		}

		private class Properties
		{
			public SerializedProperty Position;
			public SerializedProperty Rotation;
			public SerializedProperty Scale;

			public Properties (SerializedObject obj)
			{
				Position	= obj.FindProperty ("m_LocalPosition");
				Rotation	= obj.FindProperty ("m_LocalRotation");
				Scale		= obj.FindProperty ("m_LocalScale");
			}
		}

		private const int MaxDistanceFromOrigin = 100000;
		private const int ContentWidth = 60;

		private float xyRatio, xzRatio;

		private Properties properties;
		private TransformRotationGUI rotationGUI;
		private Transform transform;

		private void OnEnable ()
		{
			properties = new Properties (serializedObject);

			if (rotationGUI == null)
				rotationGUI = new TransformRotationGUI ();
			rotationGUI.Initialize (properties.Rotation, Content.Rotation);
		}

		public override void OnInspectorGUI ()
		{
			transform = target as Transform;
			
			if (!EditorGUIUtility.wideMode)
			{
				EditorGUIUtility.wideMode = true;
				EditorGUIUtility.labelWidth = EditorGUIUtility.currentViewWidth - 212;
			}

			serializedObject.UpdateIfRequiredOrScript ();

			using (new EditorGUILayout.HorizontalScope ())
			{
				EditorGUILayout.PropertyField (properties.Position, Content.Position);
				using (new EditorGUI.DisabledGroupScope (properties.Position.vector3Value == Vector3.zero))
					if (GUILayout.Button (Content.ResetPosition, Styles.ResetButton))
						properties.Position.vector3Value = Vector3.zero;
			}
			using (new EditorGUILayout.HorizontalScope ())
			{
				rotationGUI.Draw ();
				using (new EditorGUI.DisabledGroupScope (rotationGUI.eulerAngles == Vector3.zero))
					if (GUILayout.Button (Content.ResetRotation, Styles.ResetButton))
					{
						rotationGUI.Reset ();
						if (Tools.current == Tool.Rotate)
						{
							if (Tools.pivotRotation == PivotRotation.Global)
							{
								Tools.handleRotation = Quaternion.identity;
								SceneView.RepaintAll ();
							}
						}
					}
			}
			using (new EditorGUILayout.HorizontalScope ())
			{
				EditorGUILayout.PropertyField (properties.Scale, Content.Scale);
				using (new EditorGUI.DisabledGroupScope (properties.Scale.vector3Value == Vector3.one))
					if (GUILayout.Button (Content.ResetScale, Styles.ResetButton))
						properties.Scale.vector3Value = Vector3.one;
			}

			// I can hard code this b/c the transform inspector is always drawn in the same spot lmao
			var dragRect = new Rect (16, 105, EditorGUIUtility.labelWidth - 10, 10);

			var e = Event.current;
			if (dragRect.Contains (e.mousePosition) && e.type == EventType.MouseDown && e.button == 0)
			{
				var currentScale = properties.Scale.vector3Value;
				xyRatio = currentScale.y / currentScale.x;
				xzRatio = currentScale.z / currentScale.x;
			}

			using (var check = new EditorGUI.ChangeCheckScope ())
			{
				var c = GUI.color;
				GUI.color = Color.clear;
				var newScaleX = CustomFloatField.Draw (new Rect (), dragRect, properties.Scale.vector3Value.x, EditorStyles.numberField);

				if (check.changed)
				{
					var currentScale = properties.Scale.vector3Value;

					var delta = newScaleX - properties.Scale.vector3Value.x;

					currentScale.x += delta;
					currentScale.y += delta * xyRatio;
					currentScale.z += delta * xzRatio;
					
					properties.Scale.vector3Value = currentScale;
				}

				GUI.color = c;
			}

			#region Copy

			EditorGUILayout.BeginHorizontal();
            GUILayout.Label("复制坐标：",GUILayout.Width(60));
            if (GUILayout.Button("X"))
            {
                TextEditor textEd = new TextEditor();
                TransformCopyData copyData = new TransformCopyData();
                copyData.PosX = transform.position.x;
                copyData.CopyPosX = true;
                textEd.text = copyData.ToString();
                textEd.OnFocus();
                textEd.Copy();
            }
            if (GUILayout.Button("Y"))
            {
                TextEditor textEd = new TextEditor();
                TransformCopyData copyData = new TransformCopyData();
                copyData.PosY = transform.position.y;
                copyData.CopyPosY = true;
                textEd.text = copyData.ToString();
                textEd.OnFocus();
                textEd.Copy();
            }
            if (GUILayout.Button("Z"))
            {
                TextEditor textEd = new TextEditor();
                TransformCopyData copyData = new TransformCopyData();
                copyData.PosZ = transform.position.z;
                copyData.CopyPosZ = true;
                textEd.text = copyData.ToString();
                textEd.OnFocus();
                textEd.Copy();
            }
            if (GUILayout.Button("X-Y"))
            {
                TextEditor textEd = new TextEditor();
                TransformCopyData copyData = new TransformCopyData();
                copyData.PosX = transform.position.x;
                copyData.PosY = transform.position.y;
                copyData.CopyPosX = true;
                copyData.CopyPosY = true;
                textEd.text = copyData.ToString();
                textEd.OnFocus();
                textEd.Copy();
            }
            if (GUILayout.Button("X-Z"))
            {
                TextEditor textEd = new TextEditor();
                TransformCopyData copyData = new TransformCopyData();
                copyData.PosX = transform.position.x;
                copyData.PosZ = transform.position.z;
                copyData.CopyPosX = true;
                copyData.CopyPosZ = true;
                textEd.text = copyData.ToString();
                textEd.OnFocus();
                textEd.Copy();
            }
            if (GUILayout.Button("Y-Z"))
            {
                TextEditor textEd = new TextEditor();
                TransformCopyData copyData = new TransformCopyData();
                copyData.PosY = transform.position.y;
                copyData.PosZ = transform.position.z;
                copyData.CopyPosY = true;
                copyData.CopyPosZ = true;
                textEd.text = copyData.ToString();
                textEd.OnFocus();
                textEd.Copy();
            }
            if (GUILayout.Button("X-Y-Z"))
            {
                TextEditor textEd = new TextEditor();
                TransformCopyData copyData = new TransformCopyData();
                copyData.PosX = transform.position.x;
                copyData.PosY = transform.position.y;
                copyData.PosZ = transform.position.z;
                copyData.CopyPosX = true;
                copyData.CopyPosY = true;
                copyData.CopyPosZ = true;
                textEd.text = copyData.ToString();
                textEd.OnFocus();
                textEd.Copy();
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("复制旋转：",GUILayout.Width(60));
            if (GUILayout.Button("X"))
            {
                TextEditor textEd = new TextEditor();
                TransformCopyData copyData = new TransformCopyData();
                copyData.RotationX = transform.rotation.eulerAngles.x;
                copyData.CopyRotationX = true;
                textEd.text = copyData.ToString();
                textEd.OnFocus();
                textEd.Copy();
            }
            if (GUILayout.Button("Y"))
            {
                TextEditor textEd = new TextEditor();
                TransformCopyData copyData = new TransformCopyData();
                copyData.RotationY = transform.rotation.eulerAngles.y;
                copyData.CopyRotationY = true;
                textEd.text = copyData.ToString();
                textEd.OnFocus();
                textEd.Copy();
            }
            if (GUILayout.Button("Z"))
            {
                TextEditor textEd = new TextEditor();
                TransformCopyData copyData = new TransformCopyData();
                copyData.RotationZ = transform.rotation.eulerAngles.z;
                copyData.CopyRotationZ = true;
                textEd.text = copyData.ToString();
                textEd.OnFocus();
                textEd.Copy();
            }
            if (GUILayout.Button("X-Y"))
            {
                TextEditor textEd = new TextEditor();
                TransformCopyData copyData = new TransformCopyData();
                copyData.RotationX = transform.rotation.eulerAngles.x;
                copyData.RotationY = transform.rotation.eulerAngles.y;
                copyData.CopyRotationX = true;
                copyData.CopyRotationY = true;
                textEd.text = copyData.ToString();
                textEd.OnFocus();
                textEd.Copy();
            }
            if (GUILayout.Button("X-Z"))
            {
                TextEditor textEd = new TextEditor();
                TransformCopyData copyData = new TransformCopyData();
                copyData.RotationX = transform.rotation.eulerAngles.x;
                copyData.RotationZ = transform.rotation.eulerAngles.z;
                copyData.CopyRotationX = true;
                copyData.CopyRotationZ = true;
                textEd.text = copyData.ToString();
                textEd.OnFocus();
                textEd.Copy();
            }
            if (GUILayout.Button("Y-Z"))
            {
                TextEditor textEd = new TextEditor();
                TransformCopyData copyData = new TransformCopyData();
                copyData.RotationY = transform.rotation.eulerAngles.y;
                copyData.RotationZ = transform.rotation.eulerAngles.z;
                copyData.CopyRotationY = true;
                copyData.CopyRotationZ = true;
                textEd.text = copyData.ToString();
                textEd.OnFocus();
                textEd.Copy();
            }
            if (GUILayout.Button("X-Y-Z"))
            {
                TextEditor textEd = new TextEditor();
                TransformCopyData copyData = new TransformCopyData();
                copyData.RotationX = transform.rotation.eulerAngles.x;
                copyData.RotationY = transform.rotation.eulerAngles.y;
                copyData.RotationZ = transform.rotation.eulerAngles.z;
                copyData.CopyRotationX = true;
                copyData.CopyRotationY = true;
                copyData.CopyRotationZ = true;
                textEd.text = copyData.ToString();
                textEd.OnFocus();
                textEd.Copy();
            }
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("复制坐标和旋转"))
            {
                TextEditor textEd = new TextEditor();
                TransformCopyData copyData = new TransformCopyData();
                copyData.PosX = transform.position.x;
                copyData.PosY = transform.position.y;
                copyData.PosZ = transform.position.z;
                copyData.CopyPosX = true;
                copyData.CopyPosY = true;
                copyData.CopyPosZ = true;
                copyData.RotationX = transform.rotation.eulerAngles.x;
                copyData.RotationY = transform.rotation.eulerAngles.y;
                copyData.RotationZ = transform.rotation.eulerAngles.z;
                copyData.CopyRotationX = true;
                copyData.CopyRotationY = true;
                copyData.CopyRotationZ = true;
                textEd.text = copyData.ToString();
                textEd.OnFocus();
                textEd.Copy();
            }
            if (GUILayout.Button("粘贴"))
            {
                TransformCopyData transformCopyData = new TransformCopyData(GUIUtility.systemCopyBuffer);
                if (transformCopyData != null)
                {
                    Transform[] transforms = Selection.transforms;
                    for (int i = 0; i < transforms.Length; i++)
                    {
                        Transform trans = transforms[i];
                        trans.position = transformCopyData.GetCopyPos(trans.position);
                        trans.rotation = Quaternion.Euler(transformCopyData.GetCopyRotation(trans.rotation.eulerAngles));
                    }
                }
                EditorUtility.SetDirty(target);
            }
            EditorGUILayout.EndHorizontal();

			#endregion
			
			serializedObject.ApplyModifiedProperties ();

			EditorGUIUtility.labelWidth = 0;

			var position = transform.position;

			if
			(
				Mathf.Abs (position.x) > MaxDistanceFromOrigin ||
				Mathf.Abs (position.y) > MaxDistanceFromOrigin ||
				Mathf.Abs (position.z) > MaxDistanceFromOrigin
			)
				EditorGUILayout.HelpBox (Content.FloatingPointWarning, MessageType.Warning);
		}

		[MenuItem ("CONTEXT/Transform/Set Random Rotation")]
		private static void RandomRotation (MenuCommand command)
		{
			var transform = command.context as Transform;

			Undo.RecordObject (transform, "Set Random Rotation");
			transform.rotation = Random.rotation;
		}

		[MenuItem ("CONTEXT/Transform/Snap to Ground")]
		private static void SnapToGround (MenuCommand command)
		{
			var transform = command.context as Transform;

			RaycastHit hit;
			if (Physics.Raycast (transform.position, Vector3.down, out hit))
			{
				Undo.RecordObject (transform, "Snapped To Ground");
				transform.position = hit.point;
			}
		}

		[MenuItem ("CONTEXT/Transform/Snap to Ground (Physics)", true)]
		private static bool ValidateSnapToGroundPhysics (MenuCommand command)
		{
			return ((Transform)command.context).GetComponent<Collider> () != null;
		}
	}
}