using System.Collections;
using UnityEngine;
using UnityEngine.AI;


namespace MineAndRefact.Core
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class Player : MonoBehaviour
    {
        [Min(1f)]
        [SerializeField] private float _movementMultiplier;
        [Min(0.01f)]
        [SerializeField] private float _standartActionDelay;
        [SerializeField] private GameplayEventListener _gameplayEventListener;


        private Animator _animator;
        private PlayerController _playerController;
        private bool _hasPlayerController;
        private bool _hasAnimator;
        private Vector3 _moveDirection = new Vector3();
        private Coroutine _mineCoroutine;


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


        private void Awake()
        {
            _hasPlayerController = TryGetComponent<PlayerController>(out _playerController);
            _hasAnimator = TryGetComponent<Animator>(out _animator) && _animator.isActiveAndEnabled;
        }

        private void Update()
        {
            if (_hasPlayerController)
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
            if (_hasAnimator)
            {
                _animator.SetFloat("MovementMagnitude", Mathf.Abs(_moveDirection.magnitude));
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.TryGetComponent<IResource>(out IResource resource) && resource.CanPickUp)
            {
                resource.PickUp();

                if (_gameplayEventListener != null)
                    _gameplayEventListener.PickUpResource.Invoke(resource);
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.TryGetComponent<ISource>(out ISource source))
            {
                if(_mineCoroutine == null && _moveDirection == Vector3.zero && !source.IsDepletion)
                {
                    if (_hasAnimator)
                        _animator.SetBool("IsMine", true);

                    _mineCoroutine = Mine(source);
                }
                else if(_mineCoroutine != null && (_moveDirection != Vector3.zero || source.IsDepletion))
                {
                    if (_hasAnimator)
                        _animator.SetBool("IsMine", false);

                    StopCoroutine(_mineCoroutine);
                    _mineCoroutine = null;
                }
                    
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if(other.TryGetComponent<ISource>(out ISource source))
            {
                if (_hasAnimator)
                    _animator.SetBool("IsMine", false);

                if(_mineCoroutine != null)
                {
                    StopCoroutine(_mineCoroutine);
                    _mineCoroutine = null;
                }
            }
        }


        public void Move(Vector2 direction)
        {
            _moveDirection.Set(direction.x, 0, direction.y);
            CachedNavMeshAgent.Move(_moveDirection * _movementMultiplier * Time.deltaTime);
            Rotate();
        }

        private void Rotate()
        {
            Vector3 rotateDirection = _moveDirection;
            rotateDirection.Normalize();
            float rotation = Mathf.Atan2(rotateDirection.x, rotateDirection.z) * Mathf.Rad2Deg;
            CachedTransform.rotation = Quaternion.Euler(0, rotation, 0);
        }

        private IEnumerator MineCoroutine(ISource source)
        {
            float mineSpeed = source.SourceSettings.MineSpeed;

            if (_hasAnimator)
            {
                _animator.SetFloat("MineSpeed", mineSpeed);
                yield return new WaitUntil(() => _animator.GetCurrentAnimatorStateInfo(0).IsName("Mining"));
            }

            while (!source.IsDepletion)
            {
                float delay;
                if (_hasAnimator)
                    delay = _animator.GetCurrentAnimatorStateInfo(0).length;
                else
                    delay = _standartActionDelay / mineSpeed;

                yield return new WaitForSeconds(delay);

                source.Mine();

                yield return null;
            }
        }
        public Coroutine Mine(ISource source)
        {
            return StartCoroutine(MineCoroutine(source));
        }
    }
}