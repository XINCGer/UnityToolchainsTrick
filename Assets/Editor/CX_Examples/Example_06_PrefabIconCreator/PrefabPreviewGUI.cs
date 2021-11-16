//---------------------------------------------------------------------------------------
// Author: xuan_chen_work@foxmail.com
// Date: 2021-11-15 11:40:14
// Name: PrefabPreviewUIElement
//---------------------------------------------------------------------------------------

using System;
using System.IO;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;
using Object = UnityEngine.Object;

namespace Example_06_PrefabIconCreator
{
    public class PrefabPreviewGUI
    {
        public static readonly Vector2 WindowSize = new Vector2 {x = 420, y = 600};

        private const int Width = 800;
        private const int Height = 500;
        private const float Aspect = (float) Width / Height;
        private const int Iconwidth = 400;
        private const int IconHeight = 250;
        
        public VisualElement PreviewElement { get; private set; }
        private PreviewSettingData _setting;
        private Camera _renderCam;
        private GameObject _prefab;
        private GameObject _obj;
        private GameObject _groundObj;
        private GameObject _bgObj;
        private Material _bgMat;
        private bool _isDrity;

        #region UIElement
        private Image _previewImg;
        private Image _centerPoint;
        private Button _resetBtn;
        private ColorField _bgColorField;
        private ObjectField _bgTexField;
        private ObjectField _groundField;
        private FloatField _groundHeight;
        private FloatField _groundScale;
        private Vector3Field _centerOffsetField;
        private FloatField _distanceField;
        private FloatField _pitchAngleField;
        private FloatField _startAngleField;
        private Button _exportBtn;
        private IntegerField _shotsField;
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Obj"></param>
        /// <param name="RenderCam"></param>
        public PrefabPreviewGUI(Object prefab, Camera renderCam)
        {
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/CX_Examples/Example_06_PrefabIconCreator/UXML/PreviewWindow.uxml");
            PreviewElement = visualTree.Instantiate();
            PreviewElement.style.minWidth = WindowSize.x;
            PreviewElement.style.minHeight = WindowSize.y;
            PreviewElement.style.maxWidth = WindowSize.x;
            PreviewElement.style.minHeight = WindowSize.y;

            #region 找出Element
            _previewImg = PreviewElement.Q<Image>("PreviewImage");
            _centerPoint = _previewImg.Q<Image>("CenterPoint");
            _centerPoint.visible = false;
            _resetBtn = PreviewElement.Q<Button>("ResetBtn");
            _bgColorField = PreviewElement.Q<ColorField>("BgColorField");
            _bgTexField = PreviewElement.Q<ObjectField>("BgTex");
            _groundField = PreviewElement.Q<ObjectField>("GroundObj");
            _groundHeight = PreviewElement.Q<FloatField>("GroundHeight");
            _groundScale = PreviewElement.Q<FloatField>("GroundScale");
            _centerOffsetField = PreviewElement.Q<Vector3Field>("CenterOffSet");
            _distanceField = PreviewElement.Q<FloatField>("Distance");
            _pitchAngleField = PreviewElement.Q<FloatField>("PitchAngle");
            _startAngleField = PreviewElement.Q<FloatField>("StartAngle");
            _exportBtn = PreviewElement.Q<Button>("ExportBtn");
            _shotsField = PreviewElement.Q<IntegerField>("Shots");
            #endregion
            
            //设置初始值
            _renderCam = renderCam;
            _prefab = prefab as GameObject;
            if(_prefab == null) return;
            _obj = GameObject.Instantiate(_prefab, Vector3.one, Quaternion.identity);
            RegisterCallBack();
            ResetSetting();
        }

        private void ResetSetting()
        {
            var setting = PrefabPreview.CreateDefaultPreviewSetting(_obj, _renderCam, Aspect);
            InputSetting(setting);
            _isDrity = true;
        }
        
        private void InputSetting(PreviewSettingData setting)
        {
            _setting = setting;
            _bgColorField.value = setting.BgColor;
            var texPath = AssetDatabase.GUIDToAssetPath(setting.BgGuid);
            if (string.IsNullOrEmpty(texPath))
                _bgTexField.value = null;
            else
                _bgTexField.value = AssetDatabase.LoadAssetAtPath<Texture2D>(texPath);
            
            var groundPath = AssetDatabase.GUIDToAssetPath(setting.GroundGuid);
            if (string.IsNullOrEmpty(groundPath))
                _groundField.value = null;
            else
                _groundField.value = AssetDatabase.LoadAssetAtPath<GameObject>(groundPath);

            _groundHeight.value = setting.GroundHeight;
            _groundScale.value = setting.GroundScale;
            _centerOffsetField.value = setting.CenterOffSet;
            _distanceField.value = setting.Distance;
            _pitchAngleField.value = setting.PitchAngle;
            _startAngleField.value = setting.StartAngle;
            _shotsField.value = setting.Shots;
        }

