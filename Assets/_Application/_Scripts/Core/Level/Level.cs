using System.Collections.Generic;
using System.Linq;
using _Application._Scripts.Core;
using _Application._Scripts.Core.Towers;
using _Application._Scripts.Scriptables.Core.Towers;
using PathCreation;
using Pool_And_Particles;
using UnityEngine;

namespace _Application.Scripts.Managers
{
    public class Level : MonoBehaviour
    {
        [SerializeField] private List<BaseTower> _towers;
        [SerializeField] private List<PathCreator> _pathCreators;
        [SerializeField] private WaveManager _waveManager;

        public WaveManager WaveManager => _waveManager;

        public void Initialize(GlobalPool globalPool, CoreConfig coreConfig, int levelIndex)
        {
            List<VertexPath> paths = _pathCreators.Select(creator => creator.path).ToList();
            _waveManager.Initialize(globalPool, coreConfig, paths, levelIndex);

            foreach (BaseTower tower in _towers)
                tower.Initialize(coreConfig, _waveManager.EnemyTracker, globalPool);
        }

        public void StartWaves()
        {
            _waveManager.StartSpawn();
            
            foreach (BaseTower tower in _towers) 
                tower.Enable();
        }

        public List<TBaseTower> GetTowers<TBaseTower>(TowerType towerType) where TBaseTower : BaseTower
        {
            return _towers
                .Where(tower => tower.TowerType == towerType)
                .Cast<TBaseTower>()
                .ToList();
        }
        
        public void Clear()
        {
            foreach (BaseTower tower in _towers)
            {
                tower.Disable();
                tower.Clear();
            }

            _waveManager.EnemyTracker.Clear();
        }
    }
}