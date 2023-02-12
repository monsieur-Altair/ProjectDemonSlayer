using System;
using System.Collections.Generic;
using _Application.Scripts.Infrastructure.States;
using UnityEngine;

namespace _Application._Scripts.Scriptables.Core.UnitsBehaviour
{
    public class UnitStateMachine : MonoBehaviour
    {
        private BaseUnitState _activeState;

        private Dictionary<Type, IBaseState> _states;

        public BaseUnit BaseUnit { get; private set; }

        public void Initialize(BaseUnit baseUnit)
        {
            BaseUnit = baseUnit;
            _states = new Dictionary<Type, IBaseState>()
            {
                //todo: create states
            };
        }

        private void Update()
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