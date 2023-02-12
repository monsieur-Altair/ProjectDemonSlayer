namespace _Application._Scripts.Scriptables.Core.UnitsBehaviour
{
    public abstract class BaseUnitState
    {
        protected UnitStateMachine _stateMachine;
        protected BaseUnit Holder => _stateMachine.BaseUnit; 

        protected BaseUnitState(UnitStateMachine unitStateMachine)
        {
            _stateMachine = unitStateMachine;
        }

        public virtual void Enter()
        {
        }

        public virtual void Update()
        {
            
        }
        
        public virtual void Exit()
        {
        }
    }
}