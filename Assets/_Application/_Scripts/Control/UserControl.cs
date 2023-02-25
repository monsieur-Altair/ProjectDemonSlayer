using _Application.Scripts.Managers;
using _Managers;
using UnityEngine;

namespace _Application.Scripts.Control
{
    public class UserControl : MonoBehaviourService
    {
        private bool _isActive;
        public IInputSystem InputService { get; private set; }
        public InputZoned InputZoned { get; private set; }

        public override void Init()
        {
            base.Init();
            
            InputService = RegisterInputService();
            InputZoned = RegisterInputZoned();
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

        private InputZoned RegisterInputZoned()
        {
            return Application.isEditor 
                ? new StandaloneInputZoned(InputService) 
                : new MobileInputZoned(InputService);
        }

        private static IInputSystem RegisterInputService()
        {
            return Application.isEditor
                ? new StandaloneInput()
                : new MobileInput();
        }
    }
}


