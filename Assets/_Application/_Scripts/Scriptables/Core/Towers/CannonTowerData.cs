using UnityEngine;

namespace _Application._Scripts.Scriptables.Core.Towers
{
    [CreateAssetMenu(fileName = "cannon tower", menuName = "Resources/Towers/Cannon", order = 0)]
    public class CannonTowerData : BaseTowerData
    {
        [SerializeField] private float _explosionRadius;
        public float ExplosionRadius => _explosionRadius;
    }
}