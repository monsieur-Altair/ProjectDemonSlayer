using System.Collections.Generic;
using _Application._Scripts.Core.Enemies;
using _Application._Scripts.Scriptables.Core.Enemies;
using _Application._Scripts.Scriptables.Core.Levels;
using _Application._Scripts.Scriptables.Core.Towers;
using _Application._Scripts.Scriptables.Core.TowerUpgrade;
using _Application.Scripts.Infrastructure.Services;
using _Application.Scripts.Scriptables.Core.Enemies;
using UnityEngine;

namespace _Application.Scripts.Managers
{
    [CreateAssetMenu (fileName = "CoreConfig",menuName = "Resources/Core Config")]
    public class CoreConfig: ScriptableObject, IService
    {
        [Space, SerializeField] private MonoBehaviourServices _monoBehaviourServices;
        [Space, SerializeField] private bool _useTutorial;
        [Space, SerializeField] private Warehouse _warehouse;
        [Space, SerializeField] private MyDictionary<EnemyType, BaseEnemyData> _enemiesData;
        [Space, SerializeField] private MyDictionary<TowerType, BaseTowerData> _towersData;
        [Space, SerializeField, NonReorderable] private List<LevelData> _levelData;
        [Space, SerializeField] private BaseUnitData _warriorData;
        [Space, SerializeField] private KnightHeroData _knightHeroData;
        [Space, SerializeField] private MyDictionary<TowerType, BaseTowerUpgradeData[]> _towersUpgrades;

        public MyDictionary<TowerType, BaseTowerUpgradeData[]> TowersUpgrades => _towersUpgrades;
        public MyDictionary<TowerType, BaseTowerData> TowersData => _towersData;
        public List<LevelData> LevelData => _levelData;
        public MyDictionary<EnemyType, BaseEnemyData> EnemiesData => _enemiesData;
        public MonoBehaviourServices MonoBehaviourServices => _monoBehaviourServices;
        public bool UseTutorial => _useTutorial;
        public Warehouse Warehouse => _warehouse;
        public BaseUnitData WarriorData => _warriorData;
        public KnightHeroData KnightHeroData => _knightHeroData;
    }
}