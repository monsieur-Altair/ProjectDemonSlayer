using System.Collections.Generic;
using _Application._Scripts.Core;
using _Application._Scripts.Core.Enemies;
using _Application._Scripts.Core.Towers;
using _Application._Scripts.Scriptables.Core.Towers;
using _Application.Scripts.Infrastructure.Services;
using _Application.Scripts.Managers;
using _Application.Scripts.UI.Windows;
using Pool_And_Particles;
using Unity.VisualScripting;
using UnityEngine;

namespace _Application.Scripts.UI
{
    public class BarSpawner
    {
        private Transform _barParent;
        private readonly GlobalPool _pool;
        private readonly GlobalCamera _globalCamera;
        private readonly UnitBar _barPrefab;

        private readonly Dictionary<IDamagable, UnitBar> _unitsBars = new();
        private EnemyTracker _enemyTracker;
        private List<WarriorTower> _warriorTowers;
        private LevelManager _levelManager;

        public BarSpawner(CoreConfig coreConfig, GlobalPool pool, GlobalCamera globalCamera)
        {
            _globalCamera = globalCamera;
            _pool = pool;

            _barPrefab = coreConfig.Warehouse.BarPrefab;
        }

        public void Initialize(LevelManager levelManager)
        {
            _barParent = UISystem.GetWindow<GameplayWindow>().BarParent;

            _levelManager = levelManager;
            _enemyTracker = _levelManager.CurrentLevel.WaveManager.EnemyTracker;
            _warriorTowers = _levelManager.CurrentLevel.GetTowers<WarriorTower>(TowerType.WarriorTower);

            foreach (WarriorTower warriorTower in _warriorTowers)
            {
                warriorTower.WarriorAdded += AddBar;
                foreach (IDamagable damagable in warriorTower.Damagables) 
                    AddBar(damagable);
            }

            _levelManager.CurrentLevel.HeroAdded += AddBar;
            AddBar(_levelManager.CurrentLevel.BaseHero);
            
            _enemyTracker.EnemyAdded += AddBar;
        }

        public void Clear()
        {
            foreach (UnitBar counter in _unitsBars.Values) 
                _pool.Free(counter);
            
            foreach (WarriorTower warriorTower in _warriorTowers) 
                warriorTower.WarriorAdded -= AddBar;
            _enemyTracker.EnemyAdded -= AddBar;
            _levelManager.CurrentLevel.HeroAdded -= AddBar;

            _unitsBars.Clear();
            _enemyTracker = null;
        }

        private void OnDied(IDamagable damagable)
        {
            UnitBar unitBar = _unitsBars[damagable];
            _pool.Free(unitBar);
            _unitsBars.Remove(damagable);
            
            damagable.Updated -= UpdateCounterPos;
            damagable.Damaged -= UpdateBar;
            damagable.Died -= OnDied;
        }

        private void AddBar(IDamagable damagable)
        {
            UnitBar unitBar = _pool.Get(_barPrefab, parent: _barParent);
            _unitsBars.Add(damagable, unitBar);
            UpdateCounterPos(damagable);
            unitBar.gameObject.SetActive(false);

            damagable.Updated += UpdateCounterPos;
            damagable.Damaged += UpdateBar;
            damagable.Died += OnDied;
        }

        private void UpdateBar(IDamagable damagable)
        {
            UnitBar unitBar = _unitsBars[damagable];
            unitBar.gameObject.SetActive(true);
            float percent = Mathf.Clamp01(damagable.CurrentHealth / damagable.MaxHealth); 
            unitBar.UpdateBar(percent);
        }

        private void UpdateCounterPos(IDamagable damagable)
        {
            Vector2 counterPos = UISystem.GetUIPosition(_globalCamera.WorldCamera, damagable.BarPoint.position);
            _unitsBars[damagable].SetAnchorPos(counterPos);
        }
    }
}