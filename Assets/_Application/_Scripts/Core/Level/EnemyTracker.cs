using System;
using System.Collections.Generic;
using _Application._Scripts.Core.Enemies;
using Pool_And_Particles;
using UnityEngine;

namespace _Application._Scripts.Core
{
    public class EnemyTracker
    {
        public event Action<IDamagable> EnemyAdded = delegate {  };
        public event Action WaveEnded = delegate { }; 
        public event Action<BaseEnemy> Approached = delegate { };

        private int _enemyAmountInWave;

        private int _aliveEnemyAmount;
        private int _launchedEnemyAmount;
        
        private bool _isTrackingEnabled;
        private GlobalPool _globalPool;
        public List<BaseEnemy> Enemies { get; } = new();

        public void Initialize(int enemyAmountInWave, GlobalPool globalPool)
        {
            _globalPool = globalPool;
            _enemyAmountInWave = enemyAmountInWave;
            _aliveEnemyAmount = 0;
            _launchedEnemyAmount = 0;
            DisableTracking();
        }

        public void AddRange(List<BaseEnemy> enemies)
        {
            foreach (BaseEnemy enemy in enemies) 
                AddEnemy(enemy);
        }

        private void AddEnemy(BaseEnemy enemy)
        {
            enemy.Launched += OnEnemyLaunched;
            enemy.Died += OnEnemyDied;
            enemy.Approached += OnEnemyApproached;
            
            Enemies.Add(enemy);

            EnemyAdded(enemy);
        }

        public void OnEnemyDied(IDamagable damagable)
        {
            BaseEnemy baseEnemy = damagable as BaseEnemy;
            
            Enemies.Remove(baseEnemy);
            _aliveEnemyAmount--;

            FreeEnemy(baseEnemy);

            if (_isTrackingEnabled && _aliveEnemyAmount == 0)
            {
                DisableTracking();
                Debug.Log("ended");
                WaveEnded();
            }
        }

        private void FreeEnemy(BaseEnemy enemy)
        {
            enemy.Launched -= OnEnemyLaunched;
            enemy.Died -= OnEnemyDied;
            enemy.Approached -= OnEnemyApproached;

            _globalPool.Free(enemy);

            //Removed(enemy);
        }

        private void OnEnemyApproached(BaseEnemy enemy)
        {
            Debug.Log("approached");
            Approached(enemy);
        }

        private void OnEnemyLaunched(BaseEnemy baseEnemy)
        {
            _aliveEnemyAmount++;
            _launchedEnemyAmount++;
            
            if(_launchedEnemyAmount == _enemyAmountInWave)
                EnableTracking();
        }

        public void Clear()
        {
            DisableTracking();

            foreach (BaseEnemy enemy in Enemies)
            {
                FreeEnemy(enemy);
                enemy.Die();
            }

            Enemies.Clear();
        }

        private void EnableTracking()
        {
            _isTrackingEnabled = true;
        }

        private void DisableTracking()
        {
            _isTrackingEnabled = false;
        }
    }
}