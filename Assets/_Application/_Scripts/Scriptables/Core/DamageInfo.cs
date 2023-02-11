using System;
using UnityEngine;

namespace _Application.Scripts.Scriptables.Core.Enemies
{
    [Serializable]
    public class DamageInfo
    {
        [SerializeField] private float _value;
        [SerializeField] private DamageType _damageType;

        public float Value => _value;
        public DamageType DamageType => _damageType;
    }
}