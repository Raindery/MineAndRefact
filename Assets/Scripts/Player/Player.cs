using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Diagnostics;

namespace MineAndRefact.Core
{
    [RequireComponent(typeof(NavMeshAgent), typeof(ResourceHolder))]
    public class Player : MonoBehaviour
    {
        [Header("General")]
        [SerializeField] private PlayerSettings _playerSettings;
        [SerializeField] private Transform _dropResourcePoint;
        [Header("Gameplay Event Listener")]
        [SerializeField] private GameplayEventListener _gameplayEventListener;
        
        private Animator _animator;
        private PlayerController _playerController;
        private Vector3 _moveDirection;
        private Coroutine _mineCoroutine;
        private ISource _currentMiningSource;
        private Coroutine _dropCoroutine;
        private ISpot _currentDropingSpot;
        private Queue<IResource> _currentDropResourcesInSpot;
        
        protected bool HasGameplayEventListener => _gameplayEventListener != null;
        protected bool HasAnimator => _animator != null && _animator.isActiveAndEnabled;
        protected bool HasPlayerController => _playerController != null;
        protected Vector3 DropResourcePosition
        {
            get
            {
                if (_dropResourcePoint == null)
                    return CachedTransform.position;
                return _dropResourcePoint.position;
            }
        }
        protected GameplayEventListener GameplayEventListener => _gameplayEventListener;
        protected PlayerSettings PlayerSettings => _playerSettings;
        protected PlayerController PlayerController => _playerController;
        protected Animator Animator => _animator;

        public bool CanDoAction
        {
            get
            {
                if (HasPlayerController)
                    return !_playerController.IsHoldJoystick;
                else
                    return _moveDirection == Vector3.zero;
            }
        }


        private Transform _cachedTransform;
        public Transform CachedTransform
        {
            get
            {
                if (_cachedTransform == null)
                    _cachedTransform = transform;
                return _cachedTransform;
            }
        }

        private NavMeshAgent _navMeshAgent;
        public NavMeshAgent CachedNavMeshAgent
        {
            get
            {
                if (_navMeshAgent == null)
                    _navMeshAgent = GetComponent<NavMeshAgent>();
                return _navMeshAgent;
            }
        }

        private ResourceHolder _resourceHolder;
        public ResourceHolder CachedResourceHolder
        {
            get
            {
                if (_resourceHolder == null)
                    _resourceHolder = GetComponent<ResourceHolder>();
                return _resourceHolder;
            }
        }

        
        private void OnValidate()
        {
            if (_playerSettings == null)
                throw new System.ArgumentNullException(nameof(_playerSettings));
        }

        private void Awake()
        {
            if (_playerSettings == null)
                throw new System.ArgumentNullException(nameof(_playerSettings));

            TryGetComponent<PlayerController>(out _playerController);
            TryGetComponent<Animator>(out _animator);
            _currentDropResourcesInSpot = new Queue<IResource>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.K))
                Utils.ForceCrash(ForcedCrashCategory.Abort);
                
