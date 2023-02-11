using System;
using _Application._Scripts.Scriptables.Core.Enemies;
using UnityEngine;

namespace _Application._Scripts.Scriptables.Core.Levels
{
    [Serializable]
    public class MiniWaveData
    {
        [SerializeField] private EnemyType _enemyType;
        [SerializeField] private int _enemyCount;
        [SerializeField] private float _miniWaveSpawnDelay;
        
        public EnemyType EnemyType => _enemyType;
        public int EnemyCount => _enemyCount;
        public float MiniWaveSpawnDelay => _miniWaveSpawnDelay;
    }
}