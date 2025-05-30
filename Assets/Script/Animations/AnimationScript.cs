using System.Collections;
using UnityEngine;

public class AnimationScript : MonoBehaviour
{
    private Animator animator;
    private Rigidbody rb;
    private bool isDead = false;

    public bool IsDead => isDead;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    public void PlaySpawn()
    {
        animator.SetTrigger("Spawn");
    }

    public void PlayWalk(bool shouldWalk)
    {
        animator.SetBool("isWalking", shouldWalk);
    }

    public void PlayAttack()
    {
        if (!isDead)
            animator.SetTrigger("Attack");
    }

    public void PlayDeath()
    {
        if (isDead) return;

        isDead = true;
        animator.SetTrigger("Die");
        rb.isKinematic = true;
        StartCoroutine(DestroyAfterDelay());
    }

    private IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(3.5f);
        Destroy(gameObject);
    }
}