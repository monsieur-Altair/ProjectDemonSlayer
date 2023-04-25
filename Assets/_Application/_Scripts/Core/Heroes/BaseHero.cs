using System.Linq;
using _Application._Scripts.Scriptables.Core.TowerUpgrade;
using _Application._Scripts.Scriptables.Core.UnitsBehaviour;
using _Application.Scripts.Control;
using _Application.Scripts.Infrastructure.Services;
using _Application.Scripts.Infrastructure.Services.Progress;
using _Application.Scripts.Managers;
using _Application.Scripts.Misc;
using _Managers;
using _Scripts._CoreLogic;
using UnityEngine;

namespace _Application._Scripts.Core.Heroes
{
    [RequireComponent(typeof(Collider))]
    public abstract class BaseHero : BaseUnit, IMouseClickable
    {
        private static readonly int Ultimate = Animator.StringToHash("Ultimate");

        [SerializeField] private GameObject _selectionMark;
        
        private bool _isSelected;
        private InputZoned _inputZoned;
        private Camera _worldCamera;
        private Plane _raycastPlane;
        private ProgressService _progressService;
        private BaseHeroUpgradeData _baseHeroUpgradeData;

        public override float MotionsSpeed => BaseUnitData.MotionsSpeed * _baseHeroUpgradeData.SpeedCoefficient;
        public override float PowerCoefficient => _baseHeroUpgradeData.PowerCoefficient;
        public override float MaxHealth => BaseUnitData.Health * _baseHeroUpgradeData.HealthCoefficient;

        public virtual float SkillCooldown => 0.0f;
        public Vector3 TargetPos { get; private set; }

        protected abstract HeroType HeroType { get; }

        
        public override void Initialize(CoreConfig coreConfig)
        {
            _isSelected = false;
            _selectionMark.SetActive(_isSelected);
            
            _worldCamera = AllServices.Get<GlobalCamera>().WorldCamera;
            _inputZoned = AllServices.Get<UserControl>().InputZoned;
            _progressService = AllServices.Get<ProgressService>();
            
            _raycastPlane = new Plane(Vector3.up, Vector3.zero);

            _inputZoned.Upped += OnUpped;
            
            base.Initialize(coreConfig);
        }

        public override void Clear()
        {
            _inputZoned.Upped -= OnUpped;

            base.Clear();
        }

        private void OnUpped()
        {
            if (_inputZoned.IsUpped && _isSelected)
            {
                SwitchSelectionState();
                Vector3 targetPos = ConvertToTargetPos(_inputZoned.LastActivityPosition);
                if(Vector2.Distance(targetPos.ToXZ(), Transform.position.ToXZ()) > 0.5f) 
                    MoveTo(targetPos);
            }
        }

        protected override void FetchData(CoreConfig coreConfig)
        {
            ApplyUpgrades(coreConfig);
        }

        private void ApplyUpgrades(CoreConfig coreConfig)
        {
            int level = _progressService.PlayerProgress.HeroUpgrades
                .First(upgradeProgress => upgradeProgress.HeroType == HeroType).AchievedLevel;

            _baseHeroUpgradeData = coreConfig.HeroUpgrades[HeroType][level];
        }

        private Vector3 ConvertToTargetPos(Vector3 screenPos)
        {
            Ray ray = _worldCamera.ScreenPointToRay(screenPos);
            
            return _raycastPlane.Raycast(ray, out float distance) 
                ? ray.GetPoint(distance) 
                : Vector3.zero;
        }

        protected override void CreateStateMachine()
        {
            _stateMachine = new HeroStateMachine(this);
            _stateMachine.Enter<IdleState>();
        }

        private void MoveTo(Vector3 targetPos)
        {
            TargetPos = targetPos;
            _stateMachine.Enter<MoveToPositionState>();
        }

        public void DoUltimate()
        {
            _stateMachine.Enter<UltimateState>();    
        }

        public virtual void DamageByUltimate()
        {
            
        }
        
        private void SwitchSelectionState()
        {
            _isSelected = !_isSelected;
            _selectionMark.SetActive(_isSelected);
        }

        public void MouseUp()
        {
            SwitchSelectionState();
        }

        public void MouseDown()
        {
        }

        public void MouseClick()
        {
        }

        public int Priority => 1;

        public void PlayUltimateAnimation()
        {
            Animator.ResetTrigger(Ultimate);
            Animator.SetTrigger(Ultimate);
        }
    }
}