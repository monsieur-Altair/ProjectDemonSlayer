using System.Collections;
using System.Collections.Generic;
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
        private readonly CounterSpawner _counterSpawner;

        public GameLoopManager(LevelManager levelsManager, CoroutineRunner coroutineRunner,
            GlobalPool pool, OutlookService outlookService, UserControl userControl, ScriptableService scriptableService,
            ProgressService progressService, AudioManager audioManager, CoreConfig coreConfig, CounterSpawner counterSpawner)
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
            _counterSpawner = counterSpawner;
       }

        public void StartGame()
        {
            _currentGameState = GameStates.GameStarted;
            UpdateState();
        }

        public void WriteProgress(PlayerProgress playerProgress)
        {
            playerProgress.LevelInfo.lastCompletedLevel = _lastCompletedLevel;
        }

        public void ReadProgress(PlayerProgress playerProgress)
        {
            _lastCompletedLevel = playerProgress.LevelInfo.lastCompletedLevel;
            _levelsManager.CurrentLevelNumber = _lastCompletedLevel + 1;
        }

        public void Clear()
        {
            _levelsManager.DeleteCurrentLevel();
        }

        private void PrepareLevel()
        {
            
        }


        private void UpdateState()
        {
            switch (_currentGameState)
            { 
                case GameStates.GameStarted:
                {
                    _coroutineRunner.StartCoroutine(StartGameplay());
                    
                    UISystem.ShowWindow<GameplayWindow>();

                    int currentLevelNumber = _levelsManager.CurrentLevelNumber;

                    if (currentLevelNumber <= MaxTutorialCount && _useTutorial) 
                        UISystem.ShowTutorialWindow(currentLevelNumber);

                    break;
                }
                case GameStates.GameEnded:
                {
                    _userControl.Disable();
                    _counterSpawner.ClearLists();

                    int currentLevelNumber = _levelsManager.CurrentLevelNumber;
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
                _lastCompletedLevel = _levelsManager.CurrentLevelNumber;

            AddReward();
            
            _currentGameState = GameStates.GameEnded;
            UpdateState();
        }

        private void AddReward()
        {
            int money = AllServices.Get<ProgressService>().PlayerProgress.Money;
            int rewardMoney = _isWin ? _scriptableService.RewardList.GetReward(_lastCompletedLevel) : 0;
            
            money += rewardMoney;
            AllServices.Get<ProgressService>().PlayerProgress.Money = money;
        }

        private IEnumerator StartGameplay()
        {
            yield return _coroutineRunner.StartCoroutine(_levelsManager.CreateLevel());
            PrepareLevel();
            
            //_outlookService.PrepareLevel(_allBuildings);

            _counterSpawner.FillLists(UISystem.GetWindow<GameplayWindow>().CounterContainer);
            
            _userControl.Enable();
        }
    }
}