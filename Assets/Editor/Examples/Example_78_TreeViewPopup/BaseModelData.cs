
using System.IO;


    [System.Serializable]
    //基础数据
    public class BaseModelData : IModelData
    {
        public int Id;
        public string AssetName;
        public string ExtraDes;

        public virtual string GetAssetName()
        {
            return AssetName;
        }

        public virtual string GetShowName()
        {
            return Path.GetFileNameWithoutExtension(AssetName);
        }


        public virtual string GetExtraDes()
        {
            return ExtraDes;
        }
    }

