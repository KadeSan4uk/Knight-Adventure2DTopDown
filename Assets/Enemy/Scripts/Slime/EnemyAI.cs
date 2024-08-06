using System;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using Unity.VisualScripting;
using KnightAdventure.Utilities;
using System.Collections.Generic;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private State _startingState;
    [SerializeField] private float _roamingDistanceMax = 7f;
    [SerializeField] private float _roamingDistanceMin = 3f;
    [SerializeField] private float _roamingTimerMax = 2f;

    private NavMeshAgent _navMeshAgent;
    private State _state;
    private float _roamingTime;
    private Vector3 _roamPosition;
    private Vector3 _startPosition;

    private enum State
    {
        Idle,
        Roaming,
    }

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.updateRotation = false;
        _navMeshAgent.updateUpAxis = false;
        _state = _startingState;
    }
    private void Start()
    {
        _startPosition = transform.position;
    }

    private void Update()
    {
        switch (_state)
        {
            default:
            case State.Idle:
                break;
            case State.Roaming:
                _roamingTime -= Time.deltaTime;
                if (_roamingTime < 0)
                {
                    Roaming();
                    _roamingTime = _roamingTimerMax;
                }
                break;
        }
    }

    private void Roaming()
    {
        _roamPosition = GetRoamingPosition();
        _navMeshAgent.SetDestination(_roamPosition);
    }

    private Vector3 GetRoamingPosition()
    {
        return _startPosition + Utilities.GetRandomDir() * UnityEngine.Random.Range(_roamingDistanceMin, _roamingDistanceMax);
    }    
}
