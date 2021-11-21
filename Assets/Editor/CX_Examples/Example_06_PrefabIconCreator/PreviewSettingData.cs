//---------------------------------------------------------------------------------------
// Author: xuan_chen_work@foxmail.com
// Date: 2021-11-15 11:55:35
// Name: PreviewSettingData
//---------------------------------------------------------------------------------------

using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace Example_06_PrefabIconCreator
{
    [Serializable]
    public struct PreviewSettingData
    {
        public Color BgColor;
        public string BgGuid; 
        public string GroundGuid;
        public float GroundHeight;
        public float GroundScale;

        public Bounds Bounds;
        public Vector3 CenterOffSet;
        public float Distance;
        public float PitchAngle;
        public float StartAngle;
        public int Shots;

        public static PreviewSettingData GetDefaultData()
        {
            return new PreviewSettingData
            {
                PitchAngle = 25,
                StartAngle = -114,
                BgColor = Color.grey,
                GroundScale = 1,
                Shots = 15
            };
            
        }
    }
}