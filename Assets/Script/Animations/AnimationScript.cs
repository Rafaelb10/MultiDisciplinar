using System.Collections;
using UnityEngine;

public class AnimationScript : MonoBehaviour
{
    private Animator animator;
    private bool isDead = false;
    private bool walking = false;
    private bool running = false;
    void Start()
    {
        animator = GetComponent<Animator>();
       
    }

    void Update()
    {
        if (isDead) return;

        Andar();
       
    }
    //public void Spawn()
    //{
    //    animator.SetTrigger("Spawn");
    //}
    public void Andar()
    {
        if(walking == true)
        {
            animator.SetBool("isWalking", walking);
        }
        
        
    }
    public void Correr()
    {
        if(running == true)
        {
            animator.SetBool("isRunning", running);
        }
    }
    public void Crawl()
    {
        if (!running)
        {
            animator.SetBool("isCrawling", running);
        }
    }
    public void Attack2()
    {
        animator.SetTrigger("Attack2");
    }
    public void IsWalking()
    {
        walking = true;
       
    }
    public void IsNotWalking()
    {
        walking = false;

    }
    public void IsRunning()
    {
        running = true;
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
        yield return new WaitForSeconds(3.5f);
        Destroy(gameObject);
    }
}


