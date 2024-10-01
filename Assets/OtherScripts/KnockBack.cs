using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class KnockBack : MonoBehaviour
{
    [SerializeField] private float _knockBackForce = 2f;
    [SerializeField] private float _knockBackMovingTimerMax = 0.2f;

    private float _knockBackMovingTimer;

    private Rigidbody2D _rb;

    public bool IsGettingKcnokedBack { get; private set; }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        _knockBackMovingTimer -= Time.deltaTime;
        if (_knockBackMovingTimer < 0)
            StopKnockBackMovement();
    }

    public void GetKnockedBack(Transform damageSource)
    {
        IsGettingKcnokedBack = true;
        _knockBackMovingTimer = _knockBackMovingTimerMax;
        Vector2 difference = (transform.position - damageSource.position)
            .normalized * _knockBackForce / _rb.mass;
        _rb.AddForce(difference, ForceMode2D.Impulse);
    }

    public void StopKnockBackMovement()
    {
        _rb.velocity = Vector2.zero;
        IsGettingKcnokedBack = false;
    }
}
