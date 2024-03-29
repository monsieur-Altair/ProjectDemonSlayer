﻿using System;
using System.Collections.Generic;
using _Application.Scripts.Infrastructure.Services;
using _Application.Scripts.Infrastructure.Services.Factory;
using _Application.Scripts.Infrastructure.Services.Progress;
using _Application.Scripts.Managers;
using UnityEngine;

namespace _Application.Scripts.Infrastructure.States
{
    public class StateMachine : IService
    {
        private readonly Dictionary<Type, IBaseState> _states;
        private IBaseState _activeState;

        public StateMachine(MonoBehaviour monoBehaviour, SceneLoader sceneLoader, CoreConfig coreConfig)
        {
            _states = new Dictionary<Type, IBaseState>()
            {
                [typeof(BootstrapState)] = CreateBootstrapState(monoBehaviour, sceneLoader, coreConfig),
                [typeof(LoadLevelState)] = CreateLoadLevelState(sceneLoader),
                [typeof(LobbyState)] = CreateLobbyState(),
                [typeof(GameLoopState)] = CreateGameLoopState()
            };
        }

        private LobbyState CreateLobbyState() => new(this, AllServices.Get<LobbyManager>());
        
        private GameLoopState CreateGameLoopState() => new(this, AllServices.Get<GameFactory>());
        
        private LoadLevelState CreateLoadLevelState(SceneLoader sceneLoader) => 
            new(this, sceneLoader, AllServices.Get<GameFactory>(), AllServices.Get<ProgressService>());
        
        private BootstrapState CreateBootstrapState(MonoBehaviour monoBehaviour, SceneLoader sceneLoader, CoreConfig coreConfig) => 
            new(monoBehaviour, this, sceneLoader, coreConfig);

        public void Enter<TState>() where TState : class, IState
        {
            _activeState?.Exit();
            IState state = GetState<TState>();
            _activeState = state;
            state.Enter();
        }

        public void Enter<TState, TPayload>(TPayload payload) where TState : class, IStateWithPayload<TPayload>
        {
            _activeState?.Exit();
            IStateWithPayload<TPayload> state = GetState<TState>();
            _activeState = state;
            state.Enter(payload);
        }

        public TState GetState<TState>() where TState : class, IBaseState => 
            _states[typeof(TState)] as TState;
    }
}