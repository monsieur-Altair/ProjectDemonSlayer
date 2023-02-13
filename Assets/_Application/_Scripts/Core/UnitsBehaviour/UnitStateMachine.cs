using System;
using System.Collections.Generic;

namespace _Application._Scripts.Scriptables.Core.UnitsBehaviour
{
    public class UnitStateMachine
    {
        protected readonly Dictionary<Type, BaseUnitState> _states;
        
        protected BaseUnitState _activeState;
        
        public BaseUnit BaseUnit { get; }

        public UnitStateMachine(BaseUnit baseUnit)
        {
            ResetState();
            
            BaseUnit = baseUnit;
            
            _states = new Dictionary<Type, BaseUnitState>
            {
                [typeof(IdleState)] = new IdleState(this),
                [typeof(MoveToTargetState)] = new MoveToTargetState(this),
                [typeof(AttackState)] = new AttackState(this),
                [typeof(DeathState)] = new DeathState(this)
            };
        }

        public void ResetState()
        {
            _activeState = null;
        }

        public void Update()
        {
            _activeState?.Update();
        }

        public void Enter<TState>() 
            where TState : BaseUnitState 
        {
            _activeState?.Exit();
            TState state = GetState<TState>();
            _activeState = state;
            state.Enter();
        }

        private TState GetState<TState>() 
            where TState : BaseUnitState
        {
            return _states[typeof(TState)] as TState;
        }
    }
}