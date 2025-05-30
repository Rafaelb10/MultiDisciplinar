using UnityEngine;

public class BulletWizard : MonoBehaviour
{
    private int _type;
    private Rigidbody rb;
    [SerializeField] private string ignoreTagName;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        Destroy(gameObject, 2f);
    }
    public void SetDirection(Vector3 direction)
    {
        rb.linearVelocity = direction * 1.60f;
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
            damageable.TakeDamage(1);
        }

        Destroy(gameObject);
    }

    public void SetType(int type)
    {
        _type = type;
    }
}
