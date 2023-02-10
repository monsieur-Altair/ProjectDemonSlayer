using System;
using Pool_And_Particles;
using UnityEngine;

namespace _Application.Scripts.Units
{
    public abstract class BaseUnit : PooledBehaviour
    {
        public event Action<BaseUnit> Launched = delegate { };
        public event Action<BaseUnit> Updated = delegate { };
        public event Action<BaseUnit> Approached = delegate { };
        
        [SerializeField] protected Transform _counterPoint;
        [SerializeField] SkinnedMeshRenderer _skinnedMeshRenderer;
        
        public Transform CounterPoint => _counterPoint;
        public SkinnedMeshRenderer SkinnedMeshRenderer => _skinnedMeshRenderer;

        private Transform _transform;

        private void Awake()
        {
            _transform = transform;
        }

        // public void Update()
        // {
        //     if (Target != null && _canMove)
        //     {
        //         OnUpdate();
        //
        //         _transform.position = MoveTowards();
        //         
        //         float distance = Vector3.Distance(_destination, _transform.position);
        //         if (distance < MinDistance) 
        //             StopAndDestroy();
        //     }
        // }

        // private Vector3 MoveTowards()
        // {
        //     return Vector3.MoveTowards(_transform.position, Target.transform.position, 
        //         _speed * Time.deltaTime);
        // }

        protected virtual void OnUpdate()
        {
        }

        protected void OnUpdated()
        {
            Updated(this);
        }

        public override void OnReturnToPool()
        {
            base.OnReturnToPool();
        }

        public void StopAndDestroy()
        {
            Approached(this);
        }
    }
}