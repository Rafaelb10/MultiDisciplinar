using UnityEngine;

public class AnimationScript : MonoBehaviour
{
    private Animator animator;
    private bool isDead = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetTrigger("Spawn");
    }

    void Update()
    {
        if (isDead) return;

        // Caminhar
        bool walking = Input.GetKey(KeyCode.W);
        animator.SetBool("isWalking", walking);

        // Atacar
        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetTrigger("Attack");
        }

        // Morrer
        if (Input.GetKeyDown(KeyCode.K))
        {
            animator.SetTrigger("Die");
            isDead = true;
        }
    }
}

