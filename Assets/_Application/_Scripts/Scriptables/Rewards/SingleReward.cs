using System;
using UnityEngine;

namespace _Application.Scripts.Scriptables.Rewards
{
    [Serializable]
    public struct SingleReward
    {
        [SerializeField] private CurrencyType _currencyType;
        [SerializeField] private float _value;

        public CurrencyType CurrencyType => _currencyType;
        public float Value => _value;
    }
    
    public enum CurrencyType
    {
        Gold, 
        Gem,
        Elixir,
    }
}