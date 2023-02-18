using System.Collections.Generic;
using _Application._Scripts.Scriptables.Core.Towers;
using _Application.Scripts.Infrastructure.Services;
using _Application.Scripts.Infrastructure.Services.Progress;
using _Application.Scripts.Managers;
using UnityEngine;

namespace _Application.Scripts.UI.Windows
{
    public class TowerUpgradeWindow : Window
    {
        [SerializeField] private List<TowerUpgradeLabel> _towerUpgradeLabels;
        [SerializeField] private TowerUpgradePopup _towerUpgradePopup; 

        public override void GetDependencies()
        {
            base.GetDependencies();

            CoreConfig coreConfig = AllServices.Get<CoreConfig>();
            ProgressService progressService = AllServices.Get<ProgressService>();

            foreach (TowerUpgradeLabel towerUpgradeLabel in _towerUpgradeLabels)
                towerUpgradeLabel.Initialize(coreConfig, progressService);
            
            _towerUpgradePopup.Initialize(coreConfig, progressService);
        }

        protected override void OnOpened()
        {
            base.OnOpened();

            foreach (TowerUpgradeLabel towerUpgradeLabel in _towerUpgradeLabels)
            {
                towerUpgradeLabel.OnOpened();
                towerUpgradeLabel.Clicked += OnIconClicked;
            }
        }

        protected override void OnClosed()
        {
            base.OnClosed();

            foreach (TowerUpgradeLabel towerUpgradeLabel in _towerUpgradeLabels)
            {
                towerUpgradeLabel.OnClosed();
                towerUpgradeLabel.Clicked -= OnIconClicked;
            }
        }

        private void OnIconClicked(TowerType towerType, int towerLevel)
        {
            _towerUpgradePopup.gameObject.SetActive(true);
            _towerUpgradePopup.OnOpened(towerType, towerLevel);
        }
    }
}