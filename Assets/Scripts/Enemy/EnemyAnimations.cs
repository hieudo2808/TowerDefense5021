using System.Collections;
using UnityEngine;

public class EnemyAnimations : MonoBehaviour
{
    private Animator _animator;
    private Enemy _enemy;
    private EnemyHealth _enemyHealth;
    private bool isAnimating = false;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _enemy = GetComponent<Enemy>();
        _enemyHealth = GetComponent<EnemyHealth>();
    }

    private void SetAnimationTrigger(string trigger)
    {
        _animator.SetTrigger(trigger);
    }

    private float GetCurrentAnimationLength(string stateName)
    {
        RuntimeAnimatorController animatorController = _animator.runtimeAnimatorController;
        AnimationClip[] clips = animatorController.animationClips;

        foreach (AnimationClip clip in clips)
        {
            if (clip.name.Contains(stateName))
            {
                return clip.length;
            }
        }

        return 0.5f;
    }

    private IEnumerator PlayHurt()
    {
        if (isAnimating) yield break;
        isAnimating = true;
        int temp = _enemy.MoveSpeed;
        _enemy.StopMovement();
        SetAnimationTrigger("hurt");
        yield return new WaitForSeconds(GetCurrentAnimationLength("hurt") + 0.3f);
        _enemy.ResumeMovement(temp);
        isAnimating = false;
    }

    private IEnumerator PlayDead()
    {
        _animator.SetBool("isDead", true);
        isAnimating = true;
        int temp = _enemy.MoveSpeed;
        _enemy.StopMovement();
        SetAnimationTrigger("die");
        yield return new WaitForSeconds(GetCurrentAnimationLength("die") * 1.2f);
        _enemyHealth.ResetHealth();
        _enemy.ResumeMovement(temp);
        isAnimating = false;
        ObjectPooler.ReturnToPool(_enemy.gameObject);
    }

    private void EnemyHit(Enemy enemy)
    {
        if (_enemy == enemy)
        {
            StartCoroutine(PlayHurt());
        }
    }

    private void EnemyDead(Enemy enemy)
    {
        if (_enemy == enemy)
        {
            StartCoroutine(PlayDead());
        }
    }

    private void OnEnable()
    {
        EnemyHealth.OnEnemyHit += EnemyHit;
        EnemyHealth.OnEnemyKilled += EnemyDead;
    }

    private void OnDisable()
    {
        EnemyHealth.OnEnemyHit -= EnemyHit;
        EnemyHealth.OnEnemyKilled -= EnemyDead;
    }
}