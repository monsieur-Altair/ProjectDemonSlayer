using _Application.Scripts.Infrastructure.Services;
using _Application.Scripts.Infrastructure.Services.Factory;
using _Application.Scripts.Infrastructure.Services.Progress;
using _Application.Scripts.Managers;
using _Application.Scripts.SavedData;
using Pool_And_Particles;
using UnityEngine;

namespace _Application.Scripts.Infrastructure.States
{
    public class BootstrapState : IState
    {
        private const string StartUp = "Bootstrap";
        private const string Main = "Main";
        private readonly StateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly ProgressService _progressService;
        private readonly ReadWriterService _readWriterService;
        private readonly MonoBehaviour _monoBehaviour;
        private readonly CoreConfig _coreConfig;
        private GameFactory _gameFactory;
        private AudioManager _audioManager;

        public BootstrapState(MonoBehaviour monoBehaviour, StateMachine stateMachine, SceneLoader sceneLoader,
            CoreConfig coreConfig)
        {
            _monoBehaviour = monoBehaviour;
            _coreConfig = coreConfig;
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            
            RegisterServices();
            RegisterMonoBehServices();


            _progressService = AllServices.Get<ProgressService>();
            _readWriterService = AllServices.Get<ReadWriterService>();
            
            _gameFactory.CreateAndRegisterMonoBeh(_coreConfig.MonoBehaviourServices.UISystem);
            _gameFactory.CreateAndRegisterMonoBeh(_coreConfig.MonoBehaviourServices.UserControl);
        }

        private void RegisterMonoBehServices()
        {
            _gameFactory = AllServices.Get<GameFactory>();
            MonoBehaviourServices servicePrefabs = _coreConfig.MonoBehaviourServices;

            _gameFactory.CreateAndRegisterMonoBeh(servicePrefabs.AudioManager);
            _audioManager = AllServices.Get<AudioManager>();
            _gameFactory.CreateAndRegisterMonoBeh(servicePrefabs.CoroutineRunner);
            _gameFactory.CreateAndRegisterMonoBeh(servicePrefabs.LevelManager);
            _gameFactory.CreateAndRegisterMonoBeh(servicePrefabs.GlobalCamera);
            _gameFactory.CreateAndRegisterMonoBeh(servicePrefabs.LobbyManager);
        }

        public void Enter()
        {
            _sceneLoader.Load(StartUp, EnterLoadLevel);
            _audioManager.StartPlayBack();
        }

        public void Exit()
        {
            
        }
        
        private void EnterLoadLevel()
        {
            _stateMachine.Enter<LoadLevelState, string>(Main);
        }

        private void RegisterServices()
        {
            AllServices.Register(_coreConfig);
            RegisterPool();

            GameFactory factory = AllServices.Register(new GameFactory(_coreConfig));

            ProgressService progressService = AllServices.Register(new ProgressService());
            progressService.PlayerProgress = ReadWriterService.ReadProgress() ?? new PlayerProgress(-1);

            AllServices.Register(new OutlookService());

            AllServices.Register(new ReadWriterService(progressService, factory));
            AllServices.Register(_stateMachine);
        }

        private void RegisterPool()
        {
            GlobalPool globalPool = new(_coreConfig, _monoBehaviour.transform);

            AllServices.Register(globalPool);
        }
    }
}