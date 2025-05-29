using UnityEngine;

public class structure : MonoBehaviour, IDemageble
{
    [SerializeField] float Hp;
    [SerializeField] private int _type = 0;

    public void TakeDamage(float damage)
    {
       Hp = Hp - damage;
        if (Hp <= 0) 
        { 
            Destroy(gameObject);
        }
    }

    public int GetTypeValue()
    {
        return _type;
    }
    //garantia
}
