using System.Collections.Generic;
using _Application._Scripts.Core;
using _Application._Scripts.Core.Enemies;
using _Application.Scripts.Infrastructure.Services;
using _Application.Scripts.Managers;
using _Application.Scripts.UI.Windows;
using Pool_And_Particles;
using UnityEngine;

namespace _Application.Scripts.UI
{
    public class BarSpawner
    {
        private Transform _barParent;
        private readonly GlobalPool _pool;
        private readonly GlobalCamera _globalCamera;
        private readonly UnitBar _barPrefab;

        private readonly Dictionary<BaseEnemy, UnitBar> _unitsBars = new();
        private EnemyTracker _enemyTracker;

        public BarSpawner(CoreConfig coreConfig, GlobalPool pool, GlobalCamera globalCamera)
        {
            _globalCamera = globalCamera;
            _pool = pool;

            _barPrefab = coreConfig.Warehouse.BarPrefab;
        }

        public void Initialize(EnemyTracker enemyTracker)
        {
            _enemyTracker = enemyTracker;
            _barParent = UISystem.GetWindow<GameplayWindow>().BarParent;

            _enemyTracker.Added += OnEnemyAdded;
            _enemyTracker.Removed += OnEnemyRemoved;
        }

        public void Clear()
        {
            foreach (UnitBar counter in _unitsBars.Values) 
                _pool.Free(counter);

            _unitsBars.Clear();

            _enemyTracker.Added -= OnEnemyAdded;
            _enemyTracker.Removed -= OnEnemyRemoved;

            _enemyTracker = null;
        }

        private void OnEnemyRemoved(BaseEnemy enemy)
        {
            UnitBar unitBar = _unitsBars[enemy];
            _pool.Free(unitBar);
            _unitsBars.Remove(enemy);
            
            enemy.Updated -= UpdateCounterPos;
            enemy.Damaged -= UpdateBar;
        }

        private void OnEnemyAdded(BaseEnemy enemy)
        {
            UnitBar unitBar = _pool.Get(_barPrefab, parent: _barParent);
            _unitsBars.Add(enemy, unitBar);
            UpdateCounterPos(enemy);
            unitBar.gameObject.SetActive(false);

            enemy.Updated += UpdateCounterPos;
            enemy.Damaged += UpdateBar;
        }

        private void UpdateBar(BaseEnemy enemy)
        {
            UnitBar unitBar = _unitsBars[enemy];
            unitBar.gameObject.SetActive(true);
            float percent = Mathf.Clamp01(enemy.CurrentHealth / enemy.MaxHealth); 
            unitBar.UpdateBar(percent);
        }

        private void UpdateCounterPos(BaseEnemy enemy)
        {
            Vector2 counterPos = UISystem.GetUIPosition(_globalCamera.MainCamera, enemy.BarPoint.position);
            _unitsBars[enemy].SetAnchorPos(counterPos);
        }
    }
}