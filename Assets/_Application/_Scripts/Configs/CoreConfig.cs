using _Application.Scripts.Infrastructure.Services;
using _Application.Scripts.Scriptables.Rewards;
using UnityEngine;

namespace _Application.Scripts.Managers
{
    [CreateAssetMenu (fileName = "CoreConfig",menuName = "Resources/Core Config")]
    public class CoreConfig: ScriptableObject, IService
    {
        [SerializeField] private MonoBehaviourServices _monoBehaviourServices;
        [Space, SerializeField] private bool _useTutorial;
        [SerializeField] private PlayerConfig _playerConfig;
        [SerializeField] private Warehouse _warehouse;
        [SerializeField] private RewardList _rewardList;


        public RewardList RewardList => _rewardList;
        public MonoBehaviourServices MonoBehaviourServices => _monoBehaviourServices;
        public bool UseTutorial => _useTutorial;
        public PlayerConfig PlayerConfig => _playerConfig;
        public Warehouse Warehouse => _warehouse;
    }
}