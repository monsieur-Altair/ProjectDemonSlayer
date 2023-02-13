using System.Collections.Generic;
using _Application._Scripts.Core.Enemies;
using Pool_And_Particles;

namespace _Application._Scripts.Core.Towers
{
    public class ProjectileTracker
    {
        private readonly Dictionary<IDamagable, List<BaseProjectile>> _projectileInfo = new();
        private readonly GlobalPool _globalPool;

        public ProjectileTracker(GlobalPool globalPool)
        {
            _globalPool = globalPool;
        }

        public void Add(BaseEnemy enemy, BaseProjectile baseProjectile)
        {
            if (_projectileInfo.ContainsKey(enemy) == false)
            {
                _projectileInfo.Add(enemy, new List<BaseProjectile>());
                Subscribe(enemy);
            }
            
            _projectileInfo[enemy].Add(baseProjectile);
            Subscribe(baseProjectile);
        }

        private void Subscribe(BaseProjectile baseProjectile)
        {
            baseProjectile.Damaged += DeleteProjectile;
        }

        public void Clear()
        {
            foreach ((IDamagable key, List<BaseProjectile> value) in _projectileInfo)
            {
                Unsubscribe(key);
                ClearList(value);
            }
            
            _projectileInfo.Clear();
        }

        private void Unsubscribe(BaseProjectile baseProjectile)
        {
            baseProjectile.Damaged -= DeleteProjectile;
        }

        private void DeleteProjectile(BaseProjectile baseProjectile)
        {
            List<BaseProjectile> projectiles = _projectileInfo[baseProjectile.Target];
            
            FreeProjectile(baseProjectile);

            projectiles.Remove(baseProjectile);
        }

        private void Subscribe(IDamagable damagable)
        {
            damagable.Died += DeleteList;
        }

        private void Unsubscribe(IDamagable damagable)
        {
            damagable.Died -= DeleteList;
        }

        private void FreeProjectile(BaseProjectile baseProjectile)
        {
            baseProjectile.Clear();
            Unsubscribe(baseProjectile);
            _globalPool.Free(baseProjectile);
        }

        private void DeleteList(IDamagable damagable)
        {
            BaseEnemy baseEnemy = damagable as BaseEnemy;

            ClearList(_projectileInfo[baseEnemy]);
            Unsubscribe(baseEnemy);
        }

        private void ClearList(List<BaseProjectile> projectileList)
        {
            foreach (BaseProjectile baseProjectile in projectileList) 
                FreeProjectile(baseProjectile);

            projectileList.Clear();
        }
    }
}