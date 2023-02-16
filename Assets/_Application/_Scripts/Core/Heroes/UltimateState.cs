using _Application._Scripts.Scriptables.Core.UnitsBehaviour;
using DG.Tweening;
using Extensions;

namespace _Application._Scripts.Core.Heroes
{
    public class UltimateState : BaseUnitState
    {
        public UltimateState(UnitStateMachine unitStateMachine) : base(unitStateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();

            TweenExt.Wait(0.5f).OnComplete(ApplyUltimate);
        }

        private void ApplyUltimate()
        {
            (Holder as BaseHero)?.DamageByUltimate();
            _stateMachine.Enter<IdleState>();
        }
    }
}