        private void RegisterCallBack()
        {
            _resetBtn.RegisterCallback<ClickEvent>(OnReset);
            _previewImg.RegisterCallback<MouseDownEvent>(OnImgMouseDown);
            _previewImg.RegisterCallback<MouseUpEvent>(OnImgMouseUp);
            _previewImg.RegisterCallback<MouseLeaveEvent>(OnImgMouseExit);
            _previewImg.RegisterCallback<WheelEvent>(OnImgZoomWheel);
            _previewImg.RegisterCallback<MouseMoveEvent>(OnOperator);
            _bgColorField.RegisterValueChangedCallback(OnBgColorChange);
            _distanceField.RegisterValueChangedCallback(OnDistanceChange);
            _pitchAngleField.RegisterValueChangedCallback(OnPitchAngleChange);
            _startAngleField.RegisterValueChangedCallback(OnStartAngleChange);
            _centerOffsetField.RegisterValueChangedCallback(OnOffsetChange);
            _groundField.RegisterValueChangedCallback(OnGroundObjChange);
            _groundHeight.RegisterValueChangedCallback(OnGroundHeightChange);
            _groundScale.RegisterValueChangedCallback(OnGroundScaleChange);
            _bgTexField.RegisterValueChangedCallback(OnBgTexChange);
            _exportBtn.RegisterCallback<ClickEvent>(OnExport);
            _shotsField.RegisterValueChangedCallback(OnShotsChange);
        }

        private bool _isMove;
        private bool _isMid;
        private void OnImgMouseDown(MouseDownEvent evn)
        {
            _centerPoint.visible = true;
            _isMove = true;
            _isMid = evn.button == 2;
        }

        private void OnImgMouseUp(MouseUpEvent evn)
        {
            _centerPoint.visible = false;
            _isMove = false;
            _isMid = false;
        }

        private void OnImgMouseExit(MouseLeaveEvent evn)
        {
            _centerPoint.visible = false;
            _isMove = false;
            _isMid = false;
        }
        
        private void OnOperator(MouseMoveEvent evn)
        {
            if (!_isMove) return;
            if (_isMid)
            {
                var speed = 0.01f;
                if (Event.current.shift)
                    speed = 0.1f;
                var offset =  _centerOffsetField.value;
                offset.y += evn.mouseDelta.y * speed;
                offset.x -= _renderCam.transform.right.x * evn.mouseDelta.x * speed;
                offset.z -= _renderCam.transform.right.z * evn.mouseDelta.x * speed;
                _centerOffsetField.value = offset;
            }
            else
            {
                var pitchAngle = _pitchAngleField.value + evn.mouseDelta.y;
                var startAngle = _startAngleField.value + evn.mouseDelta.x;
                
                _pitchAngleField.value = pitchAngle;
                _startAngleField.value = startAngle;
            }
        }
        
        private void OnImgZoomWheel(WheelEvent evn)
        {
            var speed = 0.01f;
            if (Event.current.shift)
                speed = 0.1f;
            _distanceField.value += evn.delta.y * speed;
        }

        private void OnBgColorChange(ChangeEvent<Color> evn)
        {
            _setting.BgColor = evn.newValue;
            _isDrity = true;
        }

        private void OnGroundObjChange(ChangeEvent<Object> evn)
        {
            if(_groundObj != null)
                Object.DestroyImmediate(_groundObj);
            _isDrity = true;
            if(evn.newValue == null) return;
            var path = AssetDatabase.GetAssetPath(evn.newValue);
            _setting.GroundGuid = AssetDatabase.GUIDFromAssetPath(path).ToString();
            var pos = _setting.Bounds.center;
            pos.y -= _setting.Bounds.extents.y + _setting.GroundHeight;
            _groundObj = Object.Instantiate(evn.newValue,pos, Quaternion.identity) as GameObject;
            _groundObj.transform.localScale = Vector3.one * _groundScale.value;
        }

        private void OnBgTexChange(ChangeEvent<Object> evn)
        {
            if (evn.newValue == null)
            {
                if (_bgObj != null)
                {
                    _bgObj.SetActive(false);
                    _isDrity = true;
                }
                return;
            }

            if (_bgObj == null)
            {
                _bgObj = GameObject.CreatePrimitive(PrimitiveType.Quad);
                _bgObj.transform.parent = _renderCam.transform;
                _bgObj.transform.localRotation = Quaternion.identity;
                _bgObj.transform.localPosition = Vector3.forward * _renderCam.farClipPlane;
                _bgMat = AssetDatabase.LoadAssetAtPath<Material>(
                    "Assets/Editor/CX_Examples/Example_06_PrefabIconCreator/Bg_Mat.mat");
                _bgObj.GetComponent<MeshRenderer>().sharedMaterial = _bgMat;
                var height = 2 * _renderCam.farClipPlane * Mathf.Tan(_renderCam.fieldOfView * 0.5f * Mathf.Deg2Rad);
                var scale = _bgObj.transform.localScale;
                scale.x *= height * Aspect;
                scale.y *= height;
                _bgObj.transform.localScale = scale;
            }
            _bgObj.SetActive(true);
            var path = AssetDatabase.GetAssetPath(evn.newValue);
            _setting.BgGuid = AssetDatabase.GUIDFromAssetPath(path).ToString();
            var tex = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
            _bgMat.SetTexture("_MainTex", tex);
            _isDrity = true;
        }

