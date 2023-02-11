using _Application._Scripts.Core.Enemies;
using _Application._Scripts.Core.Towers;
using _Application._Scripts.Scriptables.Core.Enemies;
using _Application._Scripts.Scriptables.Core.Towers;
using _Application.Scripts.UI;
using UnityEngine;

namespace _Application.Scripts.Managers
{
    [CreateAssetMenu(fileName = "Warehouse", menuName = "Resources/Warehouse", order = 0)]
    public class Warehouse : ScriptableObject
    {
        [SerializeField] private UnitBar _barPrefab;
        [SerializeField, NonReorderable] private MyDictionary<EnemyType, BaseEnemy> _enemiesPrefabs;
        [SerializeField, NonReorderable] private MyDictionary<TowerType, BaseProjectile> _projectilePrefabs;
        [SerializeField, NonReorderable] private MyDictionary<TowerType, BaseTower> _towersPrefabs;

        public MyDictionary<TowerType, BaseTower> TowersPrefabs => _towersPrefabs;
        public MyDictionary<TowerType, BaseProjectile> ProjectilePrefabs => _projectilePrefabs;
        public MyDictionary<EnemyType, BaseEnemy> EnemiesPrefabs => _enemiesPrefabs;
        public UnitBar BarPrefab => _barPrefab;
    }
}