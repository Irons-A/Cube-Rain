using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Cube _prefab;
    [SerializeField] private bool _isSpawnerActive = false;
    [SerializeField] private float _spawnRate = 1f;
    [SerializeField] private bool _collectionCheck = true;
    [SerializeField] private int _poolCapacity = 10;
    [SerializeField] private int _maxPoolSize = 20;
    [SerializeField] private float _spawnHeight = 10f;
    [SerializeField] private float _minXSpawnPoint = -5f;
    [SerializeField] private float _maxXSpawnPoint = 5f;
    [SerializeField] private float _minZSpawnPoint = -5f;
    [SerializeField] private float _maxZSpawnPoint = 5f;

    private ObjectPool<Cube> _pool;
    private WaitForSeconds _spawnFrequency;

    private void Awake()
    {
        _pool = new ObjectPool<Cube>(CreateObject, OnGetFromPool, onReleaseToPool, OnDestroyPooledObject, 
            _collectionCheck, _poolCapacity, _maxPoolSize);
    }

    private void Start()
    {
        _isSpawnerActive = true;
        StartCoroutine(SpawnRoutine());
    }

    private Cube CreateObject()
    {
        Cube cubeInstance = Instantiate(_prefab);
        cubeInstance.ObjectPool = _pool;
        return cubeInstance;
    }

    private void OnGetFromPool(Cube cube)
    {
        float xSpawnPoint = Random.Range(_minXSpawnPoint, _maxXSpawnPoint);
        float zSpawnPoint = Random.Range(_minZSpawnPoint, _maxZSpawnPoint);

        cube.gameObject.SetActive(true);
        cube.transform.position = new Vector3(xSpawnPoint, _spawnHeight, zSpawnPoint);
    }

    private void onReleaseToPool (Cube cube)
    {
        cube.gameObject.SetActive(false);
    }

    private void OnDestroyPooledObject(Cube cube)
    {
        Destroy(cube.gameObject);
    }

    private IEnumerator SpawnRoutine()
    {
        _spawnFrequency = new WaitForSeconds(_spawnRate);

        while (_isSpawnerActive)
        {
            Cube cube = _pool.Get();
            yield return _spawnFrequency;
        }
    }
}