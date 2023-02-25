using _Application._Scripts.Core;
using _Application.Scripts.Control;
using _Application.Scripts.Infrastructure.Services;
using _Application.Scripts.Managers;
using _Managers;
using _UI._Screens;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Application.Scripts.UI.Windows
{
   // [RequireComponent(typeof(GraphicRaycaster))]
    public class GameplayWindow : Window
    {
        [SerializeField] private Transform _barParent;

        [SerializeField] private HeroButton _heroButton;
        [SerializeField] private TextMeshProUGUI _penetrationCountTMP;
        [SerializeField] private TextMeshProUGUI _waveCountTMP;
        [SerializeField] private TextMeshProUGUI _elixirTMP;
        [SerializeField] private SellBuyUI _sellBuyUI;
        [SerializeField] private Button _startWaveButton;
        [SerializeField] private Button _pauseButton;
        [SerializeField] private Button _accelerationButton;
        [SerializeField] private ClickableZone _clickableZone;

        private CoroutineRunner _coroutineRunner;
        private LevelManager _levelManager;
        private Level _currentLevel;
        private SellBuySystem _sellBuySystem;
        private UserControl _userControl;
        public Transform BarParent => _barParent;

        public override void GetDependencies()
        {
            base.GetDependencies();

            _coroutineRunner = AllServices.Get<CoroutineRunner>();
            _levelManager = AllServices.Get<LevelManager>();

            GlobalCamera globalCamera = AllServices.Get<GlobalCamera>();
            CoreConfig coreConfig = AllServices.Get<CoreConfig>();
            _userControl = AllServices.Get<UserControl>();

            _sellBuySystem = new SellBuySystem(_sellBuyUI, globalCamera, coreConfig, _userControl);
        }

        private void Update()
        {
        }

        protected override void OnOpened()
        {
            base.OnOpened();

            _userControl.InputZoned.SetZone(_clickableZone, ClickableZoneType.GameplayScreen);
            
            _currentLevel = _levelManager.CurrentLevel;

            UpdateWaveTMP();
            UpdatePenetrationTMP();
            UpdateElixirTMP();
            
            _startWaveButton.gameObject.SetActive(true);
            _heroButton.OnOpened(_currentLevel.BaseHero);
            _sellBuySystem.OnOpened(_currentLevel);
        }

        protected override void SubscribeEvents()
        {
            base.SubscribeEvents();
            
            _startWaveButton.onClick.AddListener(StartWave);
            _pauseButton.onClick.AddListener(OnPauseClicked);
            _accelerationButton.onClick.AddListener(OnAccelerationClicked);
            
            _heroButton.Subscribe();
            _sellBuySystem.Subscribe();
            
            _currentLevel.WaveManager.WaveUpdated += UpdateWaveTMP;
            _currentLevel.WaveManager.PenetrationUpdated += UpdatePenetrationTMP;
            _currentLevel.ElixirManager.ElixirAmountUpdated += UpdateElixirTMP;
        }
        
        protected override void UnSubscribeEvents()
        {
            base.UnSubscribeEvents();
            
            _pauseButton.onClick.RemoveListener(OnPauseClicked);
            _accelerationButton.onClick.RemoveListener(OnAccelerationClicked);
            _startWaveButton.onClick.RemoveListener(StartWave);
            
            _heroButton.Unsubscribe();
            _sellBuySystem.Unsubscribe();

            _currentLevel.WaveManager.WaveUpdated -= UpdateWaveTMP;
            _currentLevel.WaveManager.PenetrationUpdated -= UpdatePenetrationTMP;
            _currentLevel.ElixirManager.ElixirAmountUpdated -= UpdateElixirTMP;
        }

        protected override void OnClosed()
        {
            base.OnClosed();
            
            _sellBuySystem.OnClosed();
        }

        private void UpdatePenetrationTMP()
        {
            WaveManager waveManager = _currentLevel.WaveManager;
            int leftPenetrationCount = waveManager.MaxApproachingCount - waveManager.ApproachedEnemyCount;
            _penetrationCountTMP.text = leftPenetrationCount.ToString();
        }

        private void UpdateWaveTMP()
        {
            int curr = _currentLevel.WaveManager.CurrentWaveIndex;
            int max = _currentLevel.WaveManager.MaxWaveIndex;
            _waveCountTMP.text = $"{curr}/{max}";
        }

        private void UpdateElixirTMP()
        {
            _elixirTMP.text = $"{_currentLevel.ElixirManager.ElixirAmount}";
        }

        private void StartWave()
        {
            _currentLevel.StartWaves();
            _startWaveButton.gameObject.SetActive(false);
        }

        private void OnPauseClicked()
        {
            Time.timeScale = 1 - Time.timeScale;
        }

        private void OnAccelerationClicked()
        {
            Time.timeScale = 3 - Time.timeScale;
        }
    }
}