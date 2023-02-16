using System.Collections.Generic;
using _Application._Scripts.Core.Enemies;
using _Application.Scripts.Scriptables.Core.Enemies;

namespace _Application._Scripts.Core.Towers
{
    public class MagicProjectile : BaseProjectile
    {
        private float _slowDur;
        private float _slowCoefficient;

        public void Initialize(List<DamageInfo> attackInfo, float speed, BaseEnemy target, float powerCoefficient,
            float slowCoefficient, float slowDur)
        {
            _slowCoefficient = slowCoefficient;
            _slowDur = slowDur;
            Initialize(attackInfo, speed, target, powerCoefficient);
        }

        protected override void DamageTarget()
        {
            float newSpeed = Target.MotionSpeed * (1 - _slowCoefficient);
            Target.SlowDown(newSpeed, _slowDur);
            base.DamageTarget();
        }
    }
}