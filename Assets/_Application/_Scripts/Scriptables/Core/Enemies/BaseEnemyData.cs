using _Application.Scripts.Scriptables.Core.Enemies;
using UnityEngine;

namespace _Application._Scripts.Scriptables.Core.Enemies
{
    [CreateAssetMenu(fileName = "base enemy", menuName = "Resources/Enemies/Base", order = 0)]
    public class BaseEnemyData : BaseData
    {
        [SerializeField] private float _killingReward;
        [SerializeField] private EnemyType _enemyType;
        [SerializeField] private EnemyHierarchyType _enemyHierarchyType;
        [SerializeField] private EnemyBehaviourType _behaviourType;
        [SerializeField] private EnemyMotionType _enemyMotionType;

        public EnemyMotionType EnemyMotionType => _enemyMotionType;
        public EnemyHierarchyType EnemyHierarchyType => _enemyHierarchyType;
        public EnemyBehaviourType BehaviourType => _behaviourType;
        public float KillingReward => _killingReward;
        public EnemyType EnemyType => _enemyType;
    }
}