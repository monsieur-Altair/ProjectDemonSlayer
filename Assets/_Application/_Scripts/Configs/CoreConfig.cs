using System.Collections.Generic;
using _Application._Scripts.Core.Enemies;
using _Application._Scripts.Scriptables.Core.Enemies;
using _Application._Scripts.Scriptables.Core.Levels;
using _Application.Scripts.Infrastructure.Services;
using UnityEngine;

namespace _Application.Scripts.Managers
{
    [CreateAssetMenu (fileName = "CoreConfig",menuName = "Resources/Core Config")]
    public class CoreConfig: ScriptableObject, IService
    {
        [SerializeField] private MonoBehaviourServices _monoBehaviourServices;
        [Space, SerializeField] private bool _useTutorial;
        [SerializeField] private PlayerConfig _playerConfig;
        [SerializeField] private Warehouse _warehouse;
        [SerializeField] private MyDictionary<EnemyType, BaseEnemyData> _enemiesData;
        [SerializeField, NonReorderable] private List<LevelData> _levelData;
        
        public List<LevelData> LevelData => _levelData;
        public MyDictionary<EnemyType, BaseEnemyData> EnemiesData => _enemiesData;
        public MonoBehaviourServices MonoBehaviourServices => _monoBehaviourServices;
        public bool UseTutorial => _useTutorial;
        public PlayerConfig PlayerConfig => _playerConfig;
        public Warehouse Warehouse => _warehouse;
    }
}