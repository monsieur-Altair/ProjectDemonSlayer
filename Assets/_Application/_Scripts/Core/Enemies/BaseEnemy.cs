using System;
using System.Collections.Generic;
using _Application._Scripts.Core.Towers;
using _Application._Scripts.Scriptables.Core.Enemies;
using _Application._Scripts.Scriptables.Core.UnitsBehaviour;
using _Application.Scripts.Scriptables.Core.Enemies;
using DG.Tweening;
using Extensions;
using PathCreation;
using Pool_And_Particles;
using UnityEngine;

namespace _Application._Scripts.Core.Enemies
{
    public class BaseEnemy : PooledBehaviour, IFindable
    {
        public event Action<BaseEnemy> Launched = delegate { };
        public event Action<BaseEnemy> Died = delegate { };
        public event Action<BaseEnemy> Damaged = delegate { };
        public event Action<BaseEnemy> Approached = delegate { };
        public event Action<BaseEnemy> Updated = delegate { };

        [SerializeField] private Transform _barPoint;
        [SerializeField] private Transform _hitPoint;

        private BaseEnemyData _baseEnemyData;
        private VertexPath _path;
        private float _currentDistance;

        private float _elapsedTime;
        private float _speed;
        private Tweener _slowingDownCor;
        public Transform Transform { get; private set; }
        public Transform BarPoint => _barPoint;
        public float CurrentHealth { get; private set; }
        public float MaxHealth => _baseEnemyData.Health;
        public List<DamageInfo> DefenceInfo => _baseEnemyData.DefenseInfo;
        public Transform FindPoint => _hitPoint;
        public float MotionSpeed => _baseEnemyData.MotionsSpeed;
        public EnemyBehaviourType BehaviourType => _baseEnemyData.BehaviourType;

        private EnemyState _currentEnemyState = EnemyState.None;
        private BaseUnit _target;


        private void Awake()
        {
            Transform = transform;
        }

        public void Initialize(BaseEnemyData baseEnemyData, VertexPath path)
        {
            _path = path;
            _baseEnemyData = baseEnemyData;
            _currentDistance = 0.0f;
            _speed = MotionSpeed;

            CurrentHealth = _baseEnemyData.Health;
        }

        public void Launch()
        {
            _currentEnemyState = EnemyState.Running;
            Launched(this);
        }

        public void Stop()
        {
            _currentEnemyState = EnemyState.Waiting;
        }

        public void SlowDown(float newSpeed, float slowDur)
        {
            _speed = newSpeed;
            _slowingDownCor?.Kill();
            _slowingDownCor = TweenExt.Wait(slowDur).OnComplete(() => _speed = MotionSpeed);
        }

        public Vector3 GetFuturePos(float elapsedTime)
        {
            float speed = _currentEnemyState == EnemyState.Running ? _speed : 0f;
            float futureDistance = _currentDistance + elapsedTime * speed;
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

        public void StartAttacking(BaseUnit target)
        {
            _target = target;
            
            //todo: Subscribe for death and handle many targets   
            
            _currentEnemyState = EnemyState.Attacking;
            _elapsedTime = _baseEnemyData.AttackCooldown + float.Epsilon;
        }

        private void Update()
        {
            switch (_currentEnemyState)
            {
                case EnemyState.None:
                    break;
                case EnemyState.Running:
                    Run();
                    break;
                case EnemyState.Waiting:
                    break;
                case EnemyState.Attacking:
                    Attack();
                    break;
                default:
                    Debug.LogError("aaaaaaaaaaa");
                    break;
            }
        }

        private void Attack()
        {
            _elapsedTime += Time.deltaTime;

            if (_elapsedTime >= _baseEnemyData.AttackCooldown)
            {
                _elapsedTime = 0f;
                float damage = CoreMethods.CalculateDamage(_baseEnemyData.AttackInfo, _target.DefenceInfo);
                _target.TakeDamage(damage);
            }
        }

        private void Run()
        {
            _currentDistance += Time.deltaTime * _speed;
            Transform.position = _path.GetPointAtDistance(_currentDistance);
            Transform.rotation = _path.GetRotationAtDistance(_currentDistance) * Quaternion.Euler(0, 0, 90);

            Updated(this);

            if (Mathf.Abs(_currentDistance - _path.length) < 1.0f)
            {
                _currentEnemyState = EnemyState.None;
                Approached(this);
            }
        }

        private void Die()
        {
            _currentEnemyState = EnemyState.None;
            Died(this);
        }
    }

    public enum EnemyState
    {
        None, 
        Running, 
        Waiting, 
        Attacking
    }
}