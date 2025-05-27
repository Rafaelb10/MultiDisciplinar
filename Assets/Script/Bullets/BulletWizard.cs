using UnityEngine;

public class BulletWizard : MonoBehaviour
{
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        Destroy(gameObject, 2f);
    }

    private void Update()
    {

    }

    public void SetDirection(Vector3 direction)
    {
        rb.linearVelocity = direction * 1.60f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        IDemageble damageable = collision.gameObject.GetComponent<IDemageble>();

        if (damageable != null)
        {
            damageable.TakeDamage(1);
            Destroy(gameObject);
        }
    }
}
