using UnityEngine;

public class BulletDamage : MonoBehaviour
{
    private float _damage;
    private int _type;
    private float _speed = 5;

    void Start()
    {
        GetComponent<Rigidbody>().linearVelocity = transform.forward * _speed;
    }

    private void OnTriggerEnter(Collider collision)
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
            damageable.TakeDamage(_damage);
        }

        Destroy(gameObject);
    }

    public void SetType(int type)
    {
        _type = type;
    }

    public void SetDamage(int damage)
    {
        _damage = damage;
    }
}
