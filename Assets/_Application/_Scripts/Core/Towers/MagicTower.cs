using _Application._Scripts.Core.Enemies;
using _Application._Scripts.Scriptables.Core.Towers;
using _Application.Scripts.Managers;
using Pool_And_Particles;
using UnityEngine;

namespace _Application._Scripts.Core.Towers
{
    public class MagicTower : BaseTower
    {
        private MagicProjectile _projectilePrefab;
        private MagicTowerData _magicTowerData;
        
        public override void Initialize(CoreConfig coreConfig, EnemyTracker enemyTracker, GlobalPool globalPool)
        {
            base.Initialize(coreConfig, enemyTracker, globalPool);

            _projectilePrefab = _warehouse.ProjectilePrefabs[_towerType] as MagicProjectile;
            _magicTowerData = _baseTowerData as MagicTowerData;
        }
        
        protected override void Attack(BaseEnemy target)
        {
            base.Attack(target);
            
            Vector3 position = _spawnPoint.position;
            Quaternion rot = Quaternion.LookRotation(target.FindPoint.position - position);
            
            MagicProjectile projectile = _globalPool.Get(_projectilePrefab, position, rot);
            projectile.Initialize(_baseTowerData.AttackInfo, _magicTowerData.ProjectileSpeed, target, _powerCoefficient, 
                _magicTowerData.SlowCoefficient, _magicTowerData.SlowDur);
            
            _projectileTracker.Add(target, projectile);
        }
    }
}