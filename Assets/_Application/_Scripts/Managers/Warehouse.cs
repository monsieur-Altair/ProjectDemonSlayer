using System.Collections.Generic;
using _Application._Scripts.Core.Enemies;
using _Application._Scripts.Core.Heroes;
using _Application._Scripts.Core.Towers;
using _Application._Scripts.Scriptables.Core.Enemies;
using _Application._Scripts.Scriptables.Core.Towers;
using _Application._Scripts.Scriptables.Core.UnitsBehaviour;
using _Application.Scripts.UI;
using UnityEngine;

namespace _Application.Scripts.Managers
{
    [CreateAssetMenu(fileName = "Warehouse", menuName = "Resources/Warehouse", order = 0)]
    public class Warehouse : ScriptableObject
    {
        [Space, SerializeField] private UnitBar _barPrefab;
        [Space, SerializeField, NonReorderable] private MyDictionary<EnemyType, BaseEnemy> _enemiesPrefabs;
        [Space, SerializeField, NonReorderable] private MyDictionary<EnemyType, Sprite> _enemyPreviewSprites;

        [Space, SerializeField, NonReorderable] private MyDictionary<TowerType, BaseProjectile> _projectilePrefabs;
        [Space, SerializeField, NonReorderable] private MyDictionary<TowerType, BaseTower> _towersPrefabs;
        
        [Space, SerializeField, NonReorderable] private MyDictionary<TowerType, List<Sprite>> _towerCardSprites;
        [Space, SerializeField, NonReorderable] private MyDictionary<TowerType, List<Sprite>> _towerPreviewSprites;
        
        [Space, SerializeField, NonReorderable] private MyDictionary<HeroType, Sprite> _heroPreviewSprites;
        [Space, SerializeField, NonReorderable] private MyDictionary<HeroType, Sprite> _heroCardSprites;
        
        
        [Space, SerializeField] private Warrior _warriorPrefab;

        
        public MyDictionary<EnemyType, Sprite> EnemyPreviewSprites => _enemyPreviewSprites;
        public MyDictionary<HeroType, Sprite> HeroPreviewSprites => _heroPreviewSprites;
        public MyDictionary<HeroType, Sprite> HeroCardSprites => _heroCardSprites;
        public MyDictionary<TowerType, List<Sprite>> TowerPreviewSprites => _towerPreviewSprites;
        public MyDictionary<TowerType, List<Sprite>> TowerCardSprites => _towerCardSprites;
        public MyDictionary<TowerType, BaseTower> TowersPrefabs => _towersPrefabs;
        public MyDictionary<TowerType, BaseProjectile> ProjectilePrefabs => _projectilePrefabs;
        public MyDictionary<EnemyType, BaseEnemy> EnemiesPrefabs => _enemiesPrefabs;
        public UnitBar BarPrefab => _barPrefab;
        public Warrior WarriorPrefab => _warriorPrefab;
    }
}