using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Player : MonoBehaviour
{
    [Min(1f)]
    [SerializeField] private float _movementMultiplier;

    
    private Animator _animator;
    private PlayerController _playerController;
    private bool _hasPlayerController;
    private bool _hasAnimator;
    private Vector3 _moveDirection = new Vector3();


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
        _hasAnimator = TryGetComponent<Animator>(out _animator);
    }

    private void Update()
    {
        if (_hasPlayerController)
        {
            if(_playerController.Direction != Vector2.zero)
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
        float rot = Mathf.Atan2(rotateDirection.x, rotateDirection.z) * Mathf.Rad2Deg;
        CachedTransform.rotation = Quaternion.Euler(0, rot, 0);
    }
}
