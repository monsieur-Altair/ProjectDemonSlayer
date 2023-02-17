using _Application.Scripts.Scriptables.Core.Enemies;
using UnityEngine;

namespace _Application._Scripts.Scriptables.Core.Towers
{
    [CreateAssetMenu(fileName = "base tower", menuName = "Resources/Towers/Base", order = 0)]
    public class BaseTowerData : BaseData
    {
        [SerializeField] private float _radius = 8f;
        [SerializeField] private float _projectileSpeed = 8f;
        [SerializeField] private int _buildCost = 200;
        [SerializeField] private int _destroyReward = 50;
        
        public int DestroyReward => _destroyReward;
        public int BuildCost => _buildCost;
        public float ProjectileSpeed => _projectileSpeed;
        public float Radius => _radius;
    }
}