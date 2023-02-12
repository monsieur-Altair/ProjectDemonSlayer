using System;
using System.Collections.Generic;
using _Application._Scripts.Core.Enemies;
using _Application.Scripts.Managers;
using _Application.Scripts.Scriptables.Core.Enemies;
using UnityEngine;

namespace _Application._Scripts.Scriptables.Core.UnitsBehaviour
{
    public class BaseUnit : MonoBehaviour
    {
        public event Action<BaseUnit> Died = delegate { };
        public event Action<BaseUnit> Damaged = delegate { };
        
        [SerializeField] private float _closeAttackRadius = 1f;
        [SerializeField] private UnitStateMachine _stateMachine;
        [SerializeField] private float _reviveDuration;
        
        private BaseUnitData _baseUnitData;

        public BaseUnitData BaseUnitData { get; private set; }
        public BaseEnemy Target { get; private set; }
        public float CloseAttackRadius => _closeAttackRadius;
        public List<DamageInfo> DefenceInfo => _baseUnitData.DefenseInfo;

        public bool IsBusy { get; private set; }
        public bool IsAlive { get; private set; }
        public float CurrentHealth { get; private set; }
        public float ReviveDuration => _reviveDuration;


        public void Initialize(CoreConfig coreConfig)
        {
            _baseUnitData = coreConfig.WarriorData;
            _stateMachine.Initialize(this);
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
            target.Stop();
        }

        public void GoToTarget()
        {
            _stateMachine.Enter<MoveToTargetState>();
        }

        public void TakeDamage(float damageAmount)
        {
            CurrentHealth = Mathf.Max(0f, CurrentHealth - damageAmount);
            Damaged(this);

            if (CurrentHealth <= float.Epsilon)
                Die();
        }
        
        private void Die()
        {
            _stateMachine.Enter<DeathState>();
            Died(this);
        }
    }
}

