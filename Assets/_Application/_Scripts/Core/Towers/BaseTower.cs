using System;
using System.Collections.Generic;
using System.Linq;
using _Application._Scripts.Core.Enemies;
using _Application._Scripts.Scriptables.Core.Towers;
using _Application._Scripts.Scriptables.Core.TowerUpgrade;
using _Application.Scripts.Infrastructure.Services;
using _Application.Scripts.Infrastructure.Services.Progress;
using _Application.Scripts.Managers;
using _Application.Scripts.Misc;
using Pool_And_Particles;
using UnityEngine;

namespace _Application._Scripts.Core.Towers
{
    public class BaseTower : PooledBehaviour
    {
        [SerializeField] protected Transform _spawnPoint;
        [SerializeField] protected TowerType _towerType;
        [SerializeField] private GameObject[] _levelVisuals;
        
        protected ProjectileTracker _projectileTracker;
        protected BaseTowerData _baseTowerData;
        protected GlobalPool _globalPool;
        protected Warehouse _warehouse;

        protected bool _isEnabled;
        protected EnemyTracker _enemyTracker;
        protected float _elapsedTime;
        protected CoreConfig _coreConfig;
        protected BaseTowerUpgradeData _upgradeData;

        [SerializeField] private float _radius = 5f;

        private ProgressService _progressService;

        public int TowerLevel { get; private set; }
        public float Health => _baseTowerData.Health * _upgradeData.HealthCoefficient;
        public TowerType TowerType => _towerType;
        protected float Radius => _baseTowerData.Radius * _upgradeData.RangeCoefficient;
        protected virtual bool CanAttack => true;
        public bool IsMaxLevel => TowerLevel >= CoreConfig.TowerLevelAmount - 1;

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            const int MaxCount = 100;
            Gizmos.color = Color.red;
            for (int i = 0; i < MaxCount; i++)
            {
                Vector2 dir0 = _radius * BaseExtensions.GetPos(i/(float)MaxCount*Mathf.PI * 2);
                Vector2 dir1 = _radius * BaseExtensions.GetPos((i+1)/(float)MaxCount*Mathf.PI * 2);
                Vector3 pos = transform.position;
                Vector3 pos0 = pos.With(x: pos.x + dir0.x, z: pos.z + dir0.y);
                Vector3 pos1 = pos.With(x: pos.x + dir1.x, z: pos.z + dir1.y);
                Gizmos.DrawLine(pos0, pos1);
            }
        }
#endif

        public virtual void Initialize(CoreConfig coreConfig, EnemyTracker enemyTracker, GlobalPool globalPool)
        {
            _progressService = AllServices.Get<ProgressService>();
            TowerLevel = -1;
            _coreConfig = coreConfig;
            _warehouse = coreConfig.Warehouse;
            _globalPool = globalPool;
            _elapsedTime = 0f;
            _enemyTracker = enemyTracker;
            _projectileTracker = new ProjectileTracker(globalPool);
            
            Upgrade();

            Debug.Log($"INIT {_towerType}");
        }

        protected virtual void UpdateVisual()
        {
            foreach (GameObject levelVisual in _levelVisuals) 
                levelVisual.SetActive(false);
            
            _levelVisuals[TowerLevel].SetActive(true);
        }

        public void Upgrade()
        {
            TowerLevel++;
            _baseTowerData = _coreConfig.TowersData[_towerType][TowerLevel];
            ApplyUpgrades();
            UpdateVisual();
        }
        
        private void ApplyUpgrades()
        {
            int upgradeLevel = _progressService.PlayerProgress.TowersUpgrades
                .First(upgrade => upgrade.TowerType == _towerType).AchievedLevels[TowerLevel];
            
            _upgradeData = _coreConfig.TowersUpgradesLists[_towerType][TowerLevel].List[upgradeLevel];
        }

        public virtual void Clear()
        {
            _projectileTracker.Clear();
        }
        
        protected virtual void Update()
        {
            if (_isEnabled == false)
                return;

            _elapsedTime += Time.deltaTime;

            if (_elapsedTime >= _baseTowerData.AttackCooldown && CanAttack)
            {
                BaseEnemy target = CoreMethods.FindClosest(_enemyTracker.Enemies, Radius, transform);
                if (target != null)
                {
                    _elapsedTime = 0f;
                    Attack(target);
                }
            }
        }

        protected virtual void Attack(BaseEnemy target)
        {
            Debug.Log($"att {_towerType}");


        }

        public void Enable()
        {
            _isEnabled = true;
            
            Debug.Log($"enn {_towerType}");
        }

        public void Disable()
        {
            Debug.Log($"dis {_towerType}");
            _isEnabled = false;
        }
    }
}