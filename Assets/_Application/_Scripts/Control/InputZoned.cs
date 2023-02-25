using System;
using _Scripts._CoreLogic;
using _UI._Screens;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Managers
{
    public abstract class InputZoned : IInputSystem
    {
        public event Action Pressed;
        public event Action Upped;

        protected readonly IInputSystem _inputSystem;
        protected ClickableZone _zone;
        protected int _clickCount;
        private MouseClickableRaycaster _raycaster;

        public ClickableZoneType ClickableZoneType { get; private set; }

        public abstract bool IsHolding { get; }
        public abstract bool IsPressed { get; }
        public abstract Vector3 LastActivityPosition { get; }
        public abstract bool IsUpped { get; }
        public abstract bool IsSpecialAction { get; }
        
        
        protected InputZoned(IInputSystem inputSystem)
        {
            _inputSystem = inputSystem;
        }

        public Vector2 LastActivityPositionOnCanvas(Vector2 canvasSize)
        {
            Vector2 viewportPos = GetViewportPosition();
            return Vector2.Scale(viewportPos, canvasSize);
        }

        public Vector2 GetViewportPosition()
        {
            Vector3 pos = LastActivityPosition;
            return new Vector2(pos.x / Screen.width, pos.y / Screen.height);
        }

        public void SetZone(ClickableZone zone, ClickableZoneType clickableZoneType)
        {
            if (_zone != null)
            {
                _zone.OnPointerDowned -= OnPointerDowned;
                _zone.OnPointerUpped -= OnPointerUpped;
                _zone.OnPointerMoved -= OnPointerMoved;
                _zone = null;
            }

            _zone = zone;
            ClickableZoneType = clickableZoneType;

            _zone.OnPointerDowned += OnPointerDowned;
            _zone.OnPointerUpped += OnPointerUpped;
            _zone.OnPointerMoved += OnPointerMoved;
        }

        public void SetMouseClickable(MouseClickableRaycaster raycaster)
        {
            _raycaster = raycaster;
        }

        private void OnPointerDowned()
        {

            if (_raycaster != null && _raycaster.CastDown())
                return;

            _clickCount++;

            Pressed?.Invoke();
        }
        
        private void OnPointerMoved(PointerEventData eventData)
        {
        }

        private void OnPointerUpped()
        {

            if (_raycaster != null && _raycaster.CastUp())
                return;

            _clickCount--;

            if (_clickCount < 0)
                _clickCount = 0;
            
            Upped?.Invoke();
        }
    }

    public enum ClickableZoneType
    {
        GameplayScreen,
    }
}