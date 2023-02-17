using _Application.Scripts.Infrastructure.Services;
using _Application.Scripts.Infrastructure.Services.Factory;
using _Application.Scripts.Infrastructure.Services.Progress;

namespace _Application.Scripts.Managers
{
    public class LobbyManager : MonoBehaviourService
    {
        private GameFactory _gameFactory;
        private ProgressService _progressService;
        private OutlookService _outlookService;

        public override void Init()
        {
            base.Init();

            _progressService = AllServices.Get<ProgressService>();
            _gameFactory = AllServices.Get<GameFactory>();
            _outlookService = AllServices.Get<OutlookService>();
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