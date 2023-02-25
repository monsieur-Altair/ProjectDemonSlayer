using System;
using System.Collections.Generic;
using System.Linq;
using _Application._Scripts.Core;
using _Application._Scripts.Core.Towers;
using _Application._Scripts.Scriptables.Core.Towers;
using Pool_And_Particles;
using UnityEngine;

namespace _Application.Scripts.Managers
{
    public class TowersManager
    {
        public event Action<BaseTower> AddedTower = delegate {  };
        public event Action<BaseTower> DestroyedTower = delegate {  };
        
        private readonly List<BaseTower> _allTowers;
        private readonly WaveManager _waveManager;
        private readonly GlobalPool _globalPool;
        private readonly CoreConfig _coreConfig;
        private readonly Transform _parent;

        private bool _isAttacking;

        public TowersManager(CoreConfig coreConfig, GlobalPool globalPool, WaveManager waveManager, Transform parent)
        {
            _parent = parent;
            _coreConfig = coreConfig;
            _globalPool = globalPool;
            _waveManager = waveManager;
            _allTowers = new List<BaseTower>();
        }

        public void BuildBuilding(TowerType towerType, BuildPlace buildPlace)
        {
            BaseTower prefab = _coreConfig.Warehouse.TowersPrefabs[towerType];
            
            BaseTower builtTower = _globalPool.Get(prefab, position: buildPlace.BuildPoint.position, parent: _parent);
            builtTower.Initialize(_coreConfig, _waveManager.EnemyTracker, _globalPool);
            
            buildPlace.DefaultVisual.SetActive(false);
            buildPlace.SetTower(builtTower);
            
            _allTowers.Add(builtTower);
            
            if(_isAttacking)
                builtTower.Enable();
            
            AddedTower(builtTower);
        }

        public void DestroyBuilding(BuildPlace buildPlace)
        {
            _allTowers.Remove(buildPlace.CurrentTower);
         
            buildPlace.DefaultVisual.SetActive(true);
            BaseTower tower = buildPlace.CurrentTower;
            tower.Disable();
            tower.Clear();
            _globalPool.Free(tower);
            buildPlace.SetTower(null);
            DestroyedTower(tower);
        }

        public List<TBaseTower> GetTowers<TBaseTower>(TowerType towerType) where TBaseTower : BaseTower
        {
            return _allTowers
                .Where(tower => tower.TowerType == towerType)
                .Cast<TBaseTower>()
                .ToList();
        }

        public void Clear()
        {
            foreach (BaseTower tower in _allTowers)
            {
                tower.Disable();
                tower.Clear();
                _globalPool.Free(tower);
                DestroyedTower(tower);
            }
            
            _allTowers.Clear();
        }

        public void StopAttacking()
        {
            _isAttacking = false;
        }

        public void StartAttacking()
        {
            _isAttacking = true;
            
            foreach (BaseTower tower in _allTowers) 
                tower.Enable();
        }
    }
}