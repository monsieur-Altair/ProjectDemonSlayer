using System;
using System.Collections.Generic;
using System.Linq;
using _Application._Scripts.Core;
using _Application._Scripts.Core.Enemies;
using _Application._Scripts.Core.Heroes;
using _Application._Scripts.Core.Towers;
using _Application._Scripts.Scriptables.Core.UnitsBehaviour;
using PathCreation;
using Pool_And_Particles;
using UnityEngine;

namespace _Application.Scripts.Managers
{
    public class Level : MonoBehaviour
    {
        public event Action<IDamagable> HeroAdded = delegate {  };
        
        [SerializeField] private List<PathCreator> _pathCreators;
        [SerializeField] private WaveManager _waveManager;
        [SerializeField] private BaseHero _baseHero;
        [SerializeField] private List<BuildPlace> _buildPlaces;
        
        public List<BuildPlace> BuildPlaces => _buildPlaces;
        public WaveManager WaveManager => _waveManager;
        public BaseHero BaseHero => _baseHero;
        public ElixirManager ElixirManager { get; private set; }
        public TowersManager TowersManager { get; private set; }
        
        private List<BaseTower> _towersInLevel;

        public void Initialize(GlobalPool globalPool, CoreConfig coreConfig, int levelIndex)
        {
            foreach (BuildPlace buildPlace in _buildPlaces) 
                buildPlace.Initialize();

            _towersInLevel = _buildPlaces
                .Select(place => place.CurrentTower)
                .Where(tower => tower != null)
                .ToList();
            
            List<VertexPath> paths = _pathCreators
                .Select(creator => creator.path)
                .ToList();
            
            _waveManager.Initialize(globalPool, coreConfig, paths, levelIndex);

            _baseHero.Appeared += OnHeroAppeared;
            _baseHero.Initialize(coreConfig);


            ElixirManager = new ElixirManager(coreConfig, levelIndex, _waveManager.EnemyTracker);
            TowersManager = new TowersManager(_towersInLevel, coreConfig, globalPool, _waveManager, 
                _towersInLevel[0].transform.parent);
            
            TowersManager.Initialize();
        }

        private void OnHeroAppeared(BaseUnit hero) => 
            HeroAdded(hero);

        public void StartWaves()
        {
            _waveManager.StartSpawn();
            
            foreach (BaseTower tower in _towersInLevel) 
                tower.Enable();
        }

        public void Clear()
        {
            TowersManager.Clear();
            _baseHero.Appeared -= OnHeroAppeared;
            ElixirManager.Clear();
            _waveManager.EnemyTracker.Clear();
        }
    }
}