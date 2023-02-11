using System.Collections.Generic;
using _Application.Scripts.Control;
using _Application.Scripts.Infrastructure.Services.Progress;
using _Application.Scripts.Infrastructure.Services.Scriptables;
using _Application.Scripts.Managers;
using _Application.Scripts.UI;
using Pool_And_Particles;
using UnityEngine;

namespace _Application.Scripts.Infrastructure.Services.Factory
{
    public class GameFactory : IService
    {
        public List<IProgressReader> ProgressReaders { get; } 
        public List<IProgressWriter> ProgressWriters { get; }

        private readonly CoreConfig _coreConfig;

        public GameFactory(CoreConfig coreConfig)
        {
            ProgressReaders = new List<IProgressReader>();
            ProgressWriters = new List<IProgressWriter>();

            _coreConfig = coreConfig;
        }

        public GameLoopManager CreateWorld()
        {
            GlobalPool objectPool = AllServices.Get<GlobalPool>();
            CoroutineRunner coroutineRunner = AllServices.Get<CoroutineRunner>();
            
            Warehouse warehouse = _coreConfig.Warehouse;

            GlobalCamera globalCamera = AllServices.Get<GlobalCamera>();
            
            UserControl userControl = AllServices.Get<UserControl>();

            BarSpawner barSpawner = new(_coreConfig, objectPool, globalCamera);

            GameLoopManager gameLoopManager = new(AllServices.Get<LevelManager>(), 
                coroutineRunner, objectPool, AllServices.Get<OutlookService>(), userControl,
                AllServices.Get<ScriptableService>(),
                AllServices.Get<ProgressService>(),
                AllServices.Get<AudioManager>(), 
                _coreConfig, 
                barSpawner);
            
            ProgressReaders.Add(gameLoopManager);
            ProgressWriters.Add(gameLoopManager);

            return gameLoopManager;
        }

        public Level CreateLevel(Level levelPrefab) => 
            Object.Instantiate(levelPrefab, Vector3.zero, Quaternion.identity);

        public void CreateAndRegisterMonoBeh<T>(T prefab) where T : MonoBehaviourService
        {
            T service = Object.Instantiate(prefab);
            service.Init();
            AllServices.Register(service);
        }
    }
}