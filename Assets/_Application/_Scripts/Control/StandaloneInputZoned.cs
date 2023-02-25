using UnityEngine;

namespace _Managers
{
    public class StandaloneInputZoned : InputZoned
    {
        public StandaloneInputZoned(IInputSystem inputSystem) : base(inputSystem)
        {
        }

        public override bool IsHolding => _clickCount == 1 && _inputSystem.IsHolding;
        public override bool IsPressed => _clickCount == 1 && _inputSystem.IsPressed;
        public override Vector3 LastActivityPosition => _inputSystem.LastActivityPosition;
        public override bool IsUpped => _clickCount == 0 && _inputSystem.IsUpped;
        public override bool IsSpecialAction => _inputSystem.IsSpecialAction;
    }
}