using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T m_instance = null;

    public static T Instance
    {
        get
        {
            return m_instance;
        }
    }

    protected virtual void Awake()
    {
        if (m_instance)
        {
            Debug.LogFormat("Instance of {0} already exists! Destroying new instance.", name);
            Destroy(gameObject);
        }
        else
        {
            Debug.LogFormat("Instance of {0} successfully created!", name);
            m_instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
    }

    protected virtual void OnDestroy()
    {
        if (m_instance == this as T)
        {
            m_instance = null;
        }
    }
}