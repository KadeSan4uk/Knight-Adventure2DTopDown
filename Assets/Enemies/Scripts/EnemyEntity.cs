using System;
using UnityEngine;
using System.Collections;
using JetBrains.Annotations;
using System.Collections.Generic;

public class EnemyEntity : MonoBehaviour
{
    [SerializeField] private int _maxHealth;
    private int _curentHealth;

    private void Start()
    {
        _curentHealth = _maxHealth;
    }

    public void TakeDamage(int damage)
    {
        _curentHealth -= damage;

        DetectDeath();
    }

    private void DetectDeath()
    {
        if (_curentHealth <= 0)
        Destroy(gameObject);
    }
}
