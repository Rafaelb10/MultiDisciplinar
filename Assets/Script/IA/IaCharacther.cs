using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class IaCharacter : MonoBehaviour, IDemageble
{
    [Header("Config")]
    [SerializeField] private float detectionRadius = 10f;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private int damage = 5;
    [SerializeField] private float hp = 10f;
    [SerializeField] private int _type = 0;

    [SerializeField] private LayerMask detectionLayer;
    [SerializeField] private Role role;

    [SerializeField] private GameObject _prefabBullet;

    private NavMeshAgent agent;
    private AnimationScript animationScript;
    private Rigidbody rb;

    private Transform currentTarget;
    private bool isDead = false;
    private bool isAttacking = false;
    private bool isAllyOnWay = false;
    private bool animationOnCooldown = false;

    public bool IsAttacking { get => isAttacking; set => isAttacking = value; }

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animationScript = GetComponent<AnimationScript>();
        rb = GetComponent<Rigidbody>();

        agent.stoppingDistance = attackRange;
        animationScript.PlaySpawn();
    }

    private void Update()
    {
        if (isDead) return;

        UpdateTarget();
        HandleMovementAndAttack();
    }

    private void UpdateTarget()
    {
        // Encontrar o inimigo mais próximo, com base no tipo e no raio de detecção
        Transform enemyTarget = FindClosestTarget<IaCharacter>(detectionRadius, enemy => enemy.GetTypeValue() != this._type);
        Transform structureTarget = FindClosestTarget<structure>(Mathf.Infinity, s => s.GetTypeValue() != this._type);

        currentTarget = enemyTarget != null ? enemyTarget : structureTarget;
    }

    private void HandleMovementAndAttack()
    {
        if (currentTarget == null)
        {
            StopAgent();
            animationScript.PlayWalk(false);
            return;
        }

        float distance = Vector3.Distance(transform.position, currentTarget.position);
        Vector3 direction = (currentTarget.position - transform.position).normalized;

        FaceMovementDirection();

        if (distance > attackRange)
        {
            MoveTo(currentTarget.position);
        }
        else
        {
            Attack(direction);
        }
    }

    private void MoveTo(Vector3 position)
    {
        agent.isStopped = false;
        agent.SetDestination(position);
        animationScript.PlayWalk(true);
        IsAttacking = false;
    }

    private void Attack(Vector3 direction)
    {
        agent.isStopped = true;
        animationScript.PlayWalk(false);

        if (role == Role.Archer || role == Role.Wizard)
        {
            FireProjectile(direction);
        }
        else if (role == Role.Warrior || role == Role.Barbaro)
        {
            if (!animationOnCooldown)
            {
                animationScript.PlayAttack();
                StartCoroutine(AnimationCooldown());
                IDemageble target = currentTarget.GetComponent<IDemageble>();
                if (target != null)
                    StartCoroutine(AttackWithDelay(target));
            }
        }
    }

    private void FireProjectile(Vector3 direction)
    {
        if (_prefabBullet != null)
        {
            if (!animationOnCooldown)
            {
                animationScript.PlayAttack();
                StartCoroutine(AnimationCooldown());

                GameObject bullet = Instantiate(_prefabBullet, transform.position, Quaternion.LookRotation(-transform.forward));
                //Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
                bullet.GetComponent<BulletDamage>().SetType(_type);
                bullet.GetComponent<BulletDamage>().SetDamage(damage);

                //if (bulletRb != null)
                //{
                //    bulletRb.AddForce(direction * 5f, ForceMode.VelocityChange);
                //}

                Destroy(bullet, 5f); 
            }
        }
    }

    private void FaceMovementDirection()
    {
        Vector3 velocity = agent.velocity;

        if (velocity.sqrMagnitude > 0.01f)
        {
            Quaternion lookRotation = Quaternion.LookRotation(-velocity.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
        }
    }

    private void StopAgent()
    {
        agent.isStopped = true;
        agent.ResetPath();
        agent.speed = 0f;
    }

    private IEnumerator AttackWithDelay(IDemageble target)
    {
        IsAttacking = true;
        yield return new WaitForSeconds(0.5f);

        if (target != null)
        {
            target.TakeDamage(damage);
        }

        IsAttacking = false;
    }

    private IEnumerator AnimationCooldown()
    {
        animationOnCooldown = true;
        yield return new WaitForSeconds(2);
        animationOnCooldown = false;
    }

    private Transform FindClosestTarget<T>(float radius, System.Func<T, bool> predicate) where T : MonoBehaviour
    {
        T[] allTargets = GameObject.FindObjectsOfType<T>();
        Transform closest = null;
        float minDistance = Mathf.Infinity;

        foreach (T target in allTargets)
        {
            if (!predicate(target)) continue;

            float dist = Vector3.Distance(transform.position, target.transform.position);
            if (dist < minDistance && dist <= radius)
            {
                minDistance = dist;
                closest = target.transform;
            }
        }

        return closest;
    }

    public void TakeDamage(float dmg)
    {
        if (isDead) return;

        hp -= dmg;
        if (hp <= 0)
        {
            isDead = true;
            animationScript.PlayDeath();
        }
    }

    public int GetTypeValue() => _type;

    public void SetEnemy()
    {
        _type = 1;
    }
    public void SetPlayer()
    {
        _type = 0;
    }

    private enum Role
    {
        None,
        Archer,
        Warrior,
        Barbaro,
        Wizard
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (role == Role.Warrior || role == Role.Barbaro)
        {
            GameObject other = collision.gameObject;

            if (other.TryGetComponent<structure>(out var structureChar))
            {
                if (structureChar.GetTypeValue() == _type)
                    return;
            }

            if (other.TryGetComponent<IaCharacter>(out var iaChar))
            {
                if (iaChar.GetTypeValue() == _type)
                    return;
            }

            if (other.TryGetComponent<IDemageble>(out var damageable))
            {
                damageable.TakeDamage(damage);
            }
        }
    }

    public void SetDamage(int _damage)
    {
        damage = _damage;
    }
    public void SetHp(int _hp)
    {
        hp = _hp;
    }
}