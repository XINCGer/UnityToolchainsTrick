using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Example_03_PrefabIconCreator
{
    public class PreviewWindow
    {
        public VisualElement imageElement;
        private Camera _camera;

        public PreviewWindow(Rect rect, Camera previewCam)
        {
            _camera = previewCam;
            imageElement = new Image();
            //imageElement.style. = rect;
        }


    }
}