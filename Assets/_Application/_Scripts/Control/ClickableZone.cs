using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _UI._Screens
{
    public class ClickableZone : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerMoveHandler
    {
        public event Action OnPointerDowned;
        public event Action OnPointerUpped;
        public event Action<PointerEventData> OnPointerMoved;
        
        public void OnPointerDown(PointerEventData eventData)
        {
            OnPointerDowned?.Invoke();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            OnPointerUpped?.Invoke();
        }

        public void OnPointerMove(PointerEventData eventData)
        {
            OnPointerMoved?.Invoke(eventData);
        }
    }
}