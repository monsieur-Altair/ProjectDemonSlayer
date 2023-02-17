using System.Collections.Generic;
using _Application._Scripts.Core.Enemies;
using _Application.Scripts.Infrastructure.Services;
using _Application.Scripts.Managers;
using _Application.Scripts.Scriptables.Core.Enemies;

namespace _Application._Scripts.Core.Heroes
{
    public class KnightHero : BaseHero
    {
        private KnightHeroData _knightHeroData;
        private EnemyTracker _enemyTracker;

        protected override HeroType HeroType => HeroType.Knight;
        public override float SkillCooldown => _knightHeroData.SkillCooldown;

        public override void Initialize(CoreConfig coreConfig)
        {
            base.Initialize(coreConfig);

            _enemyTracker = AllServices.Get<LevelManager>().CurrentLevel.WaveManager.EnemyTracker;
        }

        protected override void FetchData(CoreConfig coreConfig)
        {
            base.FetchData(coreConfig);
            
            BaseUnitData = coreConfig.KnightHeroData;
            _knightHeroData = coreConfig.KnightHeroData;
        }

        public override void DamageByUltimate()
        {
            base.DamageByUltimate();

            float skillAttackRadius = _knightHeroData.SkillAttackRadius;
            List<BaseEnemy> enemies = CoreMethods.FindInRange(_enemyTracker.Enemies, Transform, skillAttackRadius);
            
            foreach (BaseEnemy enemy in enemies)
            {
                float damageAmount = CoreMethods.CalculateDamage(_knightHeroData.SkillAttackInfo, enemy.DefenceInfo);
                enemy.TakeDamage(damageAmount);
            }
            
            //OnUltimateApplied();
        }
    }
}