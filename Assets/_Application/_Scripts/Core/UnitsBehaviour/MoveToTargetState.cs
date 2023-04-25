using _Application._Scripts.Core.Enemies;
using _Application.Scripts.Managers;
using UnityEngine;

namespace _Application._Scripts.Scriptables.Core.UnitsBehaviour
{
    public class MoveToTargetState : BaseUnitState
    {
        private LevelManager _levelManager;
        private float _motionSpeed;
        private float _closeAttackRadius;
        private float _stopRange;

        private float _cachedDistance;

        public MoveToTargetState(UnitStateMachine unitStateMachine) : base(unitStateMachine)
        {
        }
        
        public override void Enter()
        {
            base.Enter();

            Holder.Target.Died += OnTargetDied;
            Holder.SetBusy(true);

            Holder.PlayRunAnimation();
            
            _motionSpeed = Holder.MotionsSpeed;
            _closeAttackRadius = Holder.CloseAttackRadius;
            _stopRange = 2f * _closeAttackRadius;

            _cachedDistance = float.MaxValue;
        }

        public override void Exit()
        {
            base.Exit();

            if (Holder.Target != null)
                Holder.Target.Died -= OnTargetDied;
            
            Holder.SetBusy(false);
            
            _cachedDistance = float.MaxValue;
        }

        private void OnTargetDied(IDamagable damagable)
        {
            Holder.Target.Died -= OnTargetDied;
            Holder.SetTarget(null);
            _stateMachine.Enter<IdleState>();
        }

        public override void Update()
        {
            Transform transform = Holder.transform;
            Vector3 current = transform.position;
            Vector3 target = Holder.Target.Transform.position;
            Vector3 newPos = Vector3.MoveTowards(current, target, _motionSpeed * Time.deltaTime);
            transform.position = newPos;
            transform.rotation = Quaternion.LookRotation(target - newPos);

            float distance = Vector3.Distance(newPos, target);

            if (distance > _cachedDistance + float.Epsilon)
            {
                _stateMachine.Enter<IdleState>();
                return;
            }

            _cachedDistance = distance;

            if (distance < _closeAttackRadius) 
                Holder.StartAttacking(Holder.Target);
            else if(distance < _stopRange) 
                Holder.StopTarget();
        }
    }
}