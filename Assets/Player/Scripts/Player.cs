using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }
    private Rigidbody2D _rb;
    [SerializeField] private float _movingSpeed = 5f;
    private float _minMovingSpeed = 0.1f;
    private bool _isRunning = false;

    private void Awake()
    {
        Instance = this;
        _rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        Vector2 inputVector = InputManager.Instance.GetMovementVector();
        _rb.MovePosition(_rb.position + inputVector * (_movingSpeed * Time.fixedDeltaTime));

        if (Mathf.Abs(inputVector.x) > _minMovingSpeed || MathF.Abs(inputVector.y) > _minMovingSpeed)        
            _isRunning = true;        
        else        
            _isRunning = false;        
    }

    public bool IsRunning()
    {
        return _isRunning;
    }

    public Vector3 GetPlayerScreenPosition()
    {
        Vector3 playerScreenPosition=Camera.main.WorldToScreenPoint(transform.position);
        return playerScreenPosition;
    }
}
