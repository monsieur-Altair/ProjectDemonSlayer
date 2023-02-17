using _Application._Scripts.Core.Enemies;
using _Application.Scripts.Managers;
using Pool_And_Particles;
using UnityEngine;

namespace _Application._Scripts.Core.Towers
{
    public class ArcherTower : BaseTower
    {
        private BaseProjectile _projectilePrefab;

        public override void Initialize(CoreConfig coreConfig, EnemyTracker enemyTracker, GlobalPool globalPool)
        {
            base.Initialize(coreConfig, enemyTracker, globalPool);

            _projectilePrefab = _warehouse.ProjectilePrefabs[_towerType];
        }

        protected override void Attack(BaseEnemy target)
        {
            base.Attack(target);
            
            Vector3 position = _spawnPoint.position;
            Quaternion rot = Quaternion.LookRotation(target.FindPoint.position - position);
            
            BaseProjectile baseProjectile = _globalPool.Get(_projectilePrefab, position, rot);
            baseProjectile.Initialize(_baseTowerData.AttackInfo, _baseTowerData.ProjectileSpeed, target, 
                _upgradeData.PowerCoefficient);
            
            _projectileTracker.Add(target, baseProjectile);
        }
    }
}