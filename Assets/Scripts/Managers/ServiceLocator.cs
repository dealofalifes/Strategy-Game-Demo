using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServiceLocator : MonoBehaviour
{
    private static ServiceLocator _instance;
    public static ServiceLocator Instance => _instance;

    private Dictionary<Type, object> _services = new Dictionary<Type, object>();

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject); // Oyun boyunca sakla
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Register<T>(T service)
    {
        _services[typeof(T)] = service;
    }

    public T Get<T>()
    {
        return (T)_services[typeof(T)];
    }
}
