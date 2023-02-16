using _Application.Scripts.Scriptables.Core.Enemies;
using UnityEngine;

namespace _Application._Scripts.Scriptables.Core.Towers
{
    [CreateAssetMenu(fileName = "base tower", menuName = "Resources/Towers/Base", order = 0)]
    public class BaseTowerData : BaseData
    {
        [SerializeField] private float _radius = 8f;
        [SerializeField] private TowerType _towerType;
        [SerializeField] private float _projectileSpeed = 8f;

        public float ProjectileSpeed => _projectileSpeed;
        public TowerType TowerType => _towerType;
        public float Radius => _radius;
    }
}