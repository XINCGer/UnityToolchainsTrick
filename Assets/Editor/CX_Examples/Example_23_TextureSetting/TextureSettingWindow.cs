using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Example_23
{
    public class TextureSettingWindow : EditorWindow
    {
        private string[] _targets;
        
        [MenuItem("Assets/ArtUtils/SetAllTexture")]
        private static void ShowWindow()
        {
            var select = Selection.activeObject;
            var path = AssetDatabase.GetAssetPath(select);
            if (!Directory.Exists(path))
            {
                EditorUtility.DisplayDialog("Fail","不是文件夹","ok");
                return;
            }
            var targets = AssetDatabase.FindAssets("t:Texture2D", new string[] {path});
            if (targets.Length <= 0)
            {
                EditorUtility.DisplayDialog("Fail","文件夹里不存在图片","ok");
                return;
            }
            var window = GetWindow<TextureSettingWindow>();
            window._targets = targets;
            window.titleContent = new GUIContent("Set All Texture");
            window.Show();
        }

        private List<TextureSetting> _settings;
        private int _anisoLevel = 1;

        private void OnEnable()
        {
            _settings = new  List<TextureSetting>
            {
                new TextureSetting
                {
                    TargetName = "Standalone",
                    HasAlphaNormalMapFormat = TextureImporterFormat.DXT5,
                    DefaultFormat = TextureImporterFormat.DXT1,
                },
                new TextureSetting
                {
                    TargetName = "iPhone",
                    HasAlphaNormalMapFormat = TextureImporterFormat.ASTC_4x4,
                    DefaultFormat = TextureImporterFormat.ASTC_6x6,
                },
                new TextureSetting
                {
                    TargetName = "Android",
                    HasAlphaNormalMapFormat = TextureImporterFormat.ASTC_4x4,
                    DefaultFormat = TextureImporterFormat.ASTC_6x6,
                },
            };
        }

        private void OnGUI()
        {
            using (new GUILayout.VerticalScope("Box"))
            {
                _anisoLevel = EditorGUILayout.IntSlider("anisoLevel",_anisoLevel, 0, 16);
                
                GUILayout.Label("Setting Compressed:");
                foreach (var setting in _settings)
                {
                    GUILayout.Label($"Platform: {setting.TargetName}");
                    setting.DefaultFormat= (TextureImporterFormat)EditorGUILayout.EnumPopup("DefaultType_Format:", setting.DefaultFormat);
                    setting.HasAlphaNormalMapFormat= (TextureImporterFormat)EditorGUILayout.EnumPopup("HasAlpha&NormalType_Format:", setting.HasAlphaNormalMapFormat);
                    GUILayout.Space(10f);
                }
            }
            
            if (GUILayout.Button("Set all Texture") && EditorUtility.DisplayDialog("Set All Texture", "确定将所有Texture都按此设置吗？", "OK", "Cancel"))
            {
                for (int i = 0; i < _targets.Length; i++)
                {
                    var path = AssetDatabase.GUIDToAssetPath(_targets[i]);
                    var imp = AssetImporter.GetAtPath(path) as TextureImporter;
                    var hasAlpha = imp.DoesSourceTextureHaveAlpha();
                    imp.anisoLevel = _anisoLevel;
                    foreach (var setting in _settings)
                    {
                        var set = imp.GetPlatformTextureSettings(setting.TargetName);
                        set.overridden = true;
                        switch (imp.textureType)
                        {
                            case TextureImporterType.NormalMap:
                                set.format = setting.HasAlphaNormalMapFormat;
                                break;
                            default:
                                set.format = hasAlpha &&  setting.TargetName == "Standalone"? setting.HasAlphaNormalMapFormat : setting.DefaultFormat;
                                break;
                        }
                        imp.SetPlatformTextureSettings(set);
                    }
                    AssetDatabase.ImportAsset(path);
                }

                
            }

        }
        
        
    }

    public class TextureSetting
    {
        public string TargetName;
        public TextureImporterFormat HasAlphaNormalMapFormat;
        public TextureImporterFormat DefaultFormat;
    }
}