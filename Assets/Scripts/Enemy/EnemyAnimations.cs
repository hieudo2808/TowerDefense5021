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
        _animator.SetTrigger("hurt");
        yield return new WaitForSeconds(GetCurrentAnimationLength("hurt") + 0.3f);
        _enemy.ResumeMovement(temp);
        isAnimating = false;
    }

    private IEnumerator PlayDead()
    {
        if (isAnimating) yield break;
        isAnimating = true;
        int temp = _enemy.MoveSpeed;
        _enemy.StopMovement();
        _animator.Play("die", 0, 0f);
        yield return new WaitForSeconds(GetCurrentAnimationLength("die"));    
        
        _enemyHealth.ResetHealth();
        _enemy.ResumeMovement(temp);
        _animator.Rebind();
        isAnimating = false;

        yield return new WaitForSeconds(0.1f);
        ObjectPooler.ReturnToPool(_enemy.gameObject);
        EnemyHealth.OnEnemyFullyDestroyed?.Invoke(_enemy);
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