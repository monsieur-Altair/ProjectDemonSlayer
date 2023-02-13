using System;
using System.Collections.Generic;
using _Application._Scripts.Core.Enemies;
using _Application.Scripts.Managers;
using _Application.Scripts.Scriptables.Core.Enemies;
using Pool_And_Particles;
using UnityEngine;

namespace _Application._Scripts.Scriptables.Core.UnitsBehaviour
{
    public class BaseUnit : PooledBehaviour, IDamagable
    {
        public event Action<BaseUnit> Appeared = delegate {  };
        public event Action<IDamagable> Died = delegate { };
        public event Action<IDamagable> Updated = delegate {  };
        public event Action<IDamagable> Damaged = delegate { };
        
        [SerializeField] private float _closeAttackRadius = 1f;
        [SerializeField] private float _reviveDuration;
        [SerializeField] private Transform _barPoint;

        protected UnitStateMachine _stateMachine;

        public BaseUnitData BaseUnitData { get; protected set; }
        public BaseEnemy Target { get; private set; }
        public float CloseAttackRadius => _closeAttackRadius;
        public List<DamageInfo> DefenceInfo => BaseUnitData.DefenseInfo;
        public Transform Transform { get; private set; }

        public bool IsBusy { get; private set; }
        public bool IsAlive { get; private set; }
        public float CurrentHealth { get; private set; }
        public float MaxHealth => BaseUnitData.Health;
        public Transform BarPoint => _barPoint;
        public float ReviveDuration => _reviveDuration;


        private void Awake()
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
            BaseUnitData = coreConfig.WarriorData;
        }

        protected virtual void CreateStateMachine()
        {
            _stateMachine = new UnitStateMachine(this);
            _stateMachine.Enter<IdleState>();
        }

        public void Clear()
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
            if (Target != null && Target.CurrentEnemyState == EnemyState.Running)
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
    }
}

