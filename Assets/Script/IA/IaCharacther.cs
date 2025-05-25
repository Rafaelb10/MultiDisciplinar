using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class IaCharacther : MonoBehaviour, IDemageble
{
    [SerializeField] private float priorityDetectionRadius = 10f;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private int damage = 1;
    [SerializeField] private int _type = 0;
    [SerializeField] private LayerMask detectionLayer;

    private bool isAttacking = false;

    private NavMeshAgent agent;
    private Transform mainTarget;
    private Transform priorityTarget;

    private int _hp = 10;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = attackRange;

        FindMainTarget();
    }

    private void Update()
    {
        DetectPriorityTargets();
        FindMainTarget();

        Transform currentTarget = priorityTarget != null ? priorityTarget : mainTarget;

        if (currentTarget == null)
        {
            // Nenhum alvo disponível, parar completamente
            agent.isStopped = true;
            agent.ResetPath();
            return;
        }

        float distance = Vector3.Distance(transform.position, currentTarget.position);

        if (distance > attackRange)
        {
            // Só define novo destino se for diferente
            if (!agent.hasPath || agent.destination != currentTarget.position)
            {
                agent.SetDestination(currentTarget.position);
            }

            agent.isStopped = false;
        }
        else
        {
            agent.isStopped = true;

            // Gira suavemente para o alvo
            Vector3 dir = (currentTarget.position - transform.position).normalized;
            if (dir != Vector3.zero)
            {
                Quaternion lookRot = Quaternion.LookRotation(new Vector3(dir.x, 0, dir.z));
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * 5f);
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
    }

    public void SetPriorityTarget(Transform newTarget)
    {
        priorityTarget = newTarget;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isAttacking)
        {
            IDemageble target = collision.gameObject.GetComponent<IDemageble>();
            if (target != null)
            {
                StartCoroutine(AttackAfterDelay(target));
            }
        }
    }

    private IEnumerator AttackAfterDelay(IDemageble target)
    {
        isAttacking = true;
        yield return new WaitForSeconds(0.5f);

        if (target != null)
        {
            target.TakeDamage(damage);
        }

        isAttacking = false;
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
}