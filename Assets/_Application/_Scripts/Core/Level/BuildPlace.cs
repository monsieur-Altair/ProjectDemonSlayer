using System;
using _Application._Scripts.Core.Towers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Application.Scripts.Managers
{
    public class BuildPlace : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public event Action <BuildPlace> Clicked = delegate { };
        
        [SerializeField] private Transform _clickPoint;
        [SerializeField] private Transform _buildPoint;
        [SerializeField] private BaseTower _defaultBaseTower;
        [SerializeField] private GameObject _defaultVisual;
        
        public BaseTower CurrentTower { get; private set; }
        public Transform ClickPoint => _clickPoint;
        public Transform BuildPoint => _buildPoint;
        public GameObject DefaultVisual => _defaultVisual;

        public void Initialize()
        {
            SetTower(_defaultBaseTower);
            _defaultVisual.SetActive(_defaultBaseTower == null);
        }
        
        public void SetTower(BaseTower baseTower)
        {
            CurrentTower = baseTower;
        }
        
        public void OnPointerUp(PointerEventData eventData)
        {
            Clicked(this);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
        }
    }
}