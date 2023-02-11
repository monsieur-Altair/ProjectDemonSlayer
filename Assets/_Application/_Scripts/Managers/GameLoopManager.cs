using System.Collections;
using _Application.Scripts.Control;
using _Application.Scripts.Infrastructure.Services;
using _Application.Scripts.Infrastructure.Services.Progress;
using _Application.Scripts.Infrastructure.Services.Scriptables;
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
        private readonly ScriptableService _scriptableService;
        private readonly ProgressService _progressService;
        private readonly AudioManager _audioManager;
        private readonly bool _useTutorial;
        private bool _isWin;
        private GameStates _currentGameState;

        private int _lastCompletedLevel;
        private bool _hasUpgradeTutorialShown;
        private readonly BarSpawner _barSpawner;

        public GameLoopManager(LevelManager levelsManager, CoroutineRunner coroutineRunner,
            GlobalPool pool, OutlookService outlookService, UserControl userControl, ScriptableService scriptableService,
            ProgressService progressService, AudioManager audioManager, CoreConfig coreConfig, BarSpawner barSpawner)
        {
            _useTutorial = AllServices.Get<CoreConfig>().UseTutorial;
            _coroutineRunner = coroutineRunner;
            _levelsManager = levelsManager;
            _objectPool = pool;
            _outlookService = outlookService;
            _userControl = userControl;
            _scriptableService = scriptableService;
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
                    
                    UISystem.ShowWindow<GameplayWindow>();

                    int currentLevelNumber = _levelsManager.CurrentLevelIndex;

                    if (currentLevelNumber <= MaxTutorialCount && _useTutorial) 
                        UISystem.ShowTutorialWindow(currentLevelNumber);

                    break;
                }
                case GameStates.GameEnded:
                {
                    _levelsManager.Clear();
                    _userControl.Disable();
                    _barSpawner.Clear();

                    int currentLevelNumber = _levelsManager.CurrentLevelIndex;
                    if (currentLevelNumber <= MaxTutorialCount && _useTutorial) 
                        UISystem.CloseTutorialWindow(currentLevelNumber);
                    
                    ShowEndGameWindow();

                    _audioManager.PlayEndgame(_isWin);
                    break;
                }
                default:
                    Debug.LogError("wrong");
                    break;
            }
        }

        private void ShowEndGameWindow()
        {
            UISystem.CloseWindow<GameplayWindow>();

            if (_isWin)
                UISystem.ShowWindow<WinWindow>();
            else
                UISystem.ShowWindow<LoseWindow>();
        }

        private void EndGame(bool isWin)
        {
            _coroutineRunner.CancelAllInvoked();
            _isWin = isWin;
            
            if (_isWin) 
                _lastCompletedLevel = _levelsManager.CurrentLevelIndex;

            AddReward();
            
            _currentGameState = GameStates.GameEnded;
            UpdateState();
        }

        private void AddReward()
        {
            //int money = AllServices.Get<ProgressService>().PlayerProgress.Money;
            //int rewardMoney = _isWin ? _scriptableService.RewardList.GetReward(_lastCompletedLevel) : 0;
            //
            //money += rewardMoney;
            //AllServices.Get<ProgressService>().PlayerProgress.Money = money;
        }

        private IEnumerator StartGameplay()
        {
            yield return _coroutineRunner.StartCoroutine(_levelsManager.CreateLevel());
            _levelsManager.StartLevel();
            _barSpawner.Initialize(_levelsManager.EnemyTracker);
            
            _userControl.Enable();
        }
    }
}