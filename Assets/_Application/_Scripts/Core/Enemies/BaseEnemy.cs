using System;
using System.Collections.Generic;
using _Application._Scripts.Scriptables.Core.Enemies;
using _Application.Scripts.Scriptables.Core.Enemies;
using PathCreation;
using Pool_And_Particles;
using UnityEngine;

namespace _Application._Scripts.Core.Enemies
{
    public class BaseEnemy : PooledBehaviour
    {
        public event Action<BaseEnemy> Launched = delegate { };
        public event Action<BaseEnemy> Died = delegate { };
        public event Action<BaseEnemy> Damaged = delegate { };
        public event Action<BaseEnemy> Approached = delegate { };
        public event Action<BaseEnemy> Updated = delegate { };

        [SerializeField] private Transform _barPoint;

        private BaseEnemyData _baseEnemyData;
        private bool _canMove;
        private VertexPath _path;
        private float _currentDistance;
        private Transform _transform;

        public Transform BarPoint => _barPoint;
        public float CurrentHealth { get; private set; }
        public float MaxHealth => _baseEnemyData.Health;
        public bool IsAlive { get; private set; }
        public List<DamageInfo> DefenceInfo => _baseEnemyData.DefenseInfo;

        private void Awake()
        {
            _transform = transform;
        }

        public void Initialize(BaseEnemyData baseEnemyData, VertexPath path)
        {
            _path = path;
            _baseEnemyData = baseEnemyData;
            _currentDistance = 0.0f;
            _canMove = false;

            CurrentHealth = _baseEnemyData.Health;
        }

        public void Launch()
        {
            IsAlive = true;
            _canMove = true;
            Launched(this);
        }
        
        private void Update()
        {
            if(_canMove == false)
                return;

            _currentDistance += Time.deltaTime * _baseEnemyData.MotionsSpeed;
            _transform.position = _path.GetPointAtDistance(_currentDistance);
            _transform.rotation = _path.GetRotationAtDistance(_currentDistance) * Quaternion.Euler(0,0,90);

            Updated(this);
                
            if (Mathf.Abs(_currentDistance - _path.length) < 1.0f)
            {
                IsAlive = false;
                _canMove = false;
                Approached(this);
            }
        }

        private void Die()
        {
            _canMove = false;
            IsAlive = false;
            Died(this);
        }

        public void TakeDamage(float damageAmount)
        {
            CurrentHealth = Mathf.Max(0f, CurrentHealth - damageAmount);
            Damaged(this);

            if (CurrentHealth <= float.Epsilon)
                Die();
        }
    }
}