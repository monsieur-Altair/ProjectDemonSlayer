using System.Collections.Generic;
using _Application._Scripts.Core.Enemies;
using _Application._Scripts.Scriptables.Core.Enemies;
using _Application._Scripts.Scriptables.Core.Levels;
using _Application._Scripts.Scriptables.Core.Towers;
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
        [Space, SerializeField] private PlayerConfig _playerConfig;
        [Space, SerializeField] private Warehouse _warehouse;
        [Space, SerializeField] private MyDictionary<EnemyType, BaseEnemyData> _enemiesData;
        [Space, SerializeField] private MyDictionary<TowerType, BaseTowerData> _towersData;
        [Space, SerializeField, NonReorderable] private List<LevelData> _levelData;
        [Space, SerializeField] private BaseUnitData _warriorData;

        public MyDictionary<TowerType, BaseTowerData> TowersData => _towersData;
        public List<LevelData> LevelData => _levelData;
        public MyDictionary<EnemyType, BaseEnemyData> EnemiesData => _enemiesData;
        public MonoBehaviourServices MonoBehaviourServices => _monoBehaviourServices;
        public bool UseTutorial => _useTutorial;
        public PlayerConfig PlayerConfig => _playerConfig;
        public Warehouse Warehouse => _warehouse;
        public BaseUnitData WarriorData => _warriorData;
    }
}