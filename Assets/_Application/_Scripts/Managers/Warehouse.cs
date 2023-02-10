using _Application.Scripts.UI;
using UnityEngine;

namespace _Application.Scripts.Managers
{
    public class Warehouse : MonoBehaviour
    {
        [SerializeField] private Counter _counterPrefab;

        public Counter CounterPrefab => _counterPrefab;
    }
}