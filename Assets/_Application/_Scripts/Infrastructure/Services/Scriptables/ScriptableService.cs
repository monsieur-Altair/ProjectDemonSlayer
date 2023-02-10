using _Application.Scripts.Managers;
using _Application.Scripts.Scriptables.Rewards;

namespace _Application.Scripts.Infrastructure.Services.Scriptables
{
    public class ScriptableService : IService
    {
        private readonly CoreConfig _coreConfig;

        public RewardList RewardList { get; private set; }
        
        
        public ScriptableService(CoreConfig coreConfig)
        {
            _coreConfig = coreConfig;
        }

        public void LoadAll()
        {
            RewardList = _coreConfig.RewardList;
        }
        
    }
}