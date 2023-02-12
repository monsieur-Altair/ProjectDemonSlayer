using System;
using System.Collections.Generic;
using System.Linq;
using _Application._Scripts.Core.Enemies;
using _Application.Scripts.Infrastructure.Services;
using _Application.Scripts.Scriptables.Core.Enemies;
using Pool_And_Particles;
using UnityEngine;

namespace _Application._Scripts.Core.Towers
{
    public class BaseProjectile : PooledBehaviour
    {
        public event Action<BaseProjectile> Damaged = delegate {  };

        protected float _speed;
        protected List<DamageInfo> _attackInfo;
        protected Transform _transform;
        protected GlobalPool _globalPool;
        public BaseEnemy Target { get; private set; }

        private void Awake()
        {
            _transform = transform;
            _globalPool = AllServices.Get<GlobalPool>();
        }

        public void Initialize(List<DamageInfo> attackInfo, float speed, BaseEnemy target)
        {
            Target = target;
            _attackInfo = attackInfo;
            _speed = speed;
        }

        public void Clear()
        {
            Target = null;
            _attackInfo = null;
            _speed = 0.0f;
        }
        
        private void Update()
        {
            if (Target == null)
                return;
            
            UpdatePosition();
        }

        protected virtual void UpdatePosition()
        {
            Vector3 endPos = Target.HitPoint.position;
            Vector3 newPos = GetPosition(_transform.position, endPos, _speed * Time.deltaTime);
            _transform.position = newPos;
            _transform.rotation = Quaternion.LookRotation(endPos - newPos);

            if (Vector3.Distance(endPos, newPos) < 0.1f)
                DamageTarget();
        }

        protected virtual void DamageTarget()
        {
            float damageAmount = CalculateDamage(_attackInfo, Target.DefenceInfo);
            Target.TakeDamage(damageAmount);
            OnDamaged();
        }

        protected void OnDamaged() => 
            Damaged(this);

        protected static float CalculateDamage(List<DamageInfo> attackInfos, List<DamageInfo> defenceInfos)
        {
            float result = 0.0f;

            foreach (DamageInfo attackInfo in attackInfos)
            {
                DamageInfo defenceInfo = defenceInfos.FirstOrDefault(defenceInfo => defenceInfo.DamageType == attackInfo.DamageType);
                float defenceValue = defenceInfo?.Value ?? 0.0f;
                result += attackInfo.Value * (1 - defenceValue);
            }

            return result;
        }

        protected static Vector3 GetPosition(Vector3 curr, Vector3 end, float maxDistanceDelta) => 
            Vector3.MoveTowards(curr, end, maxDistanceDelta);
    }
}