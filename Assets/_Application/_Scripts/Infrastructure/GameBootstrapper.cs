﻿using _Application.Scripts.Infrastructure.States;
using _Application.Scripts.Managers;
using UnityEngine;

namespace _Application.Scripts.Infrastructure
{
    public class GameBootstrapper : MonoBehaviour
    {
        [SerializeField] private CoreConfig _coreConfig;
        
        private Game _game;
        
        private void Awake()
        {
            _game = new Game(this, _coreConfig);
            _game.StateMachine.Enter<BootstrapState>();
            DontDestroyOnLoad(this);
        }
    }
}