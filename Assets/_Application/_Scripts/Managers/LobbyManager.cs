using _Application.Scripts.Infrastructure.Services;
using _Application.Scripts.Infrastructure.Services.Factory;
using _Application.Scripts.Infrastructure.Services.Progress;
using _Application.Scripts.Infrastructure.Services.Scriptables;

namespace _Application.Scripts.Managers
{
    public class LobbyManager : MonoBehaviourService
    {
        private GameFactory _gameFactory;
        private ProgressService _progressService;
        private OutlookService _outlookService;
        private ScriptableService _scriptableService;

        public override void Init()
        {
            base.Init();

            _progressService = AllServices.Get<ProgressService>();
            _gameFactory = AllServices.Get<GameFactory>();
            _outlookService = AllServices.Get<OutlookService>();
            _scriptableService = AllServices.Get<ScriptableService>();
        }
        
        public void OnEnter()
        {
            gameObject.SetActive(true);
        }

        public void OnExit()
        {
        }
    }
}