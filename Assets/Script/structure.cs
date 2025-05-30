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
            //if (_type == 0)
            //    FindFirstObjectByType<UiManager>().LoseHp(25f);

            //if (_type == 1)
            //    FindFirstObjectByType<EnemyManage>().LoseHp(25f);

            Destroy(gameObject);
        }
    }

    public int GetTypeValue()
    {
        return _type;
    }
}
