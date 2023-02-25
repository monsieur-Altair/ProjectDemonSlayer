using System.Collections.Generic;
using _Application._Scripts.Core.Heroes;
using _Application._Scripts.Scriptables.Core.Levels;
using _Application._Scripts.Scriptables.Core.Towers;
using _Application.Scripts.Infrastructure.Services;
using _Application.Scripts.Infrastructure.Services.Progress;
using _Application.Scripts.Infrastructure.States;
using _Application.Scripts.Managers;
using _Application.Scripts.SavedData;
using Pool_And_Particles;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Application.Scripts.UI.Windows
{
    [RequireComponent(typeof(GraphicRaycaster))]
    public class WinWindow : PayloadedWindow<CardReward>
    {
        [SerializeField] private Button _nextLevelButton;
        [SerializeField] private Button _toLobby;
        [SerializeField] private TextMeshProUGUI _moneyText;
        
        [Space]
        [SerializeField] private RectTransform _minPoint;
        [SerializeField] private RectTransform _maxPoint;
        [SerializeField] private Vector2 _minSpaces;
        [SerializeField] private CardUI _cardPrefab;
        [SerializeField] private RectTransform _centrePoint;

        private ProgressService _progressService;
        private StateMachine _stateMachine;
        private AudioManager _audioManager;
        private LevelManager _levelManager;

        private CardReward CardReward => _payload;
        private MatrixZone _matrixZone;
        private CoreConfig _coreConfig;
        private GlobalPool _globalPool;

        private readonly List<CardUI> _allCards= new();

        public override void GetDependencies()
        {
            base.GetDependencies();

            _levelManager = AllServices.Get<LevelManager>();
            _audioManager = AllServices.Get<AudioManager>();
            _progressService = AllServices.Get<ProgressService>();
            _stateMachine = AllServices.Get<StateMachine>();
            _coreConfig = AllServices.Get<CoreConfig>();
            _globalPool = AllServices.Get<GlobalPool>();
        }

        protected override void OnOpened()
        {
            base.OnOpened();

            _matrixZone = new MatrixZone(_minPoint, _maxPoint, _cardPrefab.RectTransform, _minSpaces, _centrePoint);
            _moneyText.text = _progressService.PlayerProgress.Money.ToString();
            
            _nextLevelButton.onClick.AddListener(GoNextLevel);
            _toLobby.onClick.AddListener(GoToLobby);
            
            AddHeroCard();
            AddTowerCard();
        }

        protected override void OnClosed()
        {
            base.OnClosed();
            
            _nextLevelButton.onClick.RemoveListener(GoNextLevel);
            _toLobby.onClick.RemoveListener(GoToLobby);
            
                    
            foreach (CardUI card in _allCards) 
                _globalPool.Free(card);
        }

        private void GoToLobby()
        {
            Close();
            _stateMachine.Enter<LobbyState>();
        }

        private void GoNextLevel()
        {
            _audioManager.PlayBackgroundAgain();
            _levelManager.SwitchToNextLevel();
            _stateMachine.Enter<GameLoopState>();
            
            Close();
        }
        
        private void AddHeroCard()
        {
            List<HeroCardReward> heroCardRewards = CardReward.HeroCardRewards;
            MyDictionary<HeroType, Sprite> allSprites = _coreConfig.Warehouse.HeroCardSprites;
            foreach (HeroCardReward heroCardReward in heroCardRewards)
            {
                CardUI card = _globalPool.Get(_cardPrefab, parent: _centrePoint);
                card.Init(allSprites[heroCardReward.HeroType], heroCardReward.CardAmount);
                _allCards.Add(card);
                card.RectTransform.anchoredPosition = _matrixZone.GetPos(_allCards.Count - 1);
            }
        }

        private void AddTowerCard()
        {
            List<TowerCardReward> towerCardRewards = CardReward.TowerCardRewards;
            MyDictionary<TowerType, List<Sprite>> allSprites = _coreConfig.Warehouse.TowerCardSprites;
            foreach (TowerCardReward towerCardReward in towerCardRewards)
            {
                List<Sprite> sprites = allSprites[towerCardReward.TowerType];
                CardUI card = _globalPool.Get(_cardPrefab, parent: _centrePoint);
                card.Init(sprites[towerCardReward.Level], towerCardReward.CardAmount);
                _allCards.Add(card);
                card.RectTransform.anchoredPosition = _matrixZone.GetPos(_allCards.Count - 1);
            }
        }
    }
}