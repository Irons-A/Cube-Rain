using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public abstract class Spawner : MonoBehaviour
{
    [SerializeField] protected BaseObject _prefab;
    [SerializeField] protected bool _isSpawnerActive = true;
    [SerializeField] protected bool _collectionCheck = true;
    [SerializeField] protected int _poolCapacity = 10;
    [SerializeField] protected int _maxPoolSize = 10;
    [SerializeField] protected float _statisticRefreshmentRate = 0.5f;

    protected ObjectPool<BaseObject> _pool;
    protected WaitForSeconds _spawnFrequency;
    protected WaitForSeconds _refreshmentRate;
    protected List<BaseObject> _objects = new List<BaseObject>();

    public event Action StatiscticUpdated;

    public int _alltimeObjects { get; private set; }
    public int _createdObjects { get; private set; }
    public int _activeObjects { get; private set; }

    protected void Awake()
    {
        _pool = new ObjectPool<BaseObject>(CreateObject, OnGetFromPool, OnReleaseToPool, OnDestroyPooledObject,
            _collectionCheck, _poolCapacity, _maxPoolSize);
    }

    protected virtual void OnEnable()
    {
        foreach (var item in _objects)
        {
            item.LifetimeExpired += ReleaseObject;
        }
    }

    protected virtual void OnDisable()
    {
        foreach (var item in _objects)
        {
            item.LifetimeExpired -= ReleaseObject;
        }
    }

    protected virtual void Start()
    {
        StartCoroutine(StatisticRoutine());
    }

    protected BaseObject CreateObject()
    {
        BaseObject objectInstance = Instantiate(_prefab);
        objectInstance.LifetimeExpired += ReleaseObject;
        _objects.Add(objectInstance);

        return objectInstance;
    }

    protected virtual void OnGetFromPool(BaseObject objectUnit)
    {
        _alltimeObjects++;
    }

    protected virtual void OnReleaseToPool(BaseObject objectUnit)
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

    private IEnumerator StatisticRoutine()
    {
        _refreshmentRate = new WaitForSeconds(_statisticRefreshmentRate);

        while (_isSpawnerActive)
        {
            yield return _refreshmentRate;

            _createdObjects = _pool.CountAll;
            _activeObjects = _pool.CountActive;
            StatiscticUpdated?.Invoke();
        }
    }
}
