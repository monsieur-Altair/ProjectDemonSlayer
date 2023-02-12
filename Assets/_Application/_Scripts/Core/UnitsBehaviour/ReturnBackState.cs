using _Application._Scripts.Core;
using _Application._Scripts.Core.Enemies;
using _Application.Scripts.Infrastructure.Services;
using _Application.Scripts.Managers;
using UnityEngine;

namespace _Application._Scripts.Scriptables.Core.UnitsBehaviour
{
    // public class ReturnBackState : BaseUnitState
    // {
    //     private Vector3 _targetPosition;
    //     private EnemyTracker _enemyTracker;
    //     private float _searchRadius;
    //     private float _motionSpeed;
    //
    //     public ReturnBackState(UnitStateMachine unitStateMachine) : base(unitStateMachine)
    //     {
    //     }
    //
    //     public override void Enter()
    //     {
    //         base.Enter();
    //
    //         Holder.SetBusy(false);
    //         _motionSpeed = _stateMachine.BaseUnit.BaseUnitData.MotionsSpeed;
    //         _targetPosition = Holder.IdlePosition;
    //         _searchRadius = Holder.CloseAttackRadius;
    //         _enemyTracker = AllServices.Get<LevelManager>().EnemyTracker;
    //         Holder.transform.rotation = Quaternion.LookRotation(_targetPosition - Holder.transform.position);
    //     }
    //
    //     public override void Exit()
    //     {
    //         base.Exit();
    //         
    //         Holder.SetBusy(false);
    //     }
    //
    //     public override void Update()
    //     {
    //         base.Update();
    //
    //         Transform transform = Holder.transform;
    //         Vector3 current = transform.position;
    //         transform.position = Vector3.MoveTowards(current, _targetPosition, _motionSpeed * Time.deltaTime);
    //
    //         if (Vector3.Distance(transform.position, _targetPosition) < 1f)
    //         {
    //             _stateMachine.Enter<IdleState>();
    //         }
    //         else
    //         {
    //             SearchForTarget();
    //         }
    //         
    //     }
    //
    //     private void SearchForTarget()
    //     {
    //         BaseEnemy target = CoreMethods.FindClosest(_enemyTracker.Enemies, _searchRadius, Holder.transform);
    //         if (target != null)
    //         {
    //             Holder.SetTarget(target);
    //             _stateMachine.Enter<MoveToTargetState>();
    //         }
    //     }
    // }
}