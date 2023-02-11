using System.Collections.Generic;
using System.Linq;
using _Application._Scripts.Core;
using _Application._Scripts.Scriptables.Core.Levels;
using PathCreation;
using Pool_And_Particles;
using UnityEngine;

namespace _Application.Scripts.Managers
{
    public class Level : MonoBehaviour
    {
        [SerializeField] private List<PathCreator> _pathCreators;
        [SerializeField] private WaveManager _waveManager;

        public WaveManager WaveManager => _waveManager;

        public void Initialize(GlobalPool globalPool, CoreConfig coreConfig, int levelIndex)
        {
            List<VertexPath> paths = _pathCreators.Select(creator => creator.path).ToList();
            _waveManager.Initialize(globalPool, coreConfig, paths, levelIndex);
        }

        public void StartWaves()
        {
            _waveManager.StartSpawn();
        }

        public void Clear()
        {
            _waveManager.EnemyTracker.Clear();
        }
    }
}