using System;
using System.Collections.Generic;
using _Application._Scripts.Core.Towers;
using _Application._Scripts.Scriptables.Core.Enemies;
using _Application.Scripts.Scriptables.Core.Enemies;
using DG.Tweening;
using Extensions;
using PathCreation;
using Pool_And_Particles;
using UnityEngine;

namespace _Application._Scripts.Core.Enemies
{
    public class BaseEnemy : PooledBehaviour, IFindable, IDamagable
    {
        public event Action<BaseEnemy> Launched = delegate { };
        public event Action<IDamagable> Died = delegate { };
        public event Action<IDamagable> Damaged = delegate { };
        public event Action<IDamagable> Updated = delegate { };
        public event Action<BaseEnemy> Approached = delegate { };
        public event Action<BaseEnemy> GrantedReward = delegate { };

        [SerializeField] private Transform _barPoint;
        [SerializeField] private Transform _hitPoint;

        [SerializeField] private AnimatorController<EnemyAnimationState> _animatorController;

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
        public EnemyState CurrentEnemyState { get; private set; } = EnemyState.None;
        public int KillingReward => Mathf.RoundToInt(_baseEnemyData.KillingReward);

        private IDamagable _target;

        private readonly List<IDamagable> _targets = new();


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
            CurrentEnemyState = EnemyState.Running;
            _animatorController.PlayAnimation(EnemyAnimationState.Run);
            Launched(this);
        }

        public void Stop()
        {
            CurrentEnemyState = EnemyState.Waiting;
            _animatorController.PlayAnimation(EnemyAnimationState.Idle);
        }

        public void SlowDown(float newSpeed, float slowDur)
        {
            _speed = newSpeed;
            _slowingDownCor?.Kill();
            _slowingDownCor = TweenExt.Wait(slowDur).OnComplete(() => _speed = MotionSpeed);
        }

        public Vector3 GetFuturePos(float elapsedTime)
        {
            float speed = CurrentEnemyState == EnemyState.Running ? _speed : 0f;
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

        public void StartAttacking(IDamagable target)
        {
            if (_target == null)
                _target = target;
            else
                _targets.Add(target);

            target.Died += OnTargetDied;
            
            CurrentEnemyState = EnemyState.Attacking;
            _elapsedTime = _baseEnemyData.AttackCooldown + float.Epsilon;
        }

        private void OnTargetDied(IDamagable damagable)
        {
            damagable.Died -= OnTargetDied;

            if (damagable.Equals(_target))
            {
                if (_targets.Count == 0)
                {
                    _target = null;
                    CurrentEnemyState = EnemyState.Running;
                    _animatorController.PlayAnimation(EnemyAnimationState.Run);
                }
                else
                {
                    _target = _targets[0];
                    _targets.RemoveAt(0);
                }
            }
            else
            {
                _targets.Remove(damagable);
            }
        }

        private void Update()
        {
            switch (CurrentEnemyState)
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
                _animatorController.PlayAnimation(EnemyAnimationState.Attack);
            }
        }

        private void Run()
        {
            _currentDistance += Time.deltaTime * _speed;
            Transform.position = _path.GetPointAtDistance(_currentDistance);
            Transform.rotation = _path.GetRotationAtDistance(_currentDistance) * Quaternion.Euler(0, 0, 90);

            Updated(this);

            if (Mathf.Abs(_currentDistance - _path.length) < 0.3f)
            {
                CurrentEnemyState = EnemyState.None;
                _animatorController.PlayAnimation(EnemyAnimationState.Idle);
                Approached(this);
            }
        }

        public void Die()
        {
            foreach (IDamagable damagable in _targets) 
                damagable.Died -= OnTargetDied;

            _target = null;
            _targets.Clear();
            CurrentEnemyState = EnemyState.None;
            _animatorController.PlayAnimation(EnemyAnimationState.Death);
            GrantedReward(this);
            Died(this);
        }
    }

    public enum EnemyAnimationState
    {
        Idle, Attack, Run, Death
    }

    public enum EnemyState
    {
        None, 
        Running, 
        Waiting, 
        Attacking
    }
}