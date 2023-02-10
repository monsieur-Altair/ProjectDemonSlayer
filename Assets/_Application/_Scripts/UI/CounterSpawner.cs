using System.Collections.Generic;
using _Application.Scripts.Infrastructure.Services;
using _Application.Scripts.Managers;
using _Application.Scripts.Units;
using Pool_And_Particles;
using UnityEngine;

namespace _Application.Scripts.UI
{
    public class CounterSpawner
    {
        private Transform _counterParent;
        private readonly GlobalPool _pool;
        private readonly GlobalCamera _globalCamera;
        private readonly Warehouse _warehouse;
        private readonly Counter _counterPrefab;

        private readonly Dictionary<BaseUnit, Counter> _unitsCounters = new();

        public CounterSpawner(Warehouse warehouse, GlobalPool pool, GlobalCamera globalCamera, Counter counterPrefab)
        {
            _globalCamera = globalCamera;
            _warehouse = warehouse;
            _pool = pool;

            _counterPrefab = counterPrefab;
        }

        public void FillLists(Transform counterParent)
        {
            _counterParent = counterParent;

            //BaseUnit.Launched += SetCounter;
            //BaseUnit.Updated += UpdateCounterPos;
            //BaseUnit.Approached += FreeCounter;
        }

        private void FreeCounter(BaseUnit unit)
        {
            Counter counter = _unitsCounters[unit];
            _pool.Free(counter);
            _unitsCounters.Remove(unit);
        }

        private void UpdateCounterPos(BaseUnit unit)
        {
            Vector2 counterPos = UISystem.GetUIPosition(_globalCamera.MainCamera, unit.CounterPoint.position);
            _unitsCounters[unit].SetAnchorPos(counterPos);
        }

        private void SetCounter(BaseUnit unit)
        { 
            //Counter counter = _pool.Get(_counterPrefab, parent: _counterParent);
            //int team = (int) unit.UnitInf.UnitTeam;
            //counter.SetColors(_warehouse.counterForeground[team], _warehouse.counterBackground[team]);
            //counter.SetText(Mathf.RoundToInt(unit.UnitInf.UnitCount).ToString());
            //_unitsCounters.Add(unit, counter);
            //UpdateCounterPos(unit);
        }

        public void ClearLists()
        {
            //BaseUnit.Launched -= SetCounter;
            //BaseUnit.Updated -= UpdateCounterPos;
            //BaseUnit.Approached -= FreeCounter;

            foreach (Counter counter in _unitsCounters.Values) 
                _pool.Free(counter);

            _unitsCounters.Clear();
        }

        private Counter SpawnCounterTo(Transform worldPoint)
        {
            Vector3 pos = worldPoint.position;
            Vector2 counterPos = UISystem.GetUIPosition(_globalCamera.MainCamera, pos);
            Counter counter = _pool.Get(_counterPrefab, parent: _counterParent);
            counter.SetAnchorPos(counterPos);
            return counter;
        }
    }
}