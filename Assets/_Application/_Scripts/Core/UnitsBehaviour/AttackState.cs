using _Application._Scripts.Core;
using _Application._Scripts.Core.Enemies;
using _Application.Scripts.Scriptables.Core.Enemies;
using UnityEngine;

namespace _Application._Scripts.Scriptables.Core.UnitsBehaviour
{
    public class AttackState : BaseUnitState
    {
        private float _elapsedTime;

        private bool IsOutOfDistance => 
            Vector3.Distance(Holder.Transform.position, Holder.Target.Transform.position) > Holder.CloseAttackRadius;

        public AttackState(UnitStateMachine unitStateMachine) : base(unitStateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();

            _elapsedTime = Holder.AttackCooldown + float.Epsilon;
            Holder.SetBusy(true);
            Holder.Target.Died += RemoveTarget;
            
            Holder.Target.StartAttacking(Holder);
        }

        public override void Exit()
        {
            base.Exit();
            Holder.SetBusy(false);
            if (Holder.Target != null)
                Holder.Target.Died -= RemoveTarget;
        }

        private void RemoveTarget(IDamagable damagable)
        {
            Holder.Target.Died -= RemoveTarget;
            Holder.SetTarget(null);
            _stateMachine.Enter<IdleState>();
        }

        public override void Update()
        {
            if(Holder.Target == null)
                return;
            
            if (IsOutOfDistance)
            {
                RemoveTarget(Holder.Target);
                return;
            }
            
            _elapsedTime += Time.deltaTime;

            Vector3 lookDir = (Holder.Target.transform.position - Holder.transform.position).normalized;
            Holder.transform.rotation = Quaternion.LookRotation(lookDir);

            if (_elapsedTime >= Holder.AttackCooldown)
            {
                _elapsedTime = 0f;
                float damage = CoreMethods.CalculateDamage(Holder.AttackInfo, Holder.Target.DefenceInfo);
                Holder.Target.TakeDamage(damage * Holder.PowerCoefficient);

                Holder.PlayAttackAnimation();
            }
        }
    }
}