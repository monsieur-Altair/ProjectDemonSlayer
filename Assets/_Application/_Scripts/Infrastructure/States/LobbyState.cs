using _Application.Scripts.Managers;
using _Application.Scripts.UI;
using _Application.Scripts.UI.Windows;

namespace _Application.Scripts.Infrastructure.States
{
    public class LobbyState : IState
    {
        private readonly StateMachine _stateMachine;
        private readonly LobbyManager _lobbyManager;


        public LobbyState(StateMachine stateMachine, LobbyManager lobbyManager)
        {
            _lobbyManager = lobbyManager;
            _stateMachine = stateMachine;
        }

        public void Enter()
        {
            UISystem.ShowWindow<LobbyWindow>();
            
            _lobbyManager.OnEnter();
        }

        public void Exit()
        {
            _lobbyManager.OnExit();
            
            UISystem.CloseWindow<LobbyWindow>();
        }
    }
}