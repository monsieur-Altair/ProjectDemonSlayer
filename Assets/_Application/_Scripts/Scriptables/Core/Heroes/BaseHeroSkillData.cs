using System.Collections.Generic;
using _Application.Scripts.Scriptables.Core.Enemies;
using UnityEngine;

namespace _Application._Scripts.Scriptables.Core.Heroes
{
    [CreateAssetMenu(fileName = "base hero skill data", menuName = "Resources/Base Hero Skill Data", order = 0)]
    public class BaseHeroSkillData :ScriptableObject
    {
        [SerializeField] private float _cooldown;
        [SerializeField] private List<DamageInfo> _attackInfo;
        [SerializeField] private float _radius;
    }
}