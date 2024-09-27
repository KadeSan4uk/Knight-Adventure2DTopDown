using System;
using UnityEngine;
using System.Collections;
using JetBrains.Annotations;
using System.Collections.Generic;

[RequireComponent(typeof(PolygonCollider2D))]//adding this component to the inspector if it is missing.

public class EnemyEntity : MonoBehaviour
{
    [SerializeField] private int _maxHealth;
    private int _curentHealth;

    private PolygonCollider2D _polygonCollider2D;

    private void Awake()
    {
        _polygonCollider2D = GetComponent<PolygonCollider2D>();
    }

    private void Start()
    {
        _curentHealth = _maxHealth;
    }

    public void TakeDamage(int damage)
    {
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
            Destroy(gameObject);
    }
}
