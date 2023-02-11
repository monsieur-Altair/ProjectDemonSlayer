using System.Collections;
using _Application._Scripts.Core;
using _Application.Scripts.Infrastructure.Services;
using _Application.Scripts.Infrastructure.Services.Factory;
using _Application.Scripts.Infrastructure.Services.Progress;
using Pool_And_Particles;
using UnityEngine;

namespace _Application.Scripts.Managers
{
    public class LevelManager : MonoBehaviourService
    {
        [SerializeField] private Level[] _levels;

        private Level _currentLevel;
        private GlobalPool _globalPool;
        private ProgressService _progressService;
        private GameFactory _gameFactory;
        private CoreConfig _coreConfig;

        public int CurrentLevelIndex { get;  set; }
        public EnemyTracker EnemyTracker => _currentLevel.WaveManager.EnemyTracker;

        public override void Init()
        {
            base.Init();

            _coreConfig = AllServices.Get<CoreConfig>();
            _globalPool = AllServices.Get<GlobalPool>();
            _progressService = AllServices.Get<ProgressService>();
            _gameFactory = AllServices.Get<GameFactory>();
        }

        public void SwitchToNextLevel()
        {
            StartCoroutine(DeleteLevel());
            CurrentLevelIndex++;
        }

        public void RestartLevel()
        {
            DeleteCurrentLevel();
        }

        public void StartLevel()
        {
            _currentLevel.StartWaves();
        }

        public IEnumerator CreateLevel()
        {
            if (CurrentLevelIndex >= _levels.Length)
                CurrentLevelIndex = 0;

            yield return StartCoroutine(DeleteLevel());

            _currentLevel = _gameFactory.CreateLevel(_levels[CurrentLevelIndex]);
            _currentLevel.Initialize(_globalPool, _coreConfig, CurrentLevelIndex);
            _currentLevel.transform.SetParent(transform);
        }

        public void DeleteCurrentLevel()
        {
            StartCoroutine(DeleteLevel());
        }

        public void Clear()
        {
            _currentLevel.Clear();
        }

        private IEnumerator DeleteLevel()
        {
            if (_currentLevel != null)
            {
                _currentLevel.gameObject.SetActive(false);
                Destroy(_currentLevel.gameObject);
            }
            
            yield break;
        }
    }
}