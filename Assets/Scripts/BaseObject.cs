using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BaseObject : MonoBehaviour
{
    [SerializeField] protected float _minLifetime = 2f;
    [SerializeField] protected float _maxLifetime = 5f;

    protected Rigidbody _rigidbody;

    public event Action<BaseObject> LifetimeExpired;

    protected virtual void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    protected void CommandRelease(BaseObject objectUnit)
    {
        LifetimeExpired?.Invoke(objectUnit);
        Debug.Log("Event Called");
    }
}
