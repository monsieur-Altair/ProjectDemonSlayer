using System;
using System.Collections.Generic;
using _Application._Scripts.Scriptables.Core.Enemies;
using _Application.Scripts.Scriptables.Core.Enemies;
using DG.Tweening;
using Extensions;
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
        [SerializeField] private Transform _hitPoint;

        private BaseEnemyData _baseEnemyData;
        private bool _canMove;
        private VertexPath _path;
        private float _currentDistance;

        private float _speed;
        private Tweener _slowingDownCor;
        public Transform Transform { get; private set; }
        public Transform BarPoint => _barPoint;
        public float CurrentHealth { get; private set; }
        public float MaxHealth => _baseEnemyData.Health;
        public bool IsAlive { get; private set; }
        public List<DamageInfo> DefenceInfo => _baseEnemyData.DefenseInfo;
        public Transform HitPoint => _hitPoint;
        public float MotionSpeed => _baseEnemyData.MotionsSpeed;

        private void Awake()
        {
            Transform = transform;
        }

        public void Initialize(BaseEnemyData baseEnemyData, VertexPath path)
        {
            _path = path;
            _baseEnemyData = baseEnemyData;
            _currentDistance = 0.0f;
            _canMove = false;
            _speed = MotionSpeed;

            CurrentHealth = _baseEnemyData.Health;
        }

        public void Launch()
        {
            IsAlive = true;
            _canMove = true;
            Launched(this);
        }

        public void SlowDown(float newSpeed, float slowDur)
        {
            _speed = newSpeed;
            _slowingDownCor?.Kill();
            _slowingDownCor = TweenExt.Wait(slowDur).OnComplete(() => _speed = MotionSpeed);
        }

        public Vector3 GetFuturePos(float elapsedTime)
        {
            float futureDistance = _currentDistance + elapsedTime * _speed;
            float t = Mathf.Clamp(futureDistance / _path.length, 0, 0.995f);
            return _path.GetPointAtTime(t);
        }

        public void TakeDamage(float damageAmount)
        {
            CurrentHealth = Mathf.Max(0f, CurrentHealth - damageAmount);
            Damaged(this);

            if (CurrentHealth <= float.Epsilon)
                Die();
        }

        private void Update()
        {
            if(_canMove == false)
                return;

            _currentDistance += Time.deltaTime * _speed;
            Transform.position = _path.GetPointAtDistance(_currentDistance);
            Transform.rotation = _path.GetRotationAtDistance(_currentDistance) * Quaternion.Euler(0,0,90);

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
    }
}