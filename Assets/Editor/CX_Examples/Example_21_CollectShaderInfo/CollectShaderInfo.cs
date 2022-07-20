using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Example_19_InternalBridge;
using UnityEditor;
using UnityEngine;

namespace CX_Example_21
{
    public class CollectShaderInfo : EditorWindow
    {
        [MenuItem("CX_Tools/ShowShaderInfo")]
        public static void OpenWindow()
        {
            var window = GetWindow<CollectShaderInfo>();
            window.titleContent = new GUIContent("ShowShaderInfo");
            window.Show();
        }

        private Shader _shader = null;
        private HashSet<string> _shaderFeatureKeys = new HashSet<string>();
        private HashSet<string> _multiCompileKeys = new HashSet<string>();
        private Vector2 _srollPos;
        private void OnGUI()
        {
            _shader = (Shader)EditorGUILayout.ObjectField(_shader, typeof(Shader), false);
            if (_shader != null && GUILayout.Button("Collect Shader Info"))
            {
                CollectShaderInfoFunc();
            }

            using (var scoll = new GUILayout.ScrollViewScope(_srollPos))
            {
                _srollPos = scoll.scrollPosition;
                GUILayout.Label("shader Feature Keys:");
                foreach (var key in _shaderFeatureKeys)
                {
                    GUILayout.Label(key);
                }
                GUILayout.Space(10f);
                GUILayout.Label("Multi Compile Keys:");
                foreach (var key in _multiCompileKeys)
                {
                    GUILayout.Label(key);
                }
            }
        }

        private void CollectShaderInfoFunc()
        {
            _shaderFeatureKeys.Clear();
            _multiCompileKeys.Clear();
            var regexShaderFeatures = new Regex(@"^Keywords stripped away when not used:(.+)$");
            var regexMultiComplies = new Regex(@"^Keywords always included into build:(.+)$");
            Match match;
            var combFilePath = string.Format("{0}/ParsedCombinations-{1}.shader", GetProjectUnityTempPath(),
                _shader.name.Replace('/', '-'));
            if (File.Exists(combFilePath))
            {
                File.Delete(combFilePath);
            }

            InternalShaderUtilBridge.OpenShaderCombinations(_shader, false);
            
            if(!File.Exists(combFilePath)) return;
            var lines = File.ReadAllLines(combFilePath);
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                if ((match = regexShaderFeatures.Match(line)).Success)
                {
                    string key = match.Groups[1].Value;
                    if(string.IsNullOrEmpty(key)) continue;
                    var arr = key.Trim().Split(' ');
                    foreach (var k in arr)
                    {
                        _shaderFeatureKeys.Add(k);
                    }
                }
                else if((match = regexMultiComplies.Match(line)).Success)
                {
                    string key = match.Groups[1].Value;
                    if(string.IsNullOrEmpty(key)) continue;
                    var arr = key.Trim().Split(' ');
                    foreach (var k in arr)
                    {
                        _multiCompileKeys.Add(k);
                    }
                }
            }
        }

        private string GetProjectUnityTempPath()
        {
            return Path.GetDirectoryName(Application.dataPath) + "/Temp";
        }

    }
}


