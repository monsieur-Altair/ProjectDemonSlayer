using System;
using System.Collections.Generic;
using _Application._Scripts.Scriptables.Core.Towers;
using _Application.Scripts.Managers;
using DG.Tweening;
using Extensions;
using UnityEngine;

namespace _Application.Scripts.UI.Windows
{
    public class SellBuyUI : MonoBehaviour
    {
        public event Action<TowerType, int> TowerButtonClicked = delegate { };
        public event Action<int> DestroyButtonClicked = delegate { };
        public event Action<int> UpgradeButtonClicked = delegate { };
        
        
        [SerializeField] private RectTransform _pivot;
        [SerializeField] private List<BuyTowerButton> _towerButtons;
        [SerializeField] private GameObject _buyMenuGO;
        [SerializeField] private GameObject _sellUpgradeMenu;
        [SerializeField] private TowerButton _upgradeTowerButton;
        [SerializeField] private TowerButton _destroyTowerButton;
        private CoreConfig _coreConfig;


        public bool IsShown { get; private set; }
        public RectTransform Pivot => _pivot;

        public void ShowBuyMenu()
        {
            IsShown = true;
            _buyMenuGO.SetActive(true);
        }

        public void ShowSellUpgradeMenu(TowerType towerType, int towerLevel)
        {
            Debug.Log("show up-sell");

            IsShown = true;
            _sellUpgradeMenu.SetActive(true);

            bool isMax = _coreConfig.TowersData[towerType].Count - 1 <= towerLevel;
            _upgradeTowerButton.gameObject.SetActive(isMax==false);
            if (isMax == false)
            {
                int upgradeCost = _coreConfig.TowersData[towerType][towerLevel + 1].BuildCost;
                _upgradeTowerButton.OnOpened(upgradeCost);
            }

            _destroyTowerButton.OnOpened(_coreConfig.TowersData[towerType][towerLevel].DestroyReward);
        }

        public void Hide()
        {
            IsShown = false;
            TweenExt.Wait(0.01f).OnComplete(() =>
            {
                _sellUpgradeMenu.SetActive(false);
                _buyMenuGO.SetActive(false);
            });
            
        }
        
        public void OnOpened(CoreConfig coreConfig)
        {
            _coreConfig = coreConfig;
            
            foreach (BuyTowerButton towerButton in _towerButtons) 
                towerButton.Initialize(_coreConfig);
        }
        
        public void Subscribe()
        {
            foreach (BuyTowerButton towerButton in _towerButtons)
            {
                towerButton.Subscribe();
                towerButton.Clicked += OnTowerButtonClicked;
            }
            
            _upgradeTowerButton.Clicked += UpgradeTowerButtonOnClicked;
            _destroyTowerButton.Clicked += DestroyTowerButtonOnClicked;
            
            _upgradeTowerButton.Subscribe();
            _destroyTowerButton.Subscribe();
        }

        public void Unsubscribe()
        {
            foreach (BuyTowerButton towerButton in _towerButtons)
            {
                towerButton.Unsubscribe();
                towerButton.Clicked -= OnTowerButtonClicked;
            }
            
            _upgradeTowerButton.Clicked -= UpgradeTowerButtonOnClicked;
            _destroyTowerButton.Clicked -= DestroyTowerButtonOnClicked;
            
            _upgradeTowerButton.Unsubscribe();
            _destroyTowerButton.Unsubscribe();
        }

        private void DestroyTowerButtonOnClicked(int cost) => 
            DestroyButtonClicked(cost);

        private void UpgradeTowerButtonOnClicked(int cost) => 
            UpgradeButtonClicked(cost);

        private void OnTowerButtonClicked(TowerType towerType, int cost) => 
            TowerButtonClicked(towerType, cost);
    }
}