using _Application.Scripts.Managers;
using _Application.Scripts.Scriptables.Rewards;

namespace _Application.Scripts.Infrastructure.Services.Scriptables
{
    public class ScriptableService : IService
    {
        private readonly CoreConfig _coreConfig;

        
        
        public ScriptableService(CoreConfig coreConfig)
        {
            _coreConfig = coreConfig;
        }

        public void LoadAll()
        {
        }
        
    }
}