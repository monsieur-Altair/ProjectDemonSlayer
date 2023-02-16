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
    public class BaseTower : MonoBehaviour
    {
        [SerializeField] protected Transform _spawnPoint;
        [SerializeField] protected TowerType _towerType;
        
        protected ProjectileTracker _projectileTracker;
        protected BaseTowerData _baseTowerData;
        protected GlobalPool _globalPool;
        protected Warehouse _warehouse;

        protected bool _isEnabled;
        protected EnemyTracker _enemyTracker;
        protected float _elapsedTime;
        protected CoreConfig _coreConfig;
        protected float _powerCoefficient;

        [SerializeField] private float _radius = 5f;

        private ProgressService _progressService;
        private float _healthCoefficient;
        private float _rangeCoefficient;

        public float Health => _baseTowerData.Health * _healthCoefficient;
        public TowerType TowerType => _towerType;
        
        
        protected float Radius => _baseTowerData.Radius * _rangeCoefficient;
        protected virtual bool CanAttack => true;


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

            int upgradeLevel = _progressService.PlayerProgress.TowersUpgrades
                .First(upgrade => upgrade.TowerType == _towerType).AchievedLevel;
            
            FillCoefficients(coreConfig, upgradeLevel);
            
            _coreConfig = coreConfig;
            _warehouse = coreConfig.Warehouse;
            _globalPool = globalPool;
            _elapsedTime = 0f;
            _enemyTracker = enemyTracker;
            _baseTowerData = coreConfig.TowersData[_towerType];
            _projectileTracker = new ProjectileTracker(globalPool);
        }

        private void FillCoefficients(CoreConfig coreConfig, int upgradeLevel)
        {
            BaseTowerUpgradeData[] upgradeData = coreConfig.TowersUpgrades[_towerType];

            if (upgradeLevel == -1)
            {
                _healthCoefficient = 1f;
                _rangeCoefficient = 1f;
                _powerCoefficient = 1f;
            }
            else
            {
                _healthCoefficient = upgradeData[upgradeLevel].Health;
                _rangeCoefficient  = upgradeData[upgradeLevel].Range;
                _powerCoefficient  = upgradeData[upgradeLevel].Power;
            }
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
            
        }

        public void Enable()
        {
            _isEnabled = true;
        }

        public void Disable()
        {
            _isEnabled = false;
        }
    }
}