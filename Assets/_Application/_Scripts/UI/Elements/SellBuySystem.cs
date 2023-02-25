using _Application._Scripts.Scriptables.Core.Towers;
using _Application.Scripts.Control;
using _Application.Scripts.Infrastructure.Services;
using _Application.Scripts.Managers;
using UnityEngine;

namespace _Application.Scripts.UI.Windows
{
    public class SellBuySystem
    {
        private readonly SellBuyUI _sellBuyUI;
        private readonly GlobalCamera _globalCamera;
        private readonly CoreConfig _coreConfig;
        private readonly UserControl _userControl;

        private Level _currentLevel;
        private BuildPlace _currentBuildPlace;

        public SellBuySystem(SellBuyUI sellBuyUI, GlobalCamera globalCamera, CoreConfig coreConfig,
            UserControl userControl)
        {
            _userControl = userControl;
            _coreConfig = coreConfig;
            _globalCamera = globalCamera;
            _sellBuyUI = sellBuyUI;
        }

        public void OnOpened(Level currentLevel)
        {
            _currentLevel = currentLevel;
            
            _sellBuyUI.OnOpened(_coreConfig);
        }
        
        public void Subscribe()
        {
            _sellBuyUI.Subscribe();
            _sellBuyUI.TowerButtonClicked += OnTowerButtonClicked;
            _sellBuyUI.UpgradeButtonClicked += OnUpgradeButtonClicked;
            _sellBuyUI.DestroyButtonClicked += OnDestroyButtonClicked;

            _userControl.InputZoned.Upped += OnUpped;
            
            foreach (BuildPlace buildPlace in _currentLevel.BuildPlaces) 
                buildPlace.Clicked += OnPlaceClicked;
        }

        private void OnUpped()
        {
            if (_sellBuyUI.IsShown && _userControl.InputZoned.IsUpped) 
                _sellBuyUI.Hide();
        }

        public void Unsubscribe()
        {
            _sellBuyUI.Unsubscribe();
            _sellBuyUI.TowerButtonClicked -= OnTowerButtonClicked;
            _sellBuyUI.UpgradeButtonClicked -= OnUpgradeButtonClicked;
            _sellBuyUI.DestroyButtonClicked -= OnDestroyButtonClicked;

            _userControl.InputZoned.Upped -= OnUpped;

            foreach (BuildPlace buildPlace in _currentLevel.BuildPlaces) 
                buildPlace.Clicked -= OnPlaceClicked;
        }

        public void OnClosed()
        {
            _currentBuildPlace = null;
            if(_sellBuyUI.IsShown)
                _sellBuyUI.Hide();
        }


        private void OnPlaceClicked(BuildPlace buildPlace)
        {
            if (_sellBuyUI.IsShown)
            {
                _sellBuyUI.Hide();
                _currentBuildPlace = null;
                return;
            }

            _currentBuildPlace = buildPlace;
            Vector3 pointPosition = buildPlace.ClickPoint.position;
            _sellBuyUI.Pivot.anchoredPosition = UISystem.GetUIPosition(_globalCamera.WorldCamera, pointPosition);

            if (buildPlace.CurrentTower == null)
                _sellBuyUI.ShowBuyMenu();
            else
            {
                _sellBuyUI.ShowSellUpgradeMenu(buildPlace.CurrentTower.TowerType,
                    buildPlace.CurrentTower.TowerLevel);
            }
        }

        private void OnDestroyButtonClicked(int cost)
        {
            _currentLevel.ElixirManager.IncreaseElixir(cost);
            _currentLevel.TowersManager.DestroyBuilding(_currentBuildPlace);
            
            _sellBuyUI.Hide();
            _currentBuildPlace = null;
        }

        private void OnUpgradeButtonClicked(int cost)
        {
            if (cost <= _currentLevel.ElixirManager.ElixirAmount)
            {
                _currentLevel.ElixirManager.DecreaseElixir(cost);
                _currentBuildPlace.CurrentTower.Upgrade();
                
                _sellBuyUI.Hide();
                _currentBuildPlace = null;
            }
        }

        private void OnTowerButtonClicked(TowerType towerType, int cost)
        {
            if (cost <= _currentLevel.ElixirManager.ElixirAmount)
            {
                _currentLevel.ElixirManager.DecreaseElixir(cost);
                _currentLevel.TowersManager.BuildBuilding(towerType, _currentBuildPlace);
                
                _sellBuyUI.Hide();
                _currentBuildPlace = null;
            }
        }
    }
}