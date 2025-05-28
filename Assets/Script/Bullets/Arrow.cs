using UnityEngine;

public class Arrow : MonoBehaviour
{
    private Rigidbody rb;

    [SerializeField] private LayerMask ignoreLayer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        rb.linearVelocity = -transform.up.normalized * 1.60f;
        Destroy(gameObject, 2f);
    }

    private void Update()
    {

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
