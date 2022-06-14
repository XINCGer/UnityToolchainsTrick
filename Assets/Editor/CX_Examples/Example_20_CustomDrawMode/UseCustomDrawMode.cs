/* Copyright 2018 Ming Wai Chan
https://cmwdexint.com/2018/07/01/custom-sceneview-drawmode/ */

#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditorInternal;

//====================== The custom draw modes =========================
//internal static readonly PrefColor kSceneViewWire = new PrefColor("Scene/Wireframe", 0.0f, 0.0f, 0.0f, 0.5f);
//https://github.com/Unity-Technologies/UnityCsReference/blob/master/Editor/Mono/Annotation/SceneRenderModeWindow.cs
//https://github.com/Unity-Technologies/UnityCsReference/blob/master/Editor/Mono/SceneView/SceneView.cs
//GameView? https://forum.unity.com/threads/creating-a-totally-custom-scene-editor-in-the-editor.118065/

[InitializeOnLoad]
public class UseCustomDrawMode
{
	private static Camera cam;
	private static bool delegateSceneView = false;

    static UseCustomDrawMode ()
    {
        EditorApplication.update += Update;
    }

	static void Update()
	{
		if(!delegateSceneView && SceneView.sceneViews.Count >0 )
		{
			SceneView.onSceneGUIDelegate += OnScene;
			delegateSceneView = true;
		}

		if(SceneView.sceneViews.Count == 0)
		{
			SceneView.onSceneGUIDelegate -= OnScene;
			delegateSceneView = false;
		}

	}

	private static void OnScene(SceneView sceneview)
     {
        RunDrawMode();
     }

	static bool AcceptedDrawMode(SceneView.CameraMode cameraMode)
	{
		// if (
		// 	cameraMode.drawMode == DrawCameraMode.Wireframe ||
		// 	cameraMode.drawMode == DrawCameraMode.TexturedWire ||
		// 	cameraMode.drawMode == DrawCameraMode.Textured ||
		// 	//cameraMode.drawMode == DrawCameraMode.Normal ||
		// 	cameraMode.drawMode == DrawCameraMode.UserDefined
		// 	)
		// 	return true;

		return true;
	}

    static void GetCurrentSceneCam()
    {
        if (SceneView.currentDrawingSceneView == null)
        {
            if (SceneView.lastActiveSceneView != null)
            {
                cam = SceneView.lastActiveSceneView.camera;
            }
        }
        else
        {
            cam = SceneView.currentDrawingSceneView.camera;
        }
    }

//******************************************* */

	static void RunDrawMode()
	{
		//Setup object
		if(!CustomDrawModeAssetObject.SetUpObject()) return;

		//Set camera
		GetCurrentSceneCam();

		//Setup draw mode
		SceneView.ClearUserDefinedCameraModes();
		for (int i=0; i<CustomDrawModeAssetObject.cdma.customDrawModes.Length; i++)
		{
			if(
				CustomDrawModeAssetObject.cdma.customDrawModes[i].name != "" && 
				CustomDrawModeAssetObject.cdma.customDrawModes[i].category != ""
			)
			SceneView.AddCameraMode(
			CustomDrawModeAssetObject.cdma.customDrawModes[i].name,
			CustomDrawModeAssetObject.cdma.customDrawModes[i].category);
		}
		ArrayList sceneViewArray = SceneView.sceneViews;
		foreach (SceneView sceneView in sceneViewArray)
		{
			sceneView.onValidateCameraMode -= AcceptedDrawMode; // Clean up
			sceneView.onValidateCameraMode += AcceptedDrawMode;
		}


		//Render with selected draw mode
		
		if(cam != null)
		{
			ArrayList sceneViewsArray = SceneView.sceneViews;
			foreach (SceneView sceneView in sceneViewsArray)
			{
				bool success = false;
				for (int i=0; i<CustomDrawModeAssetObject.cdma.customDrawModes.Length; i++)
				{
					if(CustomDrawModeAssetObject.cdma.customDrawModes[i].name != "")
					if(sceneView.cameraMode.name == CustomDrawModeAssetObject.cdma.customDrawModes[i].name)
					{
						//cam.SetReplacementShader(shader, ""); //this does nothing...
						if(CustomDrawModeAssetObject.cdma.customDrawModes[i].shader != null)
						cam.RenderWithShader(CustomDrawModeAssetObject.cdma.customDrawModes[i].shader, "");
						success = true;
						break;
					}
				}
				//If nothing can be found
				if (!success) cam.ResetReplacementShader();
			}
		}

	}
}
#endif