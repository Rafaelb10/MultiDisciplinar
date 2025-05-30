using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Weapon Config")]
    [SerializeField] private WeaponType weaponType = WeaponType.None;
    [SerializeField] private Arrow arrowPrefab;
    [SerializeField] private BulletWizard bulletWizardPrefab;

    [Header("Fire Rates")]
    [SerializeField] private float arrowRate = 1f;
    [SerializeField] private float wandRate = 1.5f;

    private IaCharacter iaCharacter;
    private int _type;

    private void Awake()
    {
        iaCharacter = GetComponentInParent<IaCharacter>();
    }

    private void Update()
    {
        if (iaCharacter == null) return;

        switch (weaponType)
        {
            case WeaponType.Bow:
                HandleAttack(nameof(FireArrow), arrowRate, iaCharacter.IsAttacking, FireArrow, StopArrow);
                break;

            case WeaponType.Wand:
                HandleAttack(nameof(FireWand), wandRate, iaCharacter.IsAttacking, FireWand, StopWand);
                break;

            case WeaponType.None:
                CancelInvoke(nameof(FireArrow));
                CancelInvoke(nameof(FireWand));
                break;
        }
    }

    private void HandleAttack(string methodName, float rate, bool isAttacking, System.Action start, System.Action stop)
    {
        if (isAttacking)
        {
            if (!IsInvoking(methodName))
                InvokeRepeating(methodName, 0f, rate);
        }
        else
        {
            stop.Invoke();
        }
    }
    private void FireArrow()
    {
        if (arrowPrefab == null) return;

        Quaternion offset = Quaternion.Euler(0, -90, 90);
        Quaternion rotation = transform.rotation * offset;
        Arrow bullet = Instantiate(arrowPrefab, transform.position, rotation);
        bullet.SetType(_type);
    }

    private void StopArrow()
    {
        CancelInvoke(nameof(FireArrow));
    }

    private void FireWand()
    {
        if (bulletWizardPrefab == null) return;

        BulletWizard bullet = Instantiate(bulletWizardPrefab, transform.position, Quaternion.identity);
        bullet.SetDirection(transform.forward);
        bullet.SetType(_type);
    }

    private void StopWand()
    {
        CancelInvoke(nameof(FireWand));
    }

    private enum WeaponType
    {
        None,
        Bow,
        Wand
    }

    public void SetType(int type)
    {
        _type = type;
    }
}