using UnityEngine;

namespace _Scripts._CoreLogic
{
    public interface IMouseClickable
    {
        void MouseDown();

        void MouseClick();

        void MouseUp();
        
        void MouseMove(Vector3 hitPoint) { }

        int Priority { get; }
    }
}