using _Application._Scripts.Core.Heroes;
using UnityEngine;

namespace _Application._Scripts.Scriptables.Core.UnitsBehaviour
{
    public class MoveToPositionState : BaseUnitState
    {
        private Vector3 _targetPos;
        private Vector3 _lookDir;
        private float _motionSpeed;

        public MoveToPositionState(UnitStateMachine unitStateMachine) : base(unitStateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();

            Holder.PlayRunAnimation();
            
            _targetPos = ((BaseHero) Holder).TargetPos;
            _motionSpeed = Holder.MotionsSpeed;

            _lookDir = (_targetPos - Holder.transform.position).normalized;
        }

        public override void Update()
        {
            base.Update();
            
            Transform transform = Holder.transform;
            Vector3 current = transform.position;
            Vector3 newPos = Vector3.MoveTowards(current, _targetPos, _motionSpeed * Time.deltaTime);
            transform.position = newPos;
            transform.rotation = Quaternion.LookRotation(_targetPos - newPos);

            if (Vector3.Distance(transform.position, _targetPos) < 0.001f)
            {
                transform.rotation = Quaternion.LookRotation(_lookDir);
                _stateMachine.Enter<IdleState>();
            }
        }
    }
}