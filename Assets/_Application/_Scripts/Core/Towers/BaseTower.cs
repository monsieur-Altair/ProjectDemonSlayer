using _Application._Scripts.Core.Enemies;
using _Application._Scripts.Scriptables.Core.Towers;
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

        [SerializeField] private float _radius;

        protected ProjectileTracker _projectileTracker;
        protected BaseTowerData _baseTowerData;
        protected GlobalPool _globalPool;
        protected Warehouse _warehouse;

        protected bool _isEnabled;
        protected EnemyTracker _enemyTracker;
        protected float _elapsedTime;


        protected virtual bool CanAttack => true;

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            const int MaxCount = 100;
            Gizmos.color = Color.red;
            for (int i = 0; i < MaxCount; i++)
            {
                Vector2 dir0 = _radius * GetPos(i/(float)MaxCount*Mathf.PI * 2);
                Vector2 dir1 = _radius * GetPos((i+1)/(float)MaxCount*Mathf.PI * 2);
                Vector3 pos = transform.position;
                Vector3 pos0 = pos.With(x: pos.x + dir0.x, z: pos.z + dir0.y);
                Vector3 pos1 = pos.With(x: pos.x + dir1.x, z: pos.z + dir1.y);
                Gizmos.DrawLine(pos0, pos1);
            }
        }

        private static Vector2 GetPos(float angle) => 
            new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).normalized; 
#endif
        
        public virtual void Initialize(CoreConfig coreConfig, EnemyTracker enemyTracker, GlobalPool globalPool)
        {
            _warehouse = coreConfig.Warehouse;
            _globalPool = globalPool;
            _elapsedTime = 0f;
            _enemyTracker = enemyTracker;
            _baseTowerData = coreConfig.TowersData[_towerType];
            _radius = _baseTowerData.Radius;
            _projectileTracker = new ProjectileTracker(globalPool);
        }

        public void Clear()
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
                BaseEnemy target = CoreMethods.FindClosest(_enemyTracker.Enemies, _baseTowerData.Radius, transform);
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