using UnityEngine;

namespace _Managers
{
    public interface IInputSystem
    {
        public bool IsHolding { get; }
        public bool IsPressed { get; }
        public Vector3 LastActivityPosition { get; }
        public bool IsUpped { get; }
        public bool IsSpecialAction { get; }
    }
}