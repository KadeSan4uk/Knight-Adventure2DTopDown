using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[SelectionBase]
public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }
    private Rigidbody2D _rb;
    [SerializeField] private float _movingSpeed = 5f;
    private float _minMovingSpeed = 0.1f;
    private bool _isRunning = false;
    private Vector2 _inputVector;

    private void Awake()
    {
        Instance = this;
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        InputManager.Instance.OnPlayerAttack += InputManager_OnPlayerAttack;
    }

    private void InputManager_OnPlayerAttack(object sender, EventArgs e)
    {
        ActiveWeapon.Instance.GetActiveWeapon().Attack();
    }

    private void Update()
    {
        ReadInput();
    }   

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        _rb.MovePosition(_rb.position + _inputVector * (_movingSpeed * Time.fixedDeltaTime));

        if (Mathf.Abs(_inputVector.x) > _minMovingSpeed || MathF.Abs(_inputVector.y) > _minMovingSpeed)
            _isRunning = true;
        else
            _isRunning = false;
    }

    private void ReadInput()
    {
        _inputVector = InputManager.Instance.GetMovementVector();
    }

    public bool IsRunning()
    {
        return _isRunning;
    }

    public Vector3 GetPlayerScreenPosition()
    {
        Vector3 playerScreenPosition = Camera.main.WorldToScreenPoint(transform.position);
        return playerScreenPosition;
    }
}
