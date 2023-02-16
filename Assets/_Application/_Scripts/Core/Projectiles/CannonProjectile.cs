using System.Collections.Generic;
using _Application._Scripts.Core.Enemies;
using _Application.Scripts.Misc;
using _Application.Scripts.Scriptables.Core.Enemies;
using UnityEngine;

namespace _Application._Scripts.Core.Towers
{
    public class CannonProjectile : BaseProjectile
    {
        private float _explosionRadius;
        private Vector3 _targetPos;
        private EnemyTracker _enemyTracker;
        private float _elapsedTime;
        private Vector3 _startPos;
        private float _flightTime;
        private float _powerCoefficient;
        private float HorizontalSpeed => _speed;

        public void Initialize(List<DamageInfo> attackInfo, float horizontalSpeed, BaseEnemy target, float powerCoefficient, 
            Vector3 targetPos, float explosionRadius, EnemyTracker enemyTracker, float flightTime)
        {
            _flightTime = flightTime;
            _powerCoefficient = powerCoefficient;
            base.Initialize(attackInfo, horizontalSpeed, target, _powerCoefficient);

            _targetPos = targetPos;
            _enemyTracker = enemyTracker;
            _explosionRadius = explosionRadius;

            _startPos = _transform.position;
            _elapsedTime = 0.0f;
        }
        
        protected override void UpdatePosition()
        {
            _elapsedTime += Time.deltaTime;
            
            Vector3 endPos = _targetPos;
            Vector3 dir = (endPos - _startPos).normalized;
            Vector3 newPos = _startPos + dir * _elapsedTime * HorizontalSpeed;
            float newY = _startPos.y - 9.81f * _elapsedTime * _elapsedTime / 2f;
            _transform.position = newPos.With(y: newY);

            if (Mathf.Abs(_elapsedTime - _flightTime) < 0.05f)
                DamageTarget();
        }

        protected override void DamageTarget()
        {
            List<BaseEnemy> enemiesInRange = CoreMethods.FindInRange(_enemyTracker.Enemies, transform, _explosionRadius);

            foreach (BaseEnemy enemy in enemiesInRange)
            {
                float damageAmount = CoreMethods.CalculateDamage(_attackInfo, enemy.DefenceInfo);
                enemy.TakeDamage(damageAmount * _powerCoefficient);    
            }
            
            OnDamaged();
        }
    }
}