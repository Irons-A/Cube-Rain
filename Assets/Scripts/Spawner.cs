using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public abstract class Spawner : MonoBehaviour
{
    [SerializeField] protected BaseObject _prefab;
    [SerializeField] protected bool _isSpawnerActive = false;
    [SerializeField] protected float _spawnRate = 1f;
    [SerializeField] protected bool _collectionCheck = true;
    [SerializeField] protected int _poolCapacity = 10;
    [SerializeField] protected int _maxPoolSize = 10;

    protected ObjectPool<BaseObject> _pool;
    protected WaitForSeconds _spawnFrequency;
    protected List<BaseObject> _objects = new List<BaseObject>();

    protected void Awake()
    {
        _pool = new ObjectPool<BaseObject>(CreateObject, OnGetFromPool, OnReleaseToPool, OnDestroyPooledObject,
            _collectionCheck, _poolCapacity, _maxPoolSize);
    }

    protected void OnEnable()
    {
        foreach (var item in _objects)
        {
            item.LifetimeExpired += ReleaseObject;
        }
    }

    protected void OnDisable()
    {
        foreach (var item in _objects)
        {
            item.LifetimeExpired -= ReleaseObject;
        }
    }

    protected void Start()
    {
        _isSpawnerActive = true;
        StartCoroutine(SpawnRoutine());
    }

    protected BaseObject CreateObject()
    {
        BaseObject objectInstance = Instantiate(_prefab);
        objectInstance.LifetimeExpired += ReleaseObject;
        _objects.Add(objectInstance);
        return objectInstance;
    }

    protected abstract void OnGetFromPool(BaseObject objectUnit);

    protected void OnReleaseToPool(BaseObject objectUnit)
    {
        objectUnit.gameObject.SetActive(false);
    }

    protected void OnDestroyPooledObject(BaseObject objectUnit)
    {
        Destroy(objectUnit.gameObject);
    }

    protected void ReleaseObject(BaseObject objectUnit)
    {
        _pool.Release(objectUnit);
    }

    protected IEnumerator SpawnRoutine()
    {
        _spawnFrequency = new WaitForSeconds(_spawnRate);

        while (_isSpawnerActive)
        {
            _pool.Get();
            yield return _spawnFrequency;
        }
    }
}
