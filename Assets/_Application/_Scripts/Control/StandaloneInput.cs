using UnityEngine;

namespace _Managers
{
    public class StandaloneInput : IInputSystem
    {

        public bool IsHolding => Input.GetMouseButton(0);
        public bool IsPressed => Input.GetMouseButtonDown(0);
        public Vector3 LastActivityPosition => Input.mousePosition;
        public bool IsUpped => Input.GetMouseButtonUp(0);
        public bool IsSpecialAction => Input.GetKeyDown(KeyCode.D) &&
                                       (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.LeftCommand));
        
    }
}