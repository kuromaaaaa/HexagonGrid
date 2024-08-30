using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonMonoBehaviourOvertake<T> : MonoBehaviour where T : SingletonMonoBehaviourOvertake<T>
{
    protected bool DDOL = false;
    private static T instance;
    public static T Instance
    {
        get { return instance; }
        set { instance = value; }
    }
    protected void Awake()
    {
        if (instance == null)
        {
            instance = (T)FindObjectOfType(typeof(T));
        }
        else
        {
            Destroy(instance.gameObject);
            instance = (T)FindObjectOfType(typeof(T));
        }
    }

    private void OnDestroy()
    {
        if (ReferenceEquals(this, instance))
            instance = null;
    }
    private void Start()
    {
        if (DDOL)
        {
            DontDestroyOnLoad(instance);
        }
    }
}
