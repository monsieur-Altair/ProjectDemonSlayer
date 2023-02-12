using _Application._Scripts.Core;
using _Application._Scripts.Core.Enemies;
using _Application.Scripts.Scriptables.Core.Enemies;
using UnityEngine;

namespace _Application._Scripts.Scriptables.Core.UnitsBehaviour
{
    public class AttackState : BaseUnitState
    {
        private BaseUnitData _holderData;
        private float _elapsedTime;

        public AttackState(UnitStateMachine unitStateMachine) : base(unitStateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();

            _holderData = Holder.BaseUnitData;
            _elapsedTime = _holderData.AttackCooldown + float.Epsilon;
            Holder.SetBusy(true);
            Holder.Target.Died += OnTargetDied;
            
            Holder.Target.StartAttacking(Holder);
        }

        public override void Exit()
        {
            base.Exit();
            Holder.SetBusy(false);
            Holder.Target.Died -= OnTargetDied;
        }

        private void OnTargetDied(BaseEnemy target)
        {
            _stateMachine.Enter<IdleState>();
        }

        public override void Update()
        {
            _elapsedTime += Time.deltaTime;

            if (_elapsedTime >= _holderData.AttackCooldown)
            {
                _elapsedTime = 0f;
                float damage = CoreMethods.CalculateDamage(_holderData.AttackInfo, Holder.Target.DefenceInfo);
                Holder.Target.TakeDamage(damage);
            }
        }
    }
}