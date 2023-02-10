using System.Collections;
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

        public int CurrentLevelNumber { get;  set; }

        public override void Init()
        {
            base.Init();

            _globalPool = AllServices.Get<GlobalPool>();
            _progressService = AllServices.Get<ProgressService>();
            _gameFactory = AllServices.Get<GameFactory>();
        }

        public void SwitchToNextLevel()
        {
            StartCoroutine(DeleteLevel());
            CurrentLevelNumber++;
        }

        public void RestartLevel()
        {
            DeleteCurrentLevel();
        }

        public IEnumerator CreateLevel()
        {
            if (CurrentLevelNumber >= _levels.Length)
                CurrentLevelNumber = 0;

            yield return StartCoroutine(DeleteLevel());

            _currentLevel = _gameFactory.CreateLevel(_levels[CurrentLevelNumber]);
            _currentLevel.Prepare(_globalPool);
            _currentLevel.gameObject.SetActive(true);
            _currentLevel.transform.SetParent(transform.parent);
        }

        public void DeleteCurrentLevel()
        {
            StartCoroutine(DeleteLevel());
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