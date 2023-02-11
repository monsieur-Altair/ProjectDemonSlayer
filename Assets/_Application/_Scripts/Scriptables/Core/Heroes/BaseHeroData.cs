using _Application.Scripts.Scriptables.Core.Enemies;
using UnityEngine;

namespace _Application._Scripts.Scriptables.Core.Heroes
{
    [CreateAssetMenu(fileName = "base hero data", menuName = "Resources/Base Hero Data", order = 0)]
    public class BaseHeroData : BaseUnitData
    {
        [SerializeField] private BaseHeroSkillData _skillData;
        [SerializeField] private HeroType _heroType;

        public BaseHeroSkillData SkillData => _skillData;
        public HeroType HeroType => _heroType;
    }
}