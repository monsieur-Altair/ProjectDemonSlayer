using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Application._Scripts.Core.Enemies;
using _Application._Scripts.Scriptables.Core.Enemies;
using _Application._Scripts.Scriptables.Core.Levels;
using _Application.Scripts.Managers;
using Extensions;
using PathCreation;
using Pool_And_Particles;
using UnityEngine;

namespace _Application._Scripts.Core
{
    public class WaveManager : MonoBehaviour
    {
        public event Action LevelPassed = delegate { };
        public event Action LevelFailed = delegate { };
        public event Action WaveUpdated = delegate { };
        public event Action PenetrationUpdated = delegate { };
        
        private GlobalPool _globalPool;

        private MyDictionary<EnemyType, BaseEnemy> _enemiesPrefabs;
        private MyDictionary<EnemyType, BaseEnemyData> _enemiesData;
        private List<VertexPath> _paths;
        private bool _isWaveEnded;
        private Coroutine _spawnCoroutine;
        private int _approachedEnemyCount;
        private LevelData _levelData;
        
        private int _currentWaveIndex;
        public int CurrentWaveIndex
        {
            get => _currentWaveIndex;
            private set
            {
                _currentWaveIndex = value;
                WaveUpdated();
            }
        }
        public int MaxWaveIndex => _levelData.WavesData.Count;
        public int MaxApproachingCount => _levelData.ApproachingCount;
        public int ApproachedEnemyCount
        {
            get => _approachedEnemyCount;
            private set
            {
                _approachedEnemyCount = value;
                PenetrationUpdated();
            }
        }


        public EnemyTracker EnemyTracker { get; private set; }

        public void Initialize(GlobalPool globalPool, CoreConfig coreConfig, List<VertexPath> paths, int index)
        {
            _levelData = coreConfig.LevelData[index];
            _paths = paths;
            _globalPool = globalPool;
            _enemiesPrefabs = coreConfig.Warehouse.EnemiesPrefabs;
            _enemiesData = coreConfig.EnemiesData;
            
            EnemyTracker = new EnemyTracker();

            CurrentWaveIndex = -1;
            ApproachedEnemyCount = 0;
        }

        public void StartSpawn()
        {
            CurrentWaveIndex++;
            _isWaveEnded = false;
            EnemyTracker.WaveEnded += OnWaveEnded;
            EnemyTracker.Approached += OnEnemyApproached;
            
            _spawnCoroutine = StartCoroutine(SpawnCoroutine());
        }

        private IEnumerator SpawnCoroutine()
        {
            while (CurrentWaveIndex < MaxWaveIndex)
            {
                yield return null;
                yield return StartCoroutine(SpawnWave());
                yield return new WaitUntil(() => _isWaveEnded);
                _isWaveEnded = false;
                EnemyTracker.Clear();
                CurrentWaveIndex++;
            }
            
            LevelPassed();
        }

        private void Unsubscribe()
        {
            EnemyTracker.WaveEnded -= OnWaveEnded;
            EnemyTracker.Approached -= OnEnemyApproached;
        }

        private void OnEnemyApproached(BaseEnemy enemy)
        {
            ApproachedEnemyCount++;

            if (ApproachedEnemyCount == MaxApproachingCount)
            {
                Debug.Log("failed");
                LevelFailed();
            }
            else
                EnemyTracker.OnEnemyDied(enemy);
        }
        

        private IEnumerator SpawnWave()
        {
            WaveData currentWaveData = _levelData.WavesData[CurrentWaveIndex];
            int enemyAmountInWave = currentWaveData.MiniWavesData.Sum(miniWaveData => miniWaveData.EnemyCount);
            EnemyTracker.Initialize(enemyAmountInWave, _globalPool);

            yield return new WaitForSeconds(currentWaveData.StartDelay);

            foreach (MiniWaveData miniWaveData in currentWaveData.MiniWavesData)
            {
                yield return new WaitForSeconds(miniWaveData.MiniWaveSpawnDelay);
                List<BaseEnemy> spawnedEnemies = SpawnEnemies(miniWaveData.EnemyType, miniWaveData.EnemyCount);
                EnemyTracker.AddRange(spawnedEnemies);
                yield return StartCoroutine(LaunchEnemies(spawnedEnemies));
            }
        }

        private static IEnumerator LaunchEnemies(List<BaseEnemy> spawnedEnemies)
        {
            foreach (BaseEnemy enemy in spawnedEnemies)
            {
                float delay = UnityEngine.Random.Range(0.5f, 1.0f);
                yield return new WaitForSeconds(delay);
                enemy.Launch();
            }
        }

        private void OnWaveEnded()
        {
            _isWaveEnded = true;
        }

        private List<BaseEnemy> SpawnEnemies(EnemyType enemyType, int enemyCount)
        {
            BaseEnemy prefab = _enemiesPrefabs[enemyType];
            List<BaseEnemy> enemies = new(capacity: enemyCount);
            for (int i = 0; i < enemyCount; i++)
            {
                BaseEnemy enemy = _globalPool.Get(prefab, Vector3.forward * 100f);
                enemy.Initialize(_enemiesData[enemyType], _paths.Random());
                enemies.Add(enemy);
            }

            return enemies;
        }

        public void StopSpawn()
        {
            Unsubscribe();
            
            if (_spawnCoroutine != null)
            {
                StopCoroutine(_spawnCoroutine);
                _spawnCoroutine = null;
            }
        }
    }
}