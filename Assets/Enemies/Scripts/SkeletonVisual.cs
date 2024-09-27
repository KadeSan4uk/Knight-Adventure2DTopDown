using UnityEngine;

[RequireComponent(typeof(Animator))]

public class SkeletonVisual : MonoBehaviour
{
    [SerializeField] private EnemyAI _enemyAI;
    [SerializeField] private EnemyEntity _enemyEntity;

    private Animator _animator;

    private const string IS_RUNNING = "IsRunning";
    private const string CHASING_SPEED_MULTIPLIER = "ChasingSpeedMultiplier";

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        _animator.SetBool(IS_RUNNING, _enemyAI.IsRunning());
        _animator.SetFloat(CHASING_SPEED_MULTIPLIER, _enemyAI.GetRoamingAnimationSpeed());
    }

    public void TriggerAttackAnimationTurnOff()
    {
        _enemyEntity.PolygonColliderTurnOff();
    }

    public void TriggerAttackAnimationTurnOn()
    {
        _enemyEntity.PolygonColliderTurnOn();
    }
}
