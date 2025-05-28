using System.Collections;
using UnityEngine;

public class AnimationScript : MonoBehaviour
{
    private Animator animator;
    private bool isDead = false;
    private bool walking = false;
    private Rigidbody rb;
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (isDead) return;

        Andar();
       
    }
    public void Spawn()
    {
        animator.SetTrigger("Spawn");
    }
    public void Andar()
    {
        if(walking == true)
        {
            animator.SetBool("isWalking", walking);
        }
        
        
    }
    public void IsWalking()
    {
        walking = true;
       
    }
    public void IsNotWalking()
    {
        walking = false;

    }

    public void Atacar()
    {
   
      animator.SetTrigger("Attack");
    }
    public void Morrer()
    {
       
      animator.SetTrigger("Die");
      isDead = true;
      StartCoroutine(DestroyObject());

    }
    IEnumerator DestroyObject()
    {
        rb.isKinematic = true;
        yield return new WaitForSeconds(3.5f);
        Destroy(gameObject);
    }
}


