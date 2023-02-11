using _Application.Scripts.Infrastructure.Services;
using _Application.Scripts.Infrastructure.Services.Scriptables;
using _Application.Scripts.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace _Application.Scripts.UI.Windows
{
    [RequireComponent(typeof(GraphicRaycaster))]
    public class GameplayWindow : Window
    {
        [SerializeField] 
        private Transform _barParent;

        private CoroutineRunner _coroutineRunner;
        private LevelManager _levelManager;
        private ScriptableService _scriptableService;
        public Transform BarParent => _barParent;

        public override void GetDependencies()
        {
            base.GetDependencies();

            _coroutineRunner = AllServices.Get<CoroutineRunner>();
            _levelManager = AllServices.Get<LevelManager>();
            _scriptableService = AllServices.Get<ScriptableService>();
        }

        protected override void OnOpened()
        {
            base.OnOpened();

        }

        protected override void OnClosed()
        {
            base.OnClosed();
            
        }
    }
}