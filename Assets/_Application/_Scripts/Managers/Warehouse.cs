using _Application._Scripts.Core.Enemies;
using _Application._Scripts.Scriptables.Core.Enemies;
using _Application.Scripts.UI;
using UnityEngine;

namespace _Application.Scripts.Managers
{
    [CreateAssetMenu(fileName = "Warehouse", menuName = "Resources/Warehouse", order = 0)]
    public class Warehouse : ScriptableObject
    {
        [SerializeField] private UnitBar _barPrefab;
        [SerializeField, NonReorderable] private MyDictionary<EnemyType, BaseEnemy> _enemiesPrefabs;
        
        public MyDictionary<EnemyType, BaseEnemy> EnemiesPrefabs => _enemiesPrefabs;
        public UnitBar BarPrefab => _barPrefab;
    }
}