using System;
using UnityEngine;
using UnityEngine.AI;
using KnightAdventure.Utilities;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private State _startingState;
    [SerializeField] private float _roamingDistanceMax = 7f;
    [SerializeField] private float _roamingDistanceMin = 3f;
    [SerializeField] private float _roamingTimerMax = 2f;

    [SerializeField] private bool _isChasingEnemy = false;
    [SerializeField] private float _chasingDistance = 5f;
    [SerializeField] private float _chasingSpeedMultiplier = 2f;

    [SerializeField] private bool _isAttackingEnemy = false;
    [SerializeField] private float _attackingDistance = 2f;
    [SerializeField] private float _attackRate = 1f;
    private float _nextAttackTime = 0f;

    private NavMeshAgent _navMeshAgent;
    private State _currentState;
    private float _roamingTimer;
    private Vector3 _roamPosition;
    private Vector3 _startPosition;

    private float _roamingSpeed;
    private float _chasingSpeed;

    private float _nextCheckDirectionTime = 0;
    private float _checkDirectionDuration = 0.1f;
    private Vector3 _lastPosition;

    public event EventHandler OnEnemyAttack;

    private enum State
    {
        Idle,
        Roaming,
        Chasing,
        Attacking,
        Death
    }

    private void Awake()
    {
        SetupNavigationAndState();
    }

    private void Update()
    {
        StateHandler();
        MovementDirectionHandler();
    }

    public bool IsRunning()
    {
        if (_navMeshAgent.velocity == Vector3.zero)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public float GetRoamingAnimationSpeed()
    {
        return _navMeshAgent.speed / _roamingSpeed;
    }

    public void SetDeathState()
    {
        _navMeshAgent.ResetPath();
        _currentState = State.Death;
    }

    private void SetupNavigationAndState()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.updateRotation = false;
        _navMeshAgent.updateUpAxis = false;
        _currentState = _startingState;
        _roamingSpeed = _navMeshAgent.speed;
        _chasingSpeed = _navMeshAgent.speed * _chasingSpeedMultiplier;
    }

    private void StateHandler()
    {
        switch (_currentState)
        {
            case State.Roaming:
                _roamingTimer -= Time.deltaTime;
                if (_roamingTimer < 0)
                {
                    Roaming();
                    _roamingTimer = _roamingTimerMax;
                }
                CheckCurrentState();
                break;
            case State.Chasing:
                ChasingTarget();
                CheckCurrentState();
                break;
            case State.Attacking:
                AttackingTarget();
                CheckCurrentState();
                break;
            case State.Death:
                break;
            default:
            case State.Idle:
                break;
        }
    }

    private void MovementDirectionHandler()
    {
        if (Time.time > _nextCheckDirectionTime)
        {
            if (IsRunning())
            {
                ChangeFacingDirection(_lastPosition, transform.position);
            }
            else if (_currentState == State.Attacking)
            {
                ChangeFacingDirection(transform.position, Player.Instance.transform.position);
            }

            _lastPosition = transform.position;
            _nextCheckDirectionTime = Time.time + _checkDirectionDuration;
        }
    }

    private void AttackingTarget()
    {
        if (Time.time > _nextAttackTime)
        {
            OnEnemyAttack?.Invoke(this, EventArgs.Empty);

            _nextAttackTime = Time.time + _attackRate;
        }
    }

    private void CheckCurrentState()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, Player.Instance.transform.position);
        State newState = State.Roaming;

        if (_isChasingEnemy)
        {
            if (distanceToPlayer <= _chasingDistance)
            {
                newState = State.Chasing;
            }
        }

        if (_isAttackingEnemy)
        {
            if (distanceToPlayer < _attackingDistance)
            {
                newState = State.Attacking;
            }
        }

        if (newState != _currentState)
        {
            if (newState == State.Chasing)
            {
                _navMeshAgent.speed = _chasingSpeed;
            }
            else if (newState == State.Roaming)
            {
                _roamingTimer = 0f;
                _navMeshAgent.speed = _roamingSpeed;
            }
            else if (newState == State.Attacking)
            {
                _navMeshAgent.ResetPath();
            }

            _currentState = newState;
        }
    }

    private void ChasingTarget()
    {
        _navMeshAgent.SetDestination(Player.Instance.transform.position);
    }

    private void Roaming()
    {
        _startPosition = transform.position;
        _roamPosition = GetRoamingPosition();
        _navMeshAgent.SetDestination(_roamPosition);
    }

    private Vector3 GetRoamingPosition()
    {
        return _startPosition + Utilities.GetRandomDir() * UnityEngine.Random.Range(_roamingDistanceMin, _roamingDistanceMax);
    }

    private void ChangeFacingDirection(Vector3 soursePosition, Vector3 targetPosition)
    {
        if (soursePosition.x > targetPosition.x)
            transform.rotation = Quaternion.Euler(0, -180, 0);
        else//вращение обьекта по единичному вектору(Quaternion) по осям(Euler) X,Y,Z
            transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}
