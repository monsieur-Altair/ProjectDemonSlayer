﻿using _Application._Scripts.Core.Enemies;
using _Application.Scripts.Managers;
using UnityEngine;

namespace _Application._Scripts.Scriptables.Core.UnitsBehaviour
{
    public class MoveToTargetState : BaseUnitState
    {
        private LevelManager _levelManager;
        private float _motionSpeed;
        private float _closeAttackRadius;

        public MoveToTargetState(UnitStateMachine unitStateMachine) : base(unitStateMachine)
        {
        }
        
        public override void Enter()
        {
            base.Enter();

            Holder.Target.Died += OnTargetDied;
            Holder.SetBusy(true);

            _motionSpeed = Holder.BaseUnitData.MotionsSpeed;
            _closeAttackRadius = Holder.CloseAttackRadius;
        }

        public override void Exit()
        {
            base.Exit();

            Holder.Target.Died -= OnTargetDied;
            
            Holder.SetBusy(false);
        }

        private void OnTargetDied(BaseEnemy target)
        {
            _stateMachine.Enter<IdleState>();
        }

        public override void Update()
        {
            UpdatePosition();
        }

        private void UpdatePosition()
        {
            Transform transform = Holder.transform;
            Vector3 current = transform.position;
            Vector3 target = Holder.Target.Transform.position;
            Vector3 newPos = Vector3.MoveTowards(current, target, _motionSpeed * Time.deltaTime);
            transform.position = newPos;
            transform.rotation = Quaternion.LookRotation(target - newPos);

            if (Vector3.Distance(transform.position, target) < _closeAttackRadius) 
                _stateMachine.Enter<AttackState>();
        }
    }
}