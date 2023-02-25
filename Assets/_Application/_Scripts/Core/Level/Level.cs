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
        

        public void Initialize(GlobalPool globalPool, CoreConfig coreConfig, int levelIndex)
        {
            List<VertexPath> paths = _pathCreators
                .Select(creator => creator.path)
                .ToList();
            
            _waveManager.Initialize(globalPool, coreConfig, paths, levelIndex);

            _baseHero.Appeared += OnHeroAppeared;
            _baseHero.Initialize(coreConfig);
            
            ElixirManager = new ElixirManager(coreConfig, levelIndex, _waveManager.EnemyTracker);
            TowersManager = new TowersManager(coreConfig, globalPool, _waveManager, transform.parent);
            
            foreach (BuildPlace buildPlace in _buildPlaces.Where(buildPlace => buildPlace.HasBuildFromStart))
            {
                buildPlace.DefaultVisual.SetActive(false);
                TowersManager.BuildBuilding(buildPlace.TowerType, buildPlace);
            }
        }

        private void OnHeroAppeared(BaseUnit hero) => 
            HeroAdded(hero);

        public void StartWaves()
        {
            _waveManager.StartSpawn();
            TowersManager.StartAttacking();
        }

        public void Stop()
        {
            TowersManager.StopAttacking();
            _waveManager.StopSpawn();
            _waveManager.EnemyTracker.Stop();
            _baseHero.Appeared -= OnHeroAppeared;
            ElixirManager.Clear();
        }
        
        public void Clear()
        {
            TowersManager.Clear();
            _baseHero.Clear();
            _waveManager.EnemyTracker.Clear();
        }
    }
}