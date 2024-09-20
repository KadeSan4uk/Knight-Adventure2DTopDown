using System;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using Unity.VisualScripting;
using KnightAdventure.Utilities;
using System.Collections.Generic;
using UnityEngine.InputSystem.XR.Haptics;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private State _startingState;
    [SerializeField] private float _roamingDistanceMax = 7f;
    [SerializeField] private float _roamingDistanceMin = 3f;
    [SerializeField] private float _roamingTimerMax = 2f;

    [SerializeField] private bool _isChasingEnemy = false;
    private float _chasingDistance = 4f;
    private float _chasingSpeedMultiplier = 2f;

    [SerializeField] private bool _isAttackingEnemy = false;
    private float _attackingDistance = 2f;
    private float _attackRate = 2f;
    private float _nextAttackTime = 0f;

    private NavMeshAgent _navMeshAgent;
    private State _currentState;
    private float _roamingTimer;
    private Vector3 _roamPosition;
    private Vector3 _startPosition;

    private float _roamingSpeed;
    private float _chasingSpeed;

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
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.updateRotation = false;
        _navMeshAgent.updateUpAxis = false;
        _currentState = _startingState;
        _roamingSpeed = _navMeshAgent.speed;
        _chasingSpeed = _navMeshAgent.speed * _chasingSpeedMultiplier;
    }

    private void Update()
    {
        StateHandler();
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
            if (newState != State.Chasing)
            {
                _navMeshAgent.ResetPath();
                _navMeshAgent.speed = _chasingSpeed;
            }
            else if (newState == State.Roaming)
            {
                _navMeshAgent.speed = _roamingSpeed;
                _roamingTimer = 0f;
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

    public bool IsRunning()
    {
        if (_navMeshAgent.velocity == Vector3.zero)
        {
            return false;
        }
        else return true;
    }

    private void Roaming()
    {
        _startPosition = transform.position;
        _roamPosition = GetRoamingPosition();
        ChangeFacingDirection(_startPosition, _roamPosition);
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
