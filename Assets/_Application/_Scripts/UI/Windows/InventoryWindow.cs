using System.Collections.Generic;
using _Application._Scripts.Core.Heroes;
using _Application._Scripts.Scriptables.Core.Towers;
using _Application.Scripts.Infrastructure.Services;
using _Application.Scripts.Infrastructure.Services.Progress;
using _Application.Scripts.Managers;
using _Application.Scripts.SavedData;
using Pool_And_Particles;
using UnityEngine;
using UnityEngine.UI;

namespace _Application.Scripts.UI.Windows
{
    public class InventoryWindow : Window
    {
        [SerializeField] private Button _closeButton;
        [SerializeField] private RectTransform _minPoint;
        [SerializeField] private RectTransform _maxPoint;
        [SerializeField] private Vector2 _minSpaces;
        [SerializeField] private CardUI _cardPrefab;

        private MatrixZone _matrixZone;
        private readonly List<CardUI> _allCards = new();
        private ProgressService _progressService;
        private CoreConfig _coreConfig;
        private GlobalPool _globalPool;
        [SerializeField] private RectTransform _centrePoint;

        public override void GetDependencies()
        {
            base.GetDependencies();

            _progressService = AllServices.Get<ProgressService>();
            _coreConfig = AllServices.Get<CoreConfig>();
            _globalPool = AllServices.Get<GlobalPool>();
        }

        protected override void OnOpened()
        {
            base.OnOpened();

            _matrixZone = new MatrixZone(_minPoint, _maxPoint, _cardPrefab.RectTransform, _minSpaces, _centrePoint);
            
            AddTowerCard();
            AddHeroCard();
            _closeButton.onClick.AddListener(CloseWindow);
        }

        protected override void OnClosed()
        {
            base.OnClosed();

            _closeButton.onClick.RemoveListener(CloseWindow);
            
            foreach (CardUI card in _allCards) 
                _globalPool.Free(card);
        }

        private void CloseWindow()
        {
            Close();
            UISystem.ShowWindow<LobbyWindow>();
        }

        private void AddHeroCard()
        {
            HeroUpgrade[] heroUpgrades = _progressService.PlayerProgress.HeroUpgrades;
            MyDictionary<HeroType, Sprite> allSprites = _coreConfig.Warehouse.HeroCardSprites;
            foreach (HeroUpgrade heroUpgrade in heroUpgrades)
            {
                CardUI card = _globalPool.Get(_cardPrefab, parent: _centrePoint);
                card.Init(allSprites[heroUpgrade.HeroType], heroUpgrade.SavedCard);
                _allCards.Add(card);
                card.RectTransform.anchoredPosition = _matrixZone.GetPos(_allCards.Count - 1);
            }
        }

        private void AddTowerCard()
        {
            TowersUpgrade[] towersUpgrades = _progressService.PlayerProgress.TowersUpgrades;
            MyDictionary<TowerType, List<Sprite>> allSprites = _coreConfig.Warehouse.TowerCardSprites;
            foreach (TowersUpgrade towerUpgrade in towersUpgrades)
            {
                List<Sprite> sprites = allSprites[towerUpgrade.TowerType];
                for (int i = 0; i < towerUpgrade.AchievedLevels.Length; i++)
                {
                    CardUI card = _globalPool.Get(_cardPrefab, parent: _centrePoint);
                    card.Init(sprites[i], towerUpgrade.SavedCard[i]);
                    _allCards.Add(card);
                    card.RectTransform.anchoredPosition = _matrixZone.GetPos(_allCards.Count - 1);
                }
            }
        }
    }
}