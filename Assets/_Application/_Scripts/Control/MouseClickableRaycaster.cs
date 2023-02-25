using System;
using UnityEngine;

namespace _Scripts._CoreLogic
{
    public class MouseClickableRaycaster : MonoBehaviour
    {
        [SerializeField] private LayerMask _layerMask;
        [SerializeField] private Camera _camera;

        private bool _clicked;
        private IMouseClickable _clickedObject;

        public bool CastDown()
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hitInfo, Single.MaxValue, _layerMask.value))
            {
                IMouseClickable mouseClickable = hitInfo.collider.GetComponent<IMouseClickable>();
                if (mouseClickable != null)
                {
                    _clicked = true;
                    _clickedObject = mouseClickable;
                    mouseClickable.MouseDown();
                    return true;
                }
            }

            _clicked = false;
            _clickedObject = null;
            return false;
        }

        public bool CastUp()
        {
            if (!_clicked)
                return false;

            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hitInfo, Single.MaxValue, _layerMask.value))
            {
                IMouseClickable mouseClickable = hitInfo.collider.GetComponent<IMouseClickable>();
                if (mouseClickable != null && mouseClickable == _clickedObject)
                {
                    mouseClickable.MouseClick();
                }
            }

            _clickedObject.MouseUp();

            _clicked = false;
            _clickedObject = null;
            return true;
        }

        public bool CastMove()
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hitInfo, Single.MaxValue, _layerMask.value))
            {
                IMouseClickable mouseClickable = hitInfo.collider.GetComponent<IMouseClickable>();
                mouseClickable?.MouseMove(hitInfo.point);
                return true;
            }

            return false;
        }
    }
}