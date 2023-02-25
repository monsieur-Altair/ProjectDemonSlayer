using System.Collections;
using _Application.Scripts.Control;
using _Application.Scripts.Infrastructure.Services;
using _Application.Scripts.Infrastructure.Services.Progress;
using _Application.Scripts.SavedData;
using _Application.Scripts.UI;
using _Application.Scripts.UI.Windows;
using Pool_And_Particles;
using UnityEngine;

namespace _Application.Scripts.Managers
{
    public enum GameStates
    {
        GameStarted,
        GameEnded, 
    }

    public class GameLoopManager : IProgressWriter, IProgressReader
    {
        private const int MaxTutorialCount = 5;
        
        private readonly OutlookService _outlookService;
        private readonly LevelManager _levelsManager;
        private readonly GlobalPool _objectPool;
        private readonly UserControl _userControl;
        private readonly CoroutineRunner _coroutineRunner;
        private readonly ProgressService _progressService;
        private readonly AudioManager _audioManager;
        private readonly bool _useTutorial;
        private GameStates _currentGameState;

        private int _lastCompletedLevel;
        private bool _hasUpgradeTutorialShown;
        private readonly BarSpawner _barSpawner;

        public GameLoopManager(LevelManager levelsManager, CoroutineRunner coroutineRunner,
            GlobalPool pool, OutlookService outlookService, UserControl userControl,
            ProgressService progressService, AudioManager audioManager, CoreConfig coreConfig, BarSpawner barSpawner)
        {
            _useTutorial = AllServices.Get<CoreConfig>().UseTutorial;
            _coroutineRunner = coroutineRunner;
            _levelsManager = levelsManager;
            _objectPool = pool;
            _outlookService = outlookService;
            _userControl = userControl;
            _progressService = progressService;
            _audioManager = audioManager;
            _barSpawner = barSpawner;
       }

        public void StartGame()
        {
            _currentGameState = GameStates.GameStarted;
            UpdateState();
        }

        public void WriteProgress(PlayerProgress playerProgress)
        {
            playerProgress.LevelInfo.SetLevel(_lastCompletedLevel);
        }

        public void ReadProgress(PlayerProgress playerProgress)
        {
            _lastCompletedLevel = playerProgress.LevelInfo.LastCompletedLevel;
            _levelsManager.CurrentLevelIndex = _lastCompletedLevel + 1;
        }

        public void Clear()
        {
            _levelsManager.DeleteCurrentLevel();
        }

        private void UpdateState()
        {
            switch (_currentGameState)
            { 
                case GameStates.GameStarted:
                {
                    _coroutineRunner.StartCoroutine(StartGameplay());
                    
                    int currentLevelNumber = _levelsManager.CurrentLevelIndex;
                    if (currentLevelNumber <= MaxTutorialCount && _useTutorial) 
                        UISystem.ShowTutorialWindow(currentLevelNumber);

                    break;
                }
                case GameStates.GameEnded:
                {
                    _levelsManager.CurrentLevel.WaveManager.LevelPassed -= OnLevelPassed;
                    _levelsManager.CurrentLevel.WaveManager.LevelFailed -= OnLevelFailed;
                    
                    _levelsManager.CurrentLevel.Stop();
                    _userControl.Disable();
                    _barSpawner.Clear();

                    int currentLevelNumber = _levelsManager.CurrentLevelIndex;
                    if (currentLevelNumber <= MaxTutorialCount && _useTutorial) 
                        UISystem.CloseTutorialWindow(currentLevelNumber);
                    
                    break;
                }
                default:
                    Debug.LogError("wrong");
                    break;
            }
        }

        private void EndGame(bool isWin)
        {
            _coroutineRunner.CancelAllInvoked();
            _audioManager.PlayEndgame(isWin);

            if (isWin) 
                _lastCompletedLevel = _levelsManager.CurrentLevelIndex;

            AddReward();
            
            UISystem.CloseWindow<GameplayWindow>();

            if (isWin)
                UISystem.ShowWindow<WinWindow>();
            else
                UISystem.ShowWindow<LoseWindow>();

            _currentGameState = GameStates.GameEnded;
            UpdateState();
        }

        private void AddReward()
        {
        }

        private IEnumerator StartGameplay()
        {
            yield return _coroutineRunner.StartCoroutine(_levelsManager.CreateLevel());
            _barSpawner.Initialize(_levelsManager);
            _userControl.Enable();

            UISystem.ShowWindow<GameplayWindow>();

            _levelsManager.CurrentLevel.WaveManager.LevelPassed += OnLevelPassed;
            _levelsManager.CurrentLevel.WaveManager.LevelFailed += OnLevelFailed;
        }

        private void OnLevelFailed()
        {
            EndGame(false);
        }

        private void OnLevelPassed()
        {
            EndGame(true);
        }
    }
}