using _Application.Scripts.Managers;
using _Managers;
using UnityEngine;

namespace _Application.Scripts.Control
{
    public class UserControl : MonoBehaviourService
    {
        private bool _isActive;
        public IInputSystem InputService { get; private set; }

        public override void Init()
        {
            base.Init();
            
            InputService = RegisterInputService();
        }

        public void Update()
        {
            if(!_isActive)
                return;
            
        }

        public void Disable() => 
            _isActive = false;

        public void Enable() => 
            _isActive = true;

        private static IInputSystem RegisterInputService() => 
            Application.isEditor ? new StandaloneInput() : new MobileInput();
    }
}


