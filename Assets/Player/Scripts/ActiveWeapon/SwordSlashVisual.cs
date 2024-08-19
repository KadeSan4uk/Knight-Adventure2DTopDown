using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SwordSlashVisual : MonoBehaviour
{
    [SerializeField] private Sword _sword;

    private const string ATTACK = "Attack";
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _sword.OnSwordSwing += _sword_OnSwordSwing;
    }

    private void _sword_OnSwordSwing(object sender, System.EventArgs e)
    {
        _animator.SetTrigger(ATTACK);
    }
}
