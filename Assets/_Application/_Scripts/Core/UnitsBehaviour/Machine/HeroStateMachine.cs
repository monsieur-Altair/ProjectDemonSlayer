namespace _Application._Scripts.Scriptables.Core.UnitsBehaviour
{
    public class HeroStateMachine : UnitStateMachine
    {
        public HeroStateMachine(BaseUnit baseUnit) : base(baseUnit)
        {
            _states.Add(typeof(MoveToPositionState), new MoveToPositionState(this));
        }
    }
}