using _Application._Scripts.Core.Enemies;
using _Application._Scripts.Scriptables.Core.Towers;
using _Application.Scripts.Managers;
using _Application.Scripts.Misc;
using Pool_And_Particles;
using UnityEngine;

namespace _Application._Scripts.Core.Towers
{
    public class CannonTower : BaseTower
    {
        private CannonProjectile _cannonProjectilePrefab;
        private CannonTowerData _cannonTowerData;
        
        public override void Initialize(CoreConfig coreConfig, EnemyTracker enemyTracker, GlobalPool globalPool)
        {
            base.Initialize(coreConfig, enemyTracker, globalPool);
            
            _cannonProjectilePrefab = _warehouse.ProjectilePrefabs[_towerType] as CannonProjectile;
            _cannonTowerData = _baseTowerData as CannonTowerData;
        }

        protected override void Attack(BaseEnemy target)
        {
            base.Attack(target);

            const float g = 9.81f;
            Vector3 spawnPos = _spawnPoint.position;
            float h = spawnPos.y;
            float flightTime = Mathf.Sqrt(2 * h / g);
            Vector3 futurePos = target.GetFuturePos(flightTime);
            futurePos = futurePos.With(y: 0);
            float projectileHorizontalSpeed = Vector3.Distance(futurePos, spawnPos)/flightTime;
            
            Quaternion rot = Quaternion.LookRotation(futurePos - spawnPos);
            
            CannonProjectile projectile = _globalPool.Get(_cannonProjectilePrefab, spawnPos, rot);
            projectile.Initialize(_baseTowerData.AttackInfo, projectileHorizontalSpeed, target, _powerCoefficient,
                futurePos, _cannonTowerData.ExplosionRadius, _enemyTracker, flightTime);
            
            _projectileTracker.Add(target, projectile);
        }
    }
}