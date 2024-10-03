using System;
using UnityEngine;
using System.Collections;

[SelectionBase]//This object will be selected when clicking on any child object of this one on the scene.  

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }
    public event EventHandler OnPlayerDeath;

    [SerializeField] private float _movingSpeed = 5f;
    [SerializeField] private int _maxHealth = 10;
    [SerializeField] private float _damageRecoveryTime = 0.5f;

    private Vector2 _inputVector;

    private Rigidbody2D _rb;
    private KnockBack _knockBack;

    private float _minMovingSpeed = 0.1f;
    private bool _isRunning = false;

    private int _currentHealth;
    private bool _canTakeDamage;
    private bool _isAlive;

    private void Awake()
    {
        Instance = this;
        _rb = GetComponent<Rigidbody2D>();
        _knockBack = GetComponent<KnockBack>();
    }

    private void Start()
    {
        _canTakeDamage = true;
        _isAlive = true;
        _currentHealth = _maxHealth;
        InputManager.Instance.OnPlayerAttack += InputManager_OnPlayerAttack;
    }

    private void Update()
    {
        ReadInput();
    }

    private void FixedUpdate()
    {
        if (_knockBack.IsGettingKcnokedBack)
            return;

        HandleMovement();
    }

    public bool IsAlive() => _isAlive;

    public bool IsRunning()
    {
        return _isRunning;
    }

    public Vector3 GetPlayerScreenPosition()
    {
        Vector3 playerScreenPosition = Camera.main.WorldToScreenPoint(transform.position);
        return playerScreenPosition;
    }

    public void TakeDamage(Transform damageSource, int damage)
    {
        if (!_canTakeDamage || !_isAlive) return;

        ApplyDamage(damage);
        DetectDeath();

        if (!_isAlive) return;

        HandleKnockBack(damageSource);
        StartDamageRecovery();
    }

    private void HandleKnockBack(Transform damageSource)
    {
        _canTakeDamage = false;
        _knockBack.GetKnockedBack(damageSource);
    }

    private void StartDamageRecovery()
    {
        StartCoroutine(DamageRecoveryRoutine());
    }

    private void ApplyDamage(int damage)
    {
        _currentHealth -= damage;
        _currentHealth = Math.Max(0, _currentHealth);
    }

    private void DetectDeath()
    {
        if (_currentHealth <= 0 && _isAlive)
        {
            _isAlive = false;
            _knockBack.StopKnockBackMovement();
            InputManager.Instance.DisableMovement();

            OnPlayerDeath?.Invoke(this, EventArgs.Empty);
        }
    }

    private IEnumerator DamageRecoveryRoutine()
    {
        yield return new WaitForSeconds(_damageRecoveryTime);
        _canTakeDamage = true;
    }

    private void InputManager_OnPlayerAttack(object sender, EventArgs e)
    {
        ActiveWeapon.Instance.GetActiveWeapon().Attack();
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
}
