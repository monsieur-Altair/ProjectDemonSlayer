using System;
using UnityEngine;

namespace _Managers
{
    public class MobileInput : IInputSystem
    {
        private float _cachedZoomValue;
        private readonly int _height;
        private readonly int _width;

        public bool IsHolding => Input.touchCount == 1;
        public bool IsPressed => IsTouchStarted();
        public Vector3 LastActivityPosition => GetLastTouchPosition();
        public bool IsUpped => IsTouchEnded();
        public bool IsSpecialAction => Input.touchCount == 4 && Input.GetTouch(3).phase == TouchPhase.Began;

        public bool IsEndedZooming
        {
            get
            {
                if (Input.touchCount < 2 && Mathf.Abs(_cachedZoomValue) > 0.001f)
                {
                    _cachedZoomValue = 0.0f;
                    return true;
                }

                return false;
            }
        }


        public MobileInput()
        {
            _width = Screen.width;
            _height = Screen.height;
            _cachedZoomValue = 0f;
        }

        private static Vector2 GetLastTouchPosition()
        {
            if (Input.touchCount < 1)
                return Vector3.zero;
            for (var i = 0; i < Input.touchCount; i++)
            {
                if (Input.touches[i].phase == TouchPhase.Began)
                    return Input.touches[i].position;
            }

            return Input.touches[Input.touchCount - 1].position;
        }

        private static bool IsTouchStarted()
        {
            if (Input.touchCount < 1)
                return false;

            return Input.touches[0].phase == TouchPhase.Began;
        }

        private static bool IsTouchEnded()
        {
            if (Input.touchCount < 1)
                return false;

            return Input.touches[0].phase == TouchPhase.Ended;
        }
    }
}