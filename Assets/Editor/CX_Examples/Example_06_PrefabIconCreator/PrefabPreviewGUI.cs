//---------------------------------------------------------------------------------------
// Author: xuan_chen_work@foxmail.com
// Date: 2021-11-15 11:40:14
// Name: PrefabPreviewUIElement
//---------------------------------------------------------------------------------------

using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Example_06_PrefabIconCreator
{
    public class PrefabPreviewGUI
    {
        public static readonly Vector2 WindowSize = new Vector2 {x = 420, y = 570};

        private const int width = 800;
        private const int Iconwidth = 400;
        private const int PageNum = 15;
        
        public VisualElement PreviewElement { get; private set; }

        #region UIElement
        private Image _previewImg;
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
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Obj"></param>
        /// <param name="RenderCam"></param>
        public PrefabPreviewGUI(GameObject obj, Camera renderCam, PreviewSettingData settingData = null)
        {
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/CX_Examples/Example_06_PrefabIconCreator/UXML/PreviewWindow.uxml");
            PreviewElement = visualTree.Instantiate();
            PreviewElement.style.minWidth = WindowSize.x;
            PreviewElement.style.minHeight = WindowSize.y;
            PreviewElement.style.maxWidth = WindowSize.x;
            PreviewElement.style.minHeight = WindowSize.y;

            #region 找出Element
            _previewImg = PreviewElement.Q<Image>("PreviewImage");
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
            #endregion
            
            #region 设置初始值
            var (tex, setting) = PrefabPreview.CreatePreviewTexture(obj, renderCam, width);
            _previewImg.image = tex;
            SetValue(setting);
            #endregion
            RegisterCallBack();
        }

        private void SetValue(PreviewSettingData setting)
        {
            _bgColorField.value = setting.BgColor;
            var texPath = AssetDatabase.GUIDToAssetPath(setting.BgGuid);
            if (string.IsNullOrEmpty(texPath))
                _bgTexField.value = null;
            else
                _bgTexField.value = AssetDatabase.LoadAssetAtPath<Texture2D>(texPath);
            var groundPath = AssetDatabase.GUIDToAssetPath(setting.GroundGuid);
            if (string.IsNullOrEmpty(texPath))
            {
                _groundField.value = null;
            }
            else
            {
                _groundField.value = AssetDatabase.LoadAssetAtPath<GameObject>(groundPath);
            }

            _groundHeight.value = setting.GroundHeight;
            _groundScale.value = setting.GroundScale;
            _centerOffsetField.value = setting.CenterOffSet;
            _distanceField.value = setting.Distance;
            _pitchAngleField.value = setting.PitchAngle;
            _startAngleField.value = setting.StartAngle;
        }

        private void RegisterCallBack()
        {
            _previewImg.RegisterCallback<MouseDownEvent>(OnImgMouseDown);
            _previewImg.RegisterCallback<MouseUpEvent>(OnImgMouseUp);
            _previewImg.RegisterCallback<MouseLeaveEvent>(OnImgMouseExit);
            //_previewImg.RegisterCallback<WheelEvent>(OnImgZoomWheel);
            _previewImg.RegisterCallback<MouseMoveEvent>(OnOperator);
        }
        
        private bool _isMove;
        private void OnImgMouseDown(MouseDownEvent evn)
        {
            _isMove = true;
            Debug.Log("OnImgMouseDown");
        }

        private void OnImgMouseUp(MouseUpEvent evn)
        {
            _isMove = false;
            Debug.Log("OnImgMouseUp");
        }

        private void OnImgMouseExit(MouseLeaveEvent evn)
        {
            _isMove = false;
            Debug.Log("OnImgMouseExit");
        }
        
        private void OnOperator(MouseMoveEvent evn)
        {
            if (!_isMove) return;
            Debug.Log("OnOperator");
        }
    }
}