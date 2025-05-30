using UnityEngine;

public class Arrow : MonoBehaviour
{
    private int _type;
    private Rigidbody rb;
    [SerializeField] private string ignoreLayerName;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        rb.linearVelocity = transform.up.normalized * 1.60f;
        Destroy(gameObject, 2f);
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
