using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This is a generic class. It implements a singleton so I don't have to keep doing it. It's also a MonoBehavior which makes things easy.
public class MonoSingleton<T> : MonoBehaviour where T : Component
{
    private static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject singleton = new GameObject();
                //Add name so I can easily distinguish
                singleton.name = typeof(T).Name;
                singleton.AddComponent<T>();
            }
            
            return _instance;
        }
    }

    public virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
            DontDestroyOnLoad(gameObject);
            return;
        }
        
        Destroy(gameObject);
    }
}
