using UnityEngine;

namespace _Application._Scripts.Scriptables.Core.TowerUpgrade
{
    [CreateAssetMenu(fileName = "base tower upgrade data", menuName = "Resources/Towers/TowerUpgradeData", order = 0)]
    public class BaseTowerUpgradeData : ScriptableObject
    {
        [SerializeField] private float _powerCoefficient = 1;
        [SerializeField] private float _healthCoefficient = 1;
        [SerializeField] private float _rangeCoefficient = 1;
        [SerializeField] private int _requiredCardAmount;
        
        
        public float PowerCoefficient => _powerCoefficient;
        public float HealthCoefficient => _healthCoefficient;
        public float RangeCoefficient => _rangeCoefficient;
        public int RequiredCardAmount => _requiredCardAmount;
    }
}