        private void OnGroundScaleChange(ChangeEvent<float> evn)
        {
            _setting.GroundScale = evn.newValue;
            if (_groundObj != null)
            {
                _groundObj.transform.localScale = Vector3.one * _setting.GroundScale;
                _isDrity = true;
            }
        }
        
        private void OnGroundHeightChange(ChangeEvent<float> evn)
        {
            _setting.GroundHeight = evn.newValue;
            if (_groundObj != null)
            {
                var pos = _setting.Bounds.center;
                pos.y -= _setting.Bounds.extents.y + _setting.GroundHeight;
                _groundObj.transform.position = pos;
                _isDrity = true;
            }
        }

        private void OnDistanceChange(ChangeEvent<float> evn)
        {
            _setting.Distance = evn.newValue;
            _isDrity = true;
        }

        private void OnPitchAngleChange(ChangeEvent<float> evn)
        {
            _pitchAngleField.value = Mathf.Clamp(evn.newValue, -90f, 90f);
            _setting.PitchAngle = _pitchAngleField.value;
            _isDrity = true;
        }
        private void OnStartAngleChange(ChangeEvent<float> evn)
        {
            _startAngleField.value = evn.newValue % 360;
            _setting.StartAngle = _startAngleField.value;
            _isDrity = true;
        }

        private void OnOffsetChange(ChangeEvent<Vector3> evn)
        {
            _setting.CenterOffSet = evn.newValue;
            _isDrity = true;
        }

        private void OnReset(ClickEvent evn)
        {
            ResetSetting();
        }

        private void OnShotsChange(ChangeEvent<int> evn)
        {
            _setting.Shots = evn.newValue;
        }

        public void RefreshPreview()
        {
            if(!_isDrity || _obj == null) return;
            _previewImg.image = PrefabPreview.CreatePreviewTexture(_renderCam, _setting, Width, Height);
            _isDrity = false;
        }

        /// <summary>
        /// 输出一张Icon，一张连图和一个设置json
        /// </summary>
        /// <param name="evn"></param>
        private void OnExport(ClickEvent evn)
        {
            if(_obj == null) return;
            var floderPath = EditorUtility.OpenFolderPanel("Export", "Assets", string.Empty);
            if(string.IsNullOrEmpty(floderPath)) return;
            var iconTex = PrefabPreview.CreatePreviewTexture(_renderCam, _setting, Iconwidth, IconHeight);
            var ShotsTex = PrefabPreview.CreateShotsPreviewTexture(_renderCam, _setting, Width, Height);
            var settingJson = JsonUtility.ToJson(_setting);
            var savePath = floderPath + $"/{_prefab.name}";
            if (!Directory.Exists(savePath))
                Directory.CreateDirectory(savePath);
            ExportToPng(iconTex, savePath + "/Icon.png");
            var longTex = new Texture2D(Width * ShotsTex.Length, Height, TextureFormat.RGBA32, false);
            for (int i = 0; i < ShotsTex.Length; i++)
            {
                var cols = ShotsTex[i].GetPixels();
                longTex.SetPixels(i * Width, 0, Width, Height, cols);
            }
            longTex.Apply();
            ExportToPng(longTex, savePath + "/feature1.png");
            
            var jsonPath = savePath +"/setting.json";
            if (File.Exists(jsonPath))
                File.Delete(jsonPath);
            var fs = File.Create(jsonPath);
            using (var sw = new StreamWriter(fs))
            {
                sw.WriteLine(settingJson);
                sw.Flush();
            }
            fs.Close();
            EditorUtility.RevealInFinder(savePath);
        }

        private void ExportToPng(Texture2D tex, string filePath)
        {
            byte[] bytes = tex.EncodeToPNG();
            try
            {
                File.WriteAllBytes(filePath, bytes);
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
                Debug.LogError($"the prefab is Can't.path = {filePath}");
            }
        }

        public void OnDestory()
        {
            if(_bgObj != null)
                Object.DestroyImmediate(_bgObj);
            if(_groundObj != null)
                Object.DestroyImmediate(_groundObj);
            if(_obj != null)
                Object.DestroyImmediate(_obj);
        }
    }
}