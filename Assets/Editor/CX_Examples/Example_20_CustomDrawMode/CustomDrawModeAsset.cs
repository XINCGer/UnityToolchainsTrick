/* Copyright 2018 Ming Wai Chan
https://cmwdexint.com/2018/07/01/custom-sceneview-drawmode/ */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization;
using System;
using UnityEditor;

[CreateAssetMenu(fileName = "CustomDrawModeAsset", menuName = "CustomDrawModeAsset", order = 1)]
public class CustomDrawModeAsset : ScriptableObject 
{
	[Serializable]
	public struct CustomDrawMode
	{
		public string name;
		public string category;
		public Shader shader;
	}
	public CustomDrawMode[] customDrawModes ;
}

public static class CustomDrawModeAssetObject
{
	public static CustomDrawModeAsset cdma;

	public static bool SetUpObject()
	{
		if(cdma == null)
		{
			cdma = (CustomDrawModeAsset)AssetDatabase.LoadAssetAtPath("Assets/Editor/CX_Examples/Example_20_CustomDrawMode/CustomDrawModeAsset.asset", typeof(CustomDrawModeAsset));
		}
		if(cdma == null) return false; else return true;
	}
}
