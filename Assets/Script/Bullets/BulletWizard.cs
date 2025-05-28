using UnityEngine;

public class BulletWizard : MonoBehaviour
{
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

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag(ignoreTagName))
        {
            IDemageble damageable = collision.gameObject.GetComponent<IDemageble>();
            
            if (damageable != null)
            {   
               damageable.TakeDamage(1);
               Destroy(gameObject);
            }
        }
    }
}
