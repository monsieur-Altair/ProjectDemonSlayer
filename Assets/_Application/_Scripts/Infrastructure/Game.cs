﻿using _Application.Scripts.Infrastructure.States;
using _Application.Scripts.Managers;
using UnityEngine;

namespace _Application.Scripts.Infrastructure
{
    public class Game
    {
        public readonly StateMachine StateMachine;
        public Game(MonoBehaviour behaviour, CoreConfig coreConfig)
        {
            StateMachine = new StateMachine(behaviour, new SceneLoader(behaviour), coreConfig);
        }
    }
}