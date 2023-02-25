using System;
using _Application._Scripts.Core.Towers;
using _Application._Scripts.Scriptables.Core.Towers;
using _Scripts._CoreLogic;
using UnityEngine;

namespace _Application.Scripts.Managers
{
    public class BuildPlace : MonoBehaviour, IMouseClickable
    {
        public event Action <BuildPlace> Clicked = delegate { };

        [SerializeField] private TowerType _towerType;
        [SerializeField] private bool _hasBuildFromStart;
        [SerializeField] private Transform _clickPoint;
        [SerializeField] private Transform _buildPoint;
        [SerializeField] private GameObject _defaultVisual;
        private BaseTower _defaultBaseTower;

        public BaseTower CurrentTower { get; private set; }
        public Transform ClickPoint => _clickPoint;
        public Transform BuildPoint => _buildPoint;
        public GameObject DefaultVisual => _defaultVisual;
        public bool HasBuildFromStart => _hasBuildFromStart;
        public TowerType TowerType => _towerType;


        public void SetTower(BaseTower baseTower)
        {
            CurrentTower = baseTower;
        }

        public void MouseDown()
        {
        }

        public void MouseClick()
        {
        }

        public void MouseUp()
        {
            Clicked(this);
        }

        public int Priority => 1;
    }
}