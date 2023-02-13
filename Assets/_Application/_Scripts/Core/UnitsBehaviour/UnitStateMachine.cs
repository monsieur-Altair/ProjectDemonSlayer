using System;
using System.Collections.Generic;
using _Application.Scripts.Infrastructure.States;

namespace _Application._Scripts.Scriptables.Core.UnitsBehaviour
{
    public class UnitStateMachine
    {
        private BaseUnitState _activeState;

        private Dictionary<Type, IBaseState> _states;
        public BaseUnit BaseUnit { get; }

        public UnitStateMachine(BaseUnit baseUnit)
        {
            ResetState();
            
            BaseUnit = baseUnit;
            
            _states = new Dictionary<Type, IBaseState>()
            {
                //todo: create states
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