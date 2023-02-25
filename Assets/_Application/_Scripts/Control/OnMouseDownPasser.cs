using System;
using UnityEngine;

namespace _Scripts._CoreLogic
{
    public class OnMouseDownPasser : MonoBehaviour, IMouseClickable
    {
        public event Action MouseDowned;

        public void MouseDown()
        {
            MouseDowned?.Invoke();
        }

        public void MouseClick()
        {
        }
        
        public void MouseUp() { }
        public int Priority => 0;
    }
}