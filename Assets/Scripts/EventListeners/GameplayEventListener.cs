using UnityEngine;
using UnityEngine.Events;

namespace MineAndRefact.Core
{
    public sealed class GameplayEventListener : MonoBehaviour
    {
        [Header("Gameplay Events")]
        [SerializeField] private UnityEvent<string, int> _resourceDataLoaded = new UnityEvent<string, int>();
        [SerializeField] private UnityEvent<string, int> _resourceAmountChanged = new UnityEvent<string, int>();
        [SerializeField] private UnityEvent<ISource> _sourceMined = new UnityEvent<ISource>();
        [SerializeField] private UnityEvent<ISpot> _spotLoaded = new UnityEvent<ISpot>();

        public UnityEvent<string, int> ResourceAmountChanged => _resourceAmountChanged;
        public UnityEvent<string, int> ResourceDataLoaded => _resourceDataLoaded;
        public UnityEvent<ISource> SourceMined => _sourceMined;
        public UnityEvent<ISpot> SpotLoaded => _spotLoaded;
    }
}