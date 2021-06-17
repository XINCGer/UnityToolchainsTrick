using System;
using UnityEngine.UI;

namespace CZToolKit.Core.ReactiveX
{
    public class OnClick : Operator<Button>
    {
        Button button;

        public OnClick(IObservable<Button> _src) : base(_src) { }

        public override void OnNext(Button _button)
        {
            button = _button;

            if (button == null)
                OnError(new ArgumentNullException("button", "Button 参数为空"));
            else
                button.onClick.AddListener(OnBtnClick);
        }


        public override void OnDispose()
        {
            button.onClick.RemoveListener(OnBtnClick);
        }

        private void OnBtnClick()
        {
            base.OnNext(button);
        }
    }
}
