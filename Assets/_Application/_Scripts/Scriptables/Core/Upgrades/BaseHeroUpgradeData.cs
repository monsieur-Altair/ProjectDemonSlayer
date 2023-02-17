using UnityEngine;

namespace _Application._Scripts.Scriptables.Core.TowerUpgrade
{
    [CreateAssetMenu(fileName = "base hero upgrade data", menuName = "Resources/Towers/HeroUpgradeData", order = 0)]
    public class BaseHeroUpgradeData : ScriptableObject
    {
        [SerializeField] private float _powerCoefficient = 1;
        [SerializeField] private float _healthCoefficient = 1;
        [SerializeField] private float _speedCoefficient = 1;
        [SerializeField] private int _requiredCardAmount;
        
        public float PowerCoefficient => _powerCoefficient;
        public float HealthCoefficient => _healthCoefficient;
        public float SpeedCoefficient => _speedCoefficient;
        public int RequiredCardAmount => _requiredCardAmount;
    }
}