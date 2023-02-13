using _Application._Scripts.Core;
using _Application._Scripts.Core.Enemies;
using _Application.Scripts.Infrastructure.Services;
using _Application.Scripts.Managers;
using UnityEngine;

namespace _Application._Scripts.Scriptables.Core.UnitsBehaviour
{
    public class IdleState : BaseUnitState
    {
        private LevelManager _levelManager;
        private float _closeAttackRadius;
        private EnemyTracker _enemyTracker;

        public IdleState(UnitStateMachine unitStateMachine) : base(unitStateMachine)
        {
        }

        public override void Enter()
        {
            Debug.Log($"enter idle");

            base.Enter();

            Holder.SetIsAlive(true);
            Holder.SetBusy(false);
            _closeAttackRadius = Holder.CloseAttackRadius;
            _enemyTracker = AllServices.Get<LevelManager>().CurrentLevel.WaveManager.EnemyTracker;
        }

        public override void Exit()
        {
            Debug.Log($"exit idle");

            base.Exit();
            
            Holder.SetBusy(false);
        }

        public override void Update()
        {
            SearchForTarget();
        }

        private void SearchForTarget()
        {
            BaseEnemy target = CoreMethods.FindClosest(_enemyTracker.Enemies, _closeAttackRadius, Holder.transform);

            if (target != null)
            {
                Holder.SetTarget(target);
                Holder.StartAttacking(target);
            }
        }
    }
}