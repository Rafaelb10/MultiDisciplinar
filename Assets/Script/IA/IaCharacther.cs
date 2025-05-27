using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class IaCharacther : MonoBehaviour, IDemageble
{
    [SerializeField] private float priorityDetectionRadius = 10f;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private int damage = 1;
    [SerializeField] private int _type = 0;
    [SerializeField] private LayerMask detectionLayer;
    private Weapon weapon;

    private bool isAttacking = false;

    private NavMeshAgent agent;
    private Transform mainTarget;
    private Transform priorityTarget;
    private Transform currentTarget;

    private int _hp = 10;

    private Rigidbody rb;
    [SerializeField] private Role role;

    public bool IsAttacking { get => isAttacking; set => isAttacking = value; }

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = attackRange;

        weapon = GetComponentInChildren<Weapon>();
        rb = GetComponent<Rigidbody>();

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

            if (role == Role.Tank || role == Role.Warrior)
            {
                if (distance > attackRange)
                {
                    if (!agent.hasPath || agent.destination != currentTarget.position)
                    {
                        agent.SetDestination(currentTarget.position);
                    }
                   
                    agent.isStopped = false;
                }
                else
                {
                    agent.isStopped = true;

                    Vector3 dir = (currentTarget.position - transform.position).normalized;
                    if (dir != Vector3.zero)
                    {
                        Quaternion lookRot = Quaternion.LookRotation(new Vector3(dir.x, 0, dir.z));
                        transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * 5f);
                    }
                }
            }
            if(role == Role.Archer || role == Role.Wizard)
            {
                if (distance > attackRange)
                {
                    if (!agent.hasPath || agent.destination != currentTarget.position)
                    {
                        agent.SetDestination(currentTarget.position);
                    }
                    agent.isStopped = false;
                    isAttacking = false;
                    
                }
                else
                {
                    agent.isStopped = true;
                    isAttacking = true;
                    Vector3 dir = (currentTarget.position - transform.position).normalized;
                    if (dir != Vector3.zero)
                    {
                        Quaternion lookRot = Quaternion.LookRotation(new Vector3(dir.x, 0, dir.z));
                        transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * 5f);
                    }
                    
                }   
            }
        }
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

        if(role == Role.Tank || role == Role.Warrior)
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

    public void TakeDamage(int dmg)
    {
        _hp -= dmg;
        if (_hp <= 0)
        {
            Destroy(gameObject);
        }
    }

    public int GetTypeValue()
    {
        return _type;
    }

    private enum Role
    {
       None,
       Archer,
       Warrior,
       Tank,
       Wizard
    }   
}