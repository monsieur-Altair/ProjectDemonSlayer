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

        private GlobalPool _globalPool;
        private GameFactory _gameFactory;
        private CoreConfig _coreConfig;

        public Level CurrentLevel { get; private set; }
        public int CurrentLevelIndex { get; set; }


        public override void Init()
        {
            base.Init();

            _coreConfig = AllServices.Get<CoreConfig>();
            _globalPool = AllServices.Get<GlobalPool>();
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

        public IEnumerator CreateLevel()
        {
            if (CurrentLevelIndex >= _levels.Length)
                CurrentLevelIndex = 0;

            yield return StartCoroutine(DeleteLevel());

            CurrentLevel = _gameFactory.CreateLevel(_levels[CurrentLevelIndex]);
            CurrentLevel.Initialize(_globalPool, _coreConfig, CurrentLevelIndex);
            CurrentLevel.transform.SetParent(transform);
        }

        public void DeleteCurrentLevel()
        {
            StartCoroutine(DeleteLevel());
        }

        private IEnumerator DeleteLevel()
        {
            if (CurrentLevel != null)
            {
                CurrentLevel.gameObject.SetActive(false);
                Destroy(CurrentLevel.gameObject);
            }

            yield break;
        }
    }
}