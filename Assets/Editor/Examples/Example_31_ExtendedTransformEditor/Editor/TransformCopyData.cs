using UnityEngine;

namespace Beans.Unity.ETE
{
    public class TransformCopyData
    {
        public float PosX = 0;
        public float PosY = 0;
        public float PosZ = 0;

        public float RotationX = 0;
        public float RotationY = 0;
        public float RotationZ = 0;

        public bool CopyPosX = false;
        public bool CopyPosY = false;
        public bool CopyPosZ = false;

        public bool CopyRotationX = false;
        public bool CopyRotationY = false;
        public bool CopyRotationZ = false;

        public TransformCopyData(string str)
        {
            string[] dataArray = str.Split('|');
            if (dataArray.Length == 4)
            {
                string[] posArray = dataArray[0].Split(',');
                PosX = float.Parse(posArray[0]);
                PosY = float.Parse(posArray[1]);
                PosZ = float.Parse(posArray[2]);

                string[] copyPosArray = dataArray[1].Split(',');
                CopyPosX = bool.Parse(copyPosArray[0]);
                CopyPosY = bool.Parse(copyPosArray[1]);
                CopyPosZ = bool.Parse(copyPosArray[2]);

                string[] rotation = dataArray[2].Split(',');
                RotationX = float.Parse(rotation[0]);
                RotationY = float.Parse(rotation[1]);
                RotationZ = float.Parse(rotation[2]);

                string[] copyRotationArray = dataArray[3].Split(',');
                CopyRotationX = bool.Parse(copyRotationArray[0]);
                CopyRotationY = bool.Parse(copyRotationArray[1]);
                CopyRotationZ = bool.Parse(copyRotationArray[2]);
            }
        }
        
        public TransformCopyData(){}
        
        public Vector3 GetCopyPos(Vector3 originPos)
        {
            return new Vector3(
                CopyPosX ? PosX : originPos.x,
                CopyPosY ? PosY : originPos.y,
                CopyPosZ ? PosZ : originPos.z);
        }
        
        public Vector3 GetCopyRotation(Vector3 originRotation)
        {
            return new Vector3(
                CopyRotationX ? RotationX : originRotation.x,
                CopyRotationY ? RotationY : originRotation.y,
                CopyRotationZ ? RotationZ : originRotation.z);
        }

        public override string ToString()
        {
            return $"{PosX},{PosY},{PosZ}|{CopyPosX},{CopyPosY},{CopyPosZ}|{RotationX},{RotationX},{RotationX}|{CopyRotationX},{CopyRotationX},{CopyRotationX}";
        }
    }
}