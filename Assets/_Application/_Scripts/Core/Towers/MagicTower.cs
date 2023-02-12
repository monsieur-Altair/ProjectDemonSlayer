using _Application._Scripts.Core.Enemies;
using _Application.Scripts.Managers;
using Pool_And_Particles;
using UnityEngine;

namespace _Application._Scripts.Core.Towers
{
    public class MagicTower : BaseTower
    {
        private MagicProjectile _projectilePrefab;
        
        public override void Initialize(CoreConfig coreConfig, EnemyTracker enemyTracker, GlobalPool globalPool)
        {
            base.Initialize(coreConfig, enemyTracker, globalPool);

            _projectilePrefab = _warehouse.ProjectilePrefabs[_towerType] as MagicProjectile;
        }
        
        protected override void Attack(BaseEnemy target)
        {
            base.Attack(target);
            
            Vector3 position = _spawnPoint.position;
            Quaternion rot = Quaternion.LookRotation(target.HitPoint.position - position);
            
            MagicProjectile projectile = _globalPool.Get(_projectilePrefab, position, rot);
            projectile.Initialize(_baseTowerData.AttackInfo, 7f, target, 0.4f, 0.6f);
            
            _projectileTracker.Add(target, projectile);

            Debug.Log("attacked");
        }
    }
}