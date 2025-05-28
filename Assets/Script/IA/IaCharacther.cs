using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class IaCharacther : MonoBehaviour, IDemageble
{
    [SerializeField] private float priorityDetectionRadius = 10f;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float damage;
    [SerializeField] private int _type = 0;
    [SerializeField] private LayerMask detectionLayer;
    private Weapon weapon;
    private AnimationScript animationScript;

    private bool isAttacking = false;
    private bool animationFinished = false;
    private bool isAllyOnWay = false;

    private NavMeshAgent agent;
    private Transform mainTarget;
    private Transform priorityTarget;
    private Transform currentTarget;

    [SerializeField] private float _hp = 10;

    private Rigidbody rb;
    [SerializeField] private Role role;

    public bool IsAttacking { get => isAttacking; set => isAttacking = value; }

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = attackRange;

        weapon = GetComponentInChildren<Weapon>();
        rb = GetComponent<Rigidbody>();

        animationScript = GetComponent<AnimationScript>();

        animationScript.Spawn();
        FindMainTarget();
    }

    private void Update()
    {
        DetectPriorityTargets();
        FindMainTarget();

        if ((priorityTarget == null || !priorityTarget.gameObject.activeInHierarchy) &&
        (mainTarget == null || !mainTarget.gameObject.activeInHierarchy))
        {
            currentTarget = null;
            isAttacking = false;
        }
        else
        {
            currentTarget = priorityTarget != null ? priorityTarget : mainTarget;
        }


        if (currentTarget == null)
        {
            rb.isKinematic = true;
            agent.ResetPath();
            agent.isStopped = true;
            agent.speed = 0f;
        }

        if (currentTarget != null)
        {
            agent.isStopped = false;
            rb.isKinematic = false;
            agent.speed = 1;

            float distance = Vector3.Distance(transform.position, currentTarget.position);
            Vector3 dir = (currentTarget.position - transform.position).normalized;

            if (role == Role.Barbaro || role == Role.Warrior)
            {
                if (distance > attackRange)
                {
                    if (!agent.hasPath || agent.destination != currentTarget.position)
                    {
                        if (animationFinished == false)
                        {
                            animationScript.Andar();
                            StartCoroutine(AnimationCoolDown());
                        }

                        agent.SetDestination(currentTarget.position);

                        Quaternion lookRot = Quaternion.LookRotation(new Vector3(-dir.x, -dir.y,-dir.z));
                        transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * 5f);
                    }
                   
                    agent.isStopped = false;
                }
                else
                {
                    agent.isStopped = true;

                   // Vector3 dir = (currentTarget.position - transform.position).normalized;
                    if (dir != Vector3.zero)
                    {
                        Quaternion lookRot = Quaternion.LookRotation(new Vector3(-dir.x, -dir.y,-dir.z));
                        transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * 5f);

                        if (animationFinished == false)
                        {
                            animationScript.Atacar();
                            StartCoroutine(AnimationCoolDown());
                        }
                    }  
                }
            }
            if(role == Role.Archer || role == Role.Wizard)
            {
                if (distance > attackRange)
                {
                    if (!agent.hasPath || agent.destination != currentTarget.position)
                    {
                        if (animationFinished == false)
                        {
                            animationScript.Andar();
                            StartCoroutine(AnimationCoolDown());
                        }

                        agent.SetDestination(currentTarget.position);
                    }
                    agent.isStopped = false;
                    isAttacking = false;
                    
                }
                else
                {
                    agent.isStopped = true;
                    isAttacking = true;
                    //Vector3 dir = (currentTarget.position - transform.position).normalized;
                    if (dir != Vector3.zero)
                    {
                        Quaternion lookRot = Quaternion.LookRotation(new Vector3(-dir.x, -dir.y, -dir.z));
                        transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * 5f);
                        
                        if(isAllyOnWay == false)
                        {
                            if (IsLineBlockedByAlly(dir))
                            {
                            StartCoroutine(StepAside());
                            return;
                            }

                        }
                        if (animationFinished == false)
                        {
                            animationScript.Atacar();
                            StartCoroutine(AnimationCoolDown());
                        }

                    }
                }   
            }
        }
    }

    private bool IsLineBlockedByAlly(Vector3 direction)
    {
        Vector3 backward = direction;
        Vector3 origin = transform.position;

        Debug.DrawRay(origin, backward * attackRange, Color.pink);

        if (Physics.Raycast(origin, backward, out RaycastHit hit, attackRange, detectionLayer))
        {
            IaCharacther ally = hit.collider.GetComponentInParent<IaCharacther>();
            if (ally != null && ally != this && ally.GetTypeValue() == this._type)
            {
                return true;
            }
        }

        return false;
    }

    private IEnumerator StepAside()
    {
        agent.isStopped = true;
        agent.ResetPath();
        isAllyOnWay = true;

        float offset = Random.value > 0.5f ? 0.15f : 0.15f;
        Vector3 sideDir = transform.right * offset;
        Vector3 target = transform.position + sideDir;

        float time = 0f;
        float duration = 1f;
        while (time < duration)
        {
            transform.position = Vector3.Lerp(transform.position, target, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        isAllyOnWay = false;
        agent.isStopped = false;
    }



    private void FindMainTarget()
    {
        var allStructures = FindObjectsOfType<structure>();
        float closestDistance = Mathf.Infinity;
        Transform closest = null;

        foreach (var structure in allStructures)
        {
            if (structure != null && structure.GetTypeValue() == 1)
            {
                float dist = Vector3.Distance(transform.position, structure.transform.position);
                if (dist < closestDistance)
                {
                    closestDistance = dist;
                    closest = structure.transform;
                }
            }
        }

        mainTarget = closest;

        if(allStructures.Length == 0)
        {
            mainTarget = null;
        }
    }

    private void DetectPriorityTargets()
    {
        var allEnemies = FindObjectsOfType<IaCharacther>();
        float closestDistance = Mathf.Infinity;
        Transform closest = null;

        foreach (var enemy in allEnemies)
        {
            if (enemy == this) continue;
            if (enemy.GetTypeValue() != _type)
            {
                float dist = Vector3.Distance(transform.position, enemy.transform.position);
                if (dist <= priorityDetectionRadius && dist < closestDistance)
                {
                    closestDistance = dist;
                    closest = enemy.transform;
                }
            }
        }

        priorityTarget = closest;

        if (allEnemies.Length == 0)
        {
            priorityTarget = null;
        }
    }

    public void SetPriorityTarget(Transform newTarget)
    {
        priorityTarget = newTarget;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (role == Role.Archer || role == Role.Wizard) return;

        if(role == Role.Barbaro || role == Role.Warrior)
        {
            if (!IsAttacking)
            {
                IDemageble target = collision.gameObject.GetComponent<IDemageble>();
                if (target != null)
                {
                StartCoroutine(AttackAfterDelay(target));
                }
            }
        }
    }

    private IEnumerator AttackAfterDelay(IDemageble target)
    {
        IsAttacking = true;
        yield return new WaitForSeconds(0.5f);

        if (target != null)
        {
            target.TakeDamage(damage);
        }

        IsAttacking = false;
    }

    public void TakeDamage(float dmg)
    {
        _hp -= dmg;
        if (_hp <= 0)
        {
            animationScript.Morrer();
        }
    }

    public int GetTypeValue()
    {
        return _type;
    }

    IEnumerator AnimationCoolDown()
    {
        animationFinished = true;
        yield return new WaitForSeconds(2);
        animationFinished = false;
    }
    private enum Role
    {
       None,
       Archer,
       Warrior,
       Barbaro,
       Wizard
    }   
}