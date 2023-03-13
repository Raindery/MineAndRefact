using UnityEngine;
using UnityEngine.Events;

namespace MineAndRefact.Core
{
    public class GameplayEventListener : MonoBehaviour
    {
        [Header("Gameplay Events")]
        [SerializeField] private UnityEvent<string, int> _resourceDataLoaded = new UnityEvent<string, int>();
        [SerializeField] private UnityEvent<string, int> _resourceAmountChanged = new UnityEvent<string, int>();


        public UnityEvent<string, int> ResourceAmountChanged => _resourceAmountChanged;
        public UnityEvent<string, int> ResourceDataLoaded => _resourceDataLoaded;
    }
}