            if (HasPlayerController)
            {
                if (_playerController.Direction != Vector2.zero)
                {
                    Move(_playerController.Direction);
                }
                else
                {
                    if (_moveDirection != Vector3.zero)
                        _moveDirection = Vector3.zero;
                }
            }
        }

        private void LateUpdate()
        {
            if (HasAnimator)
            {
                _animator.SetFloat("MovementMagnitude", Mathf.Abs(_moveDirection.magnitude));
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.TryGetComponent<IResource>(out IResource resource) && resource.CanPickUp)
            {
                resource.PickUp();
                CachedResourceHolder.IncreaseResourceAmount(resource.ResourceId, 1);
            }

            if(other.TryGetComponent<ISource>(out ISource source))
                _currentMiningSource = source; 

            if(other.TryGetComponent<ISpot>(out ISpot spot))
                _currentDropingSpot = spot;
        }

        private void OnTriggerStay(Collider other)
        {
            if(_currentMiningSource != null)
            {
                bool isCanMining = false;
                if (CanDoAction && !_currentMiningSource.IsDepletion)
                    isCanMining = true;
                else if (!CanDoAction || _currentMiningSource.IsDepletion)
                    isCanMining = false;

                if (isCanMining && _mineCoroutine == null)
                {
                    _mineCoroutine = Mine(_currentMiningSource);
                }
                else if(!isCanMining && _mineCoroutine != null)
                {
                    StopCoroutine(_mineCoroutine);
                    _mineCoroutine = null;
                }

                if (HasAnimator)
                    _animator.SetBool("IsMine", isCanMining);     
            }

            if(_currentDropingSpot != null)
            {
                int requiredResourceAmount =
                    CachedResourceHolder.GetResourceAmount(_currentDropingSpot.SpotSettings.RequiredResource.ResourceId);

                bool isCanDropping = false;
                if (CanDoAction && !_currentDropingSpot.IsFullLoaded && !_currentDropingSpot.IsRecyclingProcessed && requiredResourceAmount > 0)
                    isCanDropping = true;
                else if (!CanDoAction || _currentDropingSpot.IsFullLoaded || _currentDropingSpot.IsRecyclingProcessed)
                    isCanDropping = false;

                if(isCanDropping && _dropCoroutine == null)
                {
                    _dropCoroutine = DropInSpot(_currentDropingSpot);
                }
                else if(!isCanDropping && _dropCoroutine != null )
                {
                    StopCoroutine(_dropCoroutine);
                    _dropCoroutine = null;

                    if (_currentDropResourcesInSpot.Count > 0)
                        ClearAndDestroyDropResources();
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if(other.TryGetComponent<ISource>(out ISource source))
            {
                if (_currentMiningSource != null && _currentMiningSource == source)
                    _currentMiningSource = null;

                if (HasAnimator)
                    _animator.SetBool("IsMine", false);

                if(_mineCoroutine != null)
                {
                    StopCoroutine(_mineCoroutine);
                    _mineCoroutine = null;
                }
            }

            if(other.TryGetComponent<ISpot>(out ISpot spot))
            {
                if (_currentDropingSpot != null && _currentDropingSpot == spot)
                    _currentDropingSpot = null;

                if(_dropCoroutine != null)
                {
                    StopCoroutine(_dropCoroutine);
                    _dropCoroutine = null;
                }

                if (_currentDropResourcesInSpot.Count > 0)
                    ClearAndDestroyDropResources();
            }
        }


        private void Rotate()
        {
            Vector3 rotateDirection = _moveDirection;
            rotateDirection.Normalize();
            float rotation = Mathf.Atan2(rotateDirection.x, rotateDirection.z) * Mathf.Rad2Deg;
            CachedTransform.rotation = Quaternion.Euler(0, rotation, 0);
        }

        private void ClearAndDestroyDropResources()
        {
            while (_currentDropResourcesInSpot.TryDequeue(out IResource resource))
            {
                resource.ResourceDestroy();
            }
        }

        private IEnumerator MineCoroutine(ISource source)
        {
            float mineSpeed = source.SourceSettings.MineSpeed;

            if (HasAnimator)
            {
                _animator.SetFloat("MineSpeed", mineSpeed);
                yield return new WaitUntil(() => _animator.GetCurrentAnimatorStateInfo(0).IsName("Mining"));
            }

            while (source != null && !source.IsDepletion)
            {
                float delay;
                if (HasAnimator)
                    delay = _animator.GetCurrentAnimatorStateInfo(0).length;
                else
                    delay = _playerSettings.DefaultActionDelay / mineSpeed;

                yield return new WaitForSeconds(delay);
                source.Mine();

                if (HasGameplayEventListener)
                    _gameplayEventListener.SourceMined.Invoke(source);
            }
            yield break;
        }
        public Coroutine Mine(ISource source)
        {
            return StartCoroutine(MineCoroutine(source));
        }

        private IEnumerator DropInSpotCoroutine(ISpot spot)
        {
            while (spot != null && !spot.IsRecyclingProcessed && !spot.IsFullLoaded)
            {
                int resourceAmount = CachedResourceHolder.GetResourceAmount(spot.SpotSettings.RequiredResource.ResourceId);
                if (resourceAmount <= 0)
                    break;

                int maxDropResourceAmount = _playerSettings.MaxDropResourceAmountInOneTime;

                if (maxDropResourceAmount > spot.RemainsToLoadAmountResources)
                    maxDropResourceAmount = spot.RemainsToLoadAmountResources;

                if (maxDropResourceAmount > resourceAmount)
                    maxDropResourceAmount = resourceAmount;

                for (int i = 0; i < maxDropResourceAmount; i++)
                {
                    IResource resource = Instantiate(spot.SpotSettings.RequiredResource, DropResourcePosition, Quaternion.identity);
                    resource.SetEnableInteractionComponents(false);
                    _currentDropResourcesInSpot.Enqueue(resource);

                    Vector3 scutter = resource.CachedTransform.position + _playerSettings.RandomDropResourceInSpotScatter;
                    if (i >= maxDropResourceAmount - 1)
                        yield return resource.MoveTo(scutter, _playerSettings.ScutterMovementDuration);
                    else
                        resource.MoveTo(scutter, _playerSettings.ScutterMovementDuration);
                }

                yield return new WaitForSeconds(_playerSettings.DropResourceInSpotDelay);

                while (_currentDropResourcesInSpot.TryPeek(out IResource resource))
                {
                    yield return resource.MoveTo(spot.CachedTransform.position, _playerSettings.DropResourceInSpotDuration);

                    CachedResourceHolder.DecreaseResourceAmount(resource.ResourceId, 1);
                    _currentDropResourcesInSpot.Dequeue();

                    spot.LoadRequiredResource(resource.ResourceId);
                    resource.ResourceDestroy();

                    if (HasGameplayEventListener)
                        _gameplayEventListener.SpotLoaded.Invoke(spot);
                }
            }
            yield break;
        }
        public Coroutine DropInSpot(ISpot spot)
        {
            return StartCoroutine(DropInSpotCoroutine(spot));
        }

        public void Move(Vector2 direction)
        {
            _moveDirection.Set(direction.x, 0, direction.y);
            CachedNavMeshAgent.Move(_playerSettings.MovementMultiplier * Time.deltaTime * _moveDirection);
            Rotate();
        }
    }
}