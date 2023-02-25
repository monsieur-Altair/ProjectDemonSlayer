using _Managers;
using UnityEngine;

public class MobileInputZoned : InputZoned
{
    public MobileInputZoned(IInputSystem inputSystem) : base(inputSystem)
    {
    }

    public override bool IsHolding => _clickCount == 1 
                                      && Input.touchCount == 1 
                                      && _inputSystem.IsHolding 
                                      && Input.touches[0].phase == TouchPhase.Moved;
    public override bool IsPressed => _clickCount == 1 && _inputSystem.IsPressed;
    public override Vector3 LastActivityPosition => _inputSystem.LastActivityPosition;
    public override bool IsUpped => _inputSystem.IsUpped && _clickCount == 0;
    public override bool IsSpecialAction => _inputSystem.IsSpecialAction;
}