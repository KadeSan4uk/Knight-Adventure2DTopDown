using UnityEngine;

public class SwortVisual : MonoBehaviour
{
    [SerializeField] private Sword _sword;
    private Animator _animator;
    private const string ATTACK = "Attack";

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _sword.OnSwordSwing += _sword_OnSwordSwing;
    }

    public void TriggerEndAttackAnimation()
    {
        _sword.AttackColliderTurnOff();
    }

    private void _sword_OnSwordSwing(object sender, System.EventArgs e)
    {
        _animator.SetTrigger(ATTACK);
    }
}
