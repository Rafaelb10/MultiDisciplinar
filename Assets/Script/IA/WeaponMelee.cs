using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class WeaponMelee : MonoBehaviour
{
    private int _type;
    [SerializeField] private float triggerResetTime = 1f;

    private Collider weaponCollider;

    private void Awake()
    {
        weaponCollider = GetComponent<Collider>();
        weaponCollider.isTrigger = true;
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
            weaponCollider.isTrigger = false;
            StartCoroutine(ResetTrigger());
        }
    }

    private IEnumerator ResetTrigger()
    {
        yield return new WaitForSeconds(triggerResetTime);
        weaponCollider.isTrigger = true;
    }

    public void SetType(int type)
    {
        _type = type;
    }
}