using System;
using System.Collections.Generic;
using _Application._Scripts.Core.Enemies;
using _Application._Scripts.Scriptables.Core.Enemies;
using _Application.Scripts.Managers;
using _Application.Scripts.Scriptables.Core.Enemies;
using Pool_And_Particles;
using UnityEngine;

namespace _Application._Scripts.Scriptables.Core.UnitsBehaviour
{
    public class BaseUnit : PooledBehaviour, IDamagable
    {
        private static readonly int Attack = Animator.StringToHash("Attack");
        private static readonly int Run = Animator.StringToHash("Run");
        private static readonly int Idle = Animator.StringToHash("Idle");        
        private static readonly int Die1 = Animator.StringToHash("Die");
        private static readonly int Revive = Animator.StringToHash("Revive");

        public event Action<BaseUnit> Appeared = delegate {  };
        public event Action<IDamagable> Died = delegate { };
        public event Action<IDamagable> Updated = delegate {  };
        public event Action<IDamagable> Damaged = delegate { };
        
        [SerializeField] private Transform _barPoint;
        [SerializeField] private Animator _animator;

        protected UnitStateMachine _stateMachine;

        protected BaseUnitData BaseUnitData { get; set; }
        public BaseEnemy Target { get; private set; }
        public float CloseAttackRadius => BaseUnitData.CloseAttackRadius;
        public List<DamageInfo> DefenceInfo => BaseUnitData.DefenseInfo;
        public Transform Transform { get; private set; }

        public bool IsBusy { get; private set; }
        public bool IsAlive { get; private set; }
        public float CurrentHealth { get; private set; }
        public Transform BarPoint => _barPoint;
        public float ReviveDuration => BaseUnitData.ReviveDur;
        public float AttackCooldown => BaseUnitData.AttackCooldown;
        public virtual List<DamageInfo> AttackInfo => BaseUnitData.AttackInfo;
        public Animator Animator => _animator;
        public virtual float PowerCoefficient => 1f;
        public virtual float MaxHealth => BaseUnitData.Health;
        public virtual float MotionsSpeed => BaseUnitData.MotionsSpeed;
        private bool CanStopEnemy => Target != null 
                                     && Target.CurrentEnemyState == EnemyState.Running 
                                     && Target.BehaviourType != EnemyBehaviourType.Runner;

        protected virtual void Awake()
        {
            Transform = transform;
        }

        private void Update()
        {
            _stateMachine.Update();
            OnUpdated();
            Updated(this);
        }

        protected virtual void OnUpdated()
        {
            
        }

        public virtual void Initialize(CoreConfig coreConfig)
        {
            FetchData(coreConfig);
            CreateStateMachine();
            RestoreHp();
            OnAppeared();
        }

        protected virtual void FetchData(CoreConfig coreConfig)
        {
        }

        protected virtual void CreateStateMachine()
        {
            _stateMachine = new UnitStateMachine(this);
            _stateMachine.Enter<IdleState>();
        }

        public virtual void Clear()
        {
            _stateMachine.ResetState();
            Died(this);
        }

        public void SetBusy(bool isBusy)
        {
            IsBusy = isBusy;
        }

        public void SetIsAlive(bool isAlive)
        {
            IsAlive = isAlive;
        }

        public void SetTarget(BaseEnemy target)
        {
            Target = target;
        }

        public void StopTarget()
        {
            if (CanStopEnemy)
                Target.Stop();
        }

        public void GoToTarget()
        {
            _stateMachine.Enter<MoveToTargetState>();
        }

        public void RestoreHp()
        {
            CurrentHealth = MaxHealth;
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
            _stateMachine.Enter<AttackState>();
        }

        public void OnAppeared() => 
            Appeared(this);

        private void Die()
        {
            _stateMachine.Enter<DeathState>();
            Died(this);
        }

        public void PlayAttackAnimation()
        {
            Animator.ResetTrigger(Attack);
            Animator.SetTrigger(Attack);
        }

        public void PlayRunAnimation()
        {
            Animator.ResetTrigger(Run);
            Animator.SetTrigger(Run);
        }

        public void PlayIdleAnimation()
        {
            Animator.ResetTrigger(Idle);
            Animator.SetTrigger(Idle);
        }

        public void PlayDeathAnimation()
        {
            Animator.ResetTrigger(Die1);
            Animator.SetTrigger(Die1);
        }

        public void PlayReviveAnimation()
        {
            Animator.ResetTrigger(Revive);
            Animator.SetTrigger(Revive);
        }
    }
}

