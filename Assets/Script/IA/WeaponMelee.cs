using System.Collections;
using UnityEngine;

public class WeaponMelee : MonoBehaviour
{
    private Collider _collider;
    [SerializeField] private string _ignoreTag;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != _ignoreTag)
        {
            IDemageble damageable = other.GetComponent<IDemageble>();

            if (damageable != null)
            {
                damageable.TakeDamage(1);
                _collider.isTrigger = false;
                StartCoroutine(TurnOffTrigger());
            }
        }
    }

    IEnumerator TurnOffTrigger()
    {
        yield return new WaitForSeconds(1f);
        _collider.isTrigger = true;
    }
}
