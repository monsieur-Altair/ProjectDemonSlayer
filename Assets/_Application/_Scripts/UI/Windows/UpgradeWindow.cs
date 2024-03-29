﻿using System.Collections.Generic;
using _Application.Scripts.Infrastructure.Services;
using _Application.Scripts.Infrastructure.Services.Progress;
using _Application.Scripts.Infrastructure.Services.Scriptables;
using _Application.Scripts.Managers;
using _Application.Scripts.Upgrades;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Application.Scripts.UI.Windows
{
    [RequireComponent(typeof(GraphicRaycaster))]
    public class UpgradeWindow : Window
    {
        [SerializeField] private UpgradeController[] upgradeControllers;
        [SerializeField] private Button backButton;
        [SerializeField] private TextMeshProUGUI moneyText;
       
        private int _money;
        
        private ProgressService _progressService;
        private ScriptableService _scriptableService;
        private CoroutineRunner _coroutineRunner;
        
        private readonly List<IProgressReader> _progressReaders = new();
        private readonly List<IProgressWriter> _progressWriters = new();

        public override void GetDependencies()
        {
            _progressService = AllServices.Get<ProgressService>();
            _scriptableService = AllServices.Get<ScriptableService>();
            _coroutineRunner = AllServices.Get<CoroutineRunner>();
            
            //foreach (UpgradeController upgradeController in upgradeControllers) 
            //    InitController(upgradeController);
        }

        protected override void OnOpened()
        {
            base.OnOpened();

            foreach (IProgressReader progressReader in _progressReaders)
                progressReader.ReadProgress(_progressService.PlayerProgress);

            //foreach (UpgradeController upgradeController in upgradeControllers) 
            //    upgradeController.Refresh();

            _money = _progressService.PlayerProgress.Money;
            moneyText.text = _money.ToString();
            
            backButton.onClick.AddListener(GoBackToGame);
        }

        protected override void OnClosed()
        {
            base.OnClosed();
            
            foreach (IProgressWriter progressWriter in _progressWriters)
                progressWriter.WriteProgress(_progressService.PlayerProgress);

            _progressService.PlayerProgress.Money = _money;
            
            _coroutineRunner.StopAllCoroutines();
            backButton.onClick.RemoveListener(GoBackToGame);
        }

        private void GoBackToGame()
        {
            Close();
            
            UISystem.ShowWindow<LobbyWindow>();
        }

        private void OnDestroy()
        {  
            //foreach (UpgradeController upgradeController in upgradeControllers)
            //    upgradeController.TriedPurchaseUpgrade -= UpgradeController_TriedPurchaseUpgrade;
        }
        
        private void InitController(UpgradeController upgradeController)
        {
            //_progressReaders.Add(upgradeController);
            //_progressWriters.Add(upgradeController);
            //upgradeController.TriedPurchaseUpgrade += UpgradeController_TriedPurchaseUpgrade;
            //upgradeController.Init(_scriptableService, _coroutineRunner);
        }

        private void UpgradeController_TriedPurchaseUpgrade(UpgradeController upgradeController, int cost)
        {
            // if (_money >= cost)
            // {
            //     _money -= cost;
            //     _progressService.PlayerProgress.Money = _money; 
            //     moneyText.text = _money.ToString();
            //     upgradeController.ApplyPurchase();
            //     upgradeController.WriteProgress(_progressService.PlayerProgress);
            // }
        }
    }
}