using UnityEngine;

namespace _Application._Scripts.Scriptables.Core.UnitsBehaviour
{
    public class DeathState : BaseUnitState
    {
        private float _reviveDuration;
        private float _elapsedTime;
        
        public DeathState(UnitStateMachine unitStateMachine) : base(unitStateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();

            Holder.SetIsAlive(false);
            _elapsedTime = 0.0f;
            _reviveDuration = Holder.ReviveDuration;
        }

        public override void Update()
        {
            base.Update();

            _elapsedTime += Time.deltaTime;
            if (_elapsedTime >= _reviveDuration) 
                _stateMachine.Enter<IdleState>();
        }

        public override void Exit()
        {
            base.Exit();
            Holder.OnAppeared();
            Holder.SetIsAlive(true);
        }
    }
}