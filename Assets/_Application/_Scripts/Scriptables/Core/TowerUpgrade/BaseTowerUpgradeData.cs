using UnityEngine;

namespace _Application._Scripts.Scriptables.Core.TowerUpgrade
{
    [CreateAssetMenu(fileName = "base tower upgrade data", menuName = "Resources/Towers/TowerUpgradeData", order = 0)]
    public class BaseTowerUpgradeData : ScriptableObject
    {
        [SerializeField] private float _power;
        [SerializeField] private float _health;
        [SerializeField] private float _range;
        [SerializeField] private int _requiredCardAmount;
        
        
        public float Power => _power;
        public float Health => _health;
        public float Range => _range;
        public int RequiredCardAmount => _requiredCardAmount;
    }
}