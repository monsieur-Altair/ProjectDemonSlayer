using _Application._Scripts.Core.Heroes;

namespace _Application._Scripts.Scriptables.Core.UnitsBehaviour
{
    public class HeroStateMachine : UnitStateMachine
    {
        public HeroStateMachine(BaseUnit baseUnit) : base(baseUnit)
        {
            _states.Add(typeof(MoveToPositionState), new MoveToPositionState(this));
            _states.Add(typeof(UltimateState), new UltimateState(this));
        }
    }
}