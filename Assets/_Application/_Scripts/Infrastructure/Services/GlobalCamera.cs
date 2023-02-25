using _Application.Scripts.Managers;
using _Application.Scripts.Misc;
using _Scripts._CoreLogic;
using UnityEngine;

namespace _Application.Scripts.Infrastructure.Services
{
    public class GlobalCamera : MonoBehaviourService
    {
        [SerializeField] private CameraResolution _cameraResolution;
        [SerializeField] private Camera _camera;
        [SerializeField] private MouseClickableRaycaster _mouseClickableRaycaster;


        public MouseClickableRaycaster MouseClickableRaycaster => _mouseClickableRaycaster;
        public Camera WorldCamera => _camera;

        public override void Init()
        {
            base.Init();

            _cameraResolution.Init(_camera);
        }
    }
}