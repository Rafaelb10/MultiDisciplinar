using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private TypeOfWeapon typeOfWeapon;
    // private GameObject weaponHandler;
    [SerializeField] private Arrow arrowPreFab;
    [SerializeField] private BulletWizard bulletWizardPreFab; 
    private IaCharacther iaCharacther;

    private void Awake()
    {
        iaCharacther = GetComponentInParent<IaCharacther>();
    }

    private void Update()
    {
        if (typeOfWeapon == TypeOfWeapon.Bow)
        {
            if (iaCharacther.IsAttacking == true)
            {
                if (!IsInvoking("MakingArrow"))
                {
                    ShootArrow();
                }
            }
            else if (iaCharacther.IsAttacking == false)
            {
                StopShootArrow();
            }
        }
        else if (typeOfWeapon == TypeOfWeapon.Wand)
        {
            if(iaCharacther.IsAttacking == true)
            {
                if (!IsInvoking("MakingWand"))
                {
                    ShootWand();
                }
            }
            else if (iaCharacther.IsAttacking == false)
            {
                StopWand();             
            }
        }
        else if (typeOfWeapon == TypeOfWeapon.None)
        {
            CancelInvoke("ShootArrow");
            CancelInvoke("ShootWand");
        }
    }
    private void MakingArrow()
    {
        Quaternion offset = Quaternion.Euler(0, -90, 90);
        Quaternion newRot = transform.rotation * offset;
        Arrow arrow = Instantiate(arrowPreFab, transform.position, newRot);
       // Arrow arrow = Instantiate(arrowPreFab, transform.position, transform.rotation);

    }
    private void MakingWand()
    {
        BulletWizard bullet = Instantiate(bulletWizardPreFab, transform.position, Quaternion.identity);
        bullet.SetDirection(transform.forward);
    }

    private void ShootArrow()
    {
        InvokeRepeating("MakingArrow", 1f, 1f);
    }
    private void StopShootArrow()
    {
        CancelInvoke("MakingArrow");
    }

    private void ShootWand()
    {
        InvokeRepeating("MakingWand", 1.5f, 1.5f);
    }

    private void StopWand()
    {
        CancelInvoke("MakingWand");
    }
    private enum TypeOfWeapon
    {
        None,
        Bow,
        Wand
    }
}
