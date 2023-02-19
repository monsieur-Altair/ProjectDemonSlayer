using System;
using System.Collections.Generic;
using _Application._Scripts.Core.Heroes;
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
        public const int TowerLevelAmount = 2;

        [Space, SerializeField] private MonoBehaviourServices _monoBehaviourServices;
        [Space, SerializeField] private bool _useTutorial;
        [Space, SerializeField] private Warehouse _warehouse;
        [Space, SerializeField] private MyDictionary<EnemyType, BaseEnemyData> _enemiesData;
        [Space, SerializeField] private MyDictionary<TowerType, List<BaseTowerData>> _towersData;
        [Space, SerializeField, NonReorderable] private List<LevelData> _levelData;
        [Space, SerializeField] private BaseUnitData _warriorData;
        [Space, SerializeField] private MyDictionary<TowerType, List<ListWrapper<BaseTowerUpgradeData>>> _towersUpgradesLists;
        [Space, SerializeField] private MyDictionary<HeroType, List<BaseHeroUpgradeData>> _heroUpgrades;
        [Space, SerializeField] private MyDictionary<HeroType, BaseUnitData> _heroDatas;

        public MyDictionary<HeroType, BaseUnitData> HeroDatas => _heroDatas;
        public MyDictionary<HeroType, List<BaseHeroUpgradeData>> HeroUpgrades => _heroUpgrades;
        public MyDictionary<TowerType, List<ListWrapper<BaseTowerUpgradeData>>> TowersUpgradesLists => _towersUpgradesLists;
        public MyDictionary<TowerType, List<BaseTowerData>> TowersData => _towersData;
        public List<LevelData> LevelData => _levelData;
        public MyDictionary<EnemyType, BaseEnemyData> EnemiesData => _enemiesData;
        public MonoBehaviourServices MonoBehaviourServices => _monoBehaviourServices;
        public bool UseTutorial => _useTutorial;
        public Warehouse Warehouse => _warehouse;
        public BaseUnitData WarriorData => _warriorData;

        [Serializable]
        public class ListWrapper<T>
        {
            public List<T> List;
        }
    }
}