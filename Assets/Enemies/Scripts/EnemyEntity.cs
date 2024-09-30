using System;
using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]//adding this component to the inspector if it is missing.
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(EnemyAI))]

public class EnemyEntity : MonoBehaviour
{
    [SerializeField] private EnemySO _enemySO;
    public event EventHandler OnTakeHit;
    public event EventHandler OnDeath;

    //[SerializeField] private int _maxHealth;
    private int _curentHealth;

    private PolygonCollider2D _polygonCollider2D;
    private BoxCollider2D _boxCollider2D;
    private EnemyAI _enemyAI;

    private void Awake()
    {
        _polygonCollider2D = GetComponent<PolygonCollider2D>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _enemyAI = GetComponent<EnemyAI>();
    }

    private void Start()
    {
        _curentHealth = _enemySO.enemyHealth;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Attack");
    }

    public void TakeDamage(int damage)
    {
        OnTakeHit?.Invoke(this, EventArgs.Empty);
        _curentHealth -= damage;

        DetectDeath();
    }

    public void PolygonColliderTurnOff()
    {
        _polygonCollider2D.enabled = false;
    }

    public void PolygonColliderTurnOn()
    {
        _polygonCollider2D.enabled = true;
    }

    private void DetectDeath()
    {
        if (_curentHealth <= 0)
        {
            _boxCollider2D.enabled = false;
            _polygonCollider2D.enabled = false;

            _enemyAI.SetDeathState();

            OnDeath?.Invoke(this, EventArgs.Empty);
        }
    }
}
