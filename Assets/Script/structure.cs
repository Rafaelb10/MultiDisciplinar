using UnityEngine;

public class structure : MonoBehaviour, IDemageble
{
    [SerializeField] int Hp;
    [SerializeField] private int _type = 0;

    public void TakeDamage(int damage)
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

}
