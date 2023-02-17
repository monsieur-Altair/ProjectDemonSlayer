using System;
using _Application._Scripts.Core;
using _Application._Scripts.Core.Enemies;
using _Application._Scripts.Scriptables.Core.Levels;

namespace _Application.Scripts.Managers
{
    public class ElixirManager
    {
        public event Action ElixirAmountUpdated = delegate { };
        
        private readonly LevelData _levelData;
        private readonly EnemyTracker _enemyTracker;

        private int _elixirAmount;
        public int ElixirAmount
        {
            get => _elixirAmount;
            private set
            {
                _elixirAmount = value;
                ElixirAmountUpdated();
            }
        }

        public ElixirManager(CoreConfig coreConfig, int levelIndex, EnemyTracker enemyTracker)
        {
            _enemyTracker = enemyTracker;
            _levelData = coreConfig.LevelData[levelIndex];
            ElixirAmount = _levelData.StartElixirAmount;

            _enemyTracker.EnemyAdded += OnEnemyAdded;
        }

        public void Clear()
        {
            _enemyTracker.EnemyAdded -= OnEnemyAdded;

            foreach (BaseEnemy baseEnemy in _enemyTracker.Enemies) 
                Unsubscribe(baseEnemy);
        }

        public void DecreaseElixir(int cost)
        {
            ElixirAmount -= cost;
        }

        private void OnEnemyAdded(BaseEnemy baseEnemy)
        {
            baseEnemy.GrantedReward += IncreaseElixir;
            baseEnemy.Died += Unsubscribe;
        }

        private void Unsubscribe(IDamagable damagable)
        {
            if (damagable is BaseEnemy baseEnemy) 
                Unsubscribe(baseEnemy);
        }

        private void Unsubscribe(BaseEnemy baseEnemy)
        {
            baseEnemy.GrantedReward -= IncreaseElixir;
            baseEnemy.Died -= Unsubscribe;
        }

        private void IncreaseElixir(BaseEnemy baseEnemy) => 
            IncreaseElixir(baseEnemy.KillingReward);

        public void IncreaseElixir(int cost)
        {
            ElixirAmount += cost;
        }
    }